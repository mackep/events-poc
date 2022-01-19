using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Events.Infrastructure.AzureServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Events.Handling.AzureServiceBus
{
    public class ServiceBusEventListener : IHostedService, IAsyncDisposable
    {
        private readonly ServiceBusEventDeserializer _deserializer;
        private readonly IEventMediator _eventMediator;
        private readonly ILogger<ServiceBusEventListener> _logger;
        private readonly ServiceBusReceiver _receiver;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private const int MaxNumberOfEventsPerBatch = 10;
        private readonly TimeSpan _maxWaitTimeToFillBatch = TimeSpan.FromSeconds(5);

        public ServiceBusEventListener(ServiceBusEventDeserializer deserializer,
            IEventMediator eventMediator,
            IOptions<AzureServiceBusConfiguration> config,
            ILogger<ServiceBusEventListener> logger)
        {
            _deserializer = deserializer;
            _eventMediator = eventMediator;
            _logger = logger;

            _cancellationTokenSource = new CancellationTokenSource();
            _receiver = new ServiceBusClient(config.Value.ReceiveConnectionString, new ServiceBusClientOptions
            {
                /*
                 * When hosted on-prem, TCP connections on port 5671/5672 seem to be blocked.
                 * Due to this, we AMQP Web Sockets as a fallback solution for connectivity.
                 */
                TransportType = ServiceBusTransportType.AmqpWebSockets,
            }).CreateReceiver(config.Value.Topic, config.Value.SystemId);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(StartAsync, cancellationToken);

            _logger.LogInformation(
                $"Successfully started event listener for {_receiver.EntityPath}. Listening for events: {string.Join(", ", _eventMediator.SupportedEventTypes.Select(e => e.Name))}");

            return Task.CompletedTask;
        }

        public async Task StartAsync()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var messages = await _receiver.ReceiveMessagesAsync(MaxNumberOfEventsPerBatch, _maxWaitTimeToFillBatch, _cancellationTokenSource.Token);

                await Handle(messages);
            }
        }

        private async Task Handle(IEnumerable<ServiceBusReceivedMessage> messages)
        {
            var responses = new List<Task>();

            foreach (var msg in messages)
            {
                var result = await Handle(msg);
                var response = _receiver.Respond(result, msg);

                responses.Add(response);
            }

            await Task.WhenAll(responses);
        }

        private async Task<EventHandlingResult> Handle(ServiceBusReceivedMessage msg)
        {
            try
            {
                var evnt = _deserializer.Deserialize(msg);

                await _eventMediator.Handle(evnt);
            }
            catch (Exception ex) when
            (ex is EventDeserializationException ||
             ex is EventHandlerNotFoundException)
            {
                _logger.LogCritical(ex, $"Failed to handle event: {ex.Message}. Moving event to dead-letter queue.");

                return EventHandlingResult.Failure;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Failed to handle event: {ex.Message} (attempt #{msg.DeliveryCount})");

                return EventHandlingResult.PotentiallyIntermittentFailure;
            }

            return EventHandlingResult.Success;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (!_cancellationTokenSource.Token.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            await _receiver.CloseAsync(cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            if (!_cancellationTokenSource.Token.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            return _receiver.DisposeAsync();
        }
    }
}
