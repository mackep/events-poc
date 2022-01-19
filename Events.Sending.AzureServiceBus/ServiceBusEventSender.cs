using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Azure.Messaging.ServiceBus;
using Events.Base;
using Events.Infrastructure.AzureServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Events.Sending.AzureServiceBus
{
    public class ServiceBusEventSender : IEventSender, IAsyncDisposable
    {
        private readonly ILogger<ServiceBusEventSender> _logger;
        private readonly ServiceBusSender _sender;
        private readonly string _sourceIdentifier;

        public ServiceBusEventSender(IOptions<AzureServiceBusConfiguration> config,
            ILogger<ServiceBusEventSender> logger)
        {
            _logger = logger;
            _sourceIdentifier = config.Value.SystemId ?? throw new ArgumentException(
                $"{nameof(AzureServiceBusConfiguration.SystemId)} must be provided in order to send events");

            var client = new ServiceBusClient(config.Value.SendConnectionString, new ServiceBusClientOptions
            {
                /*
                 * ServiceBusTransportType.AmqpTcp does not seem to work. Are ports 5671/5672
                 * blocked by some policy by Group IT?
                 */
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });
            _sender = client.CreateSender(config.Value.Topic);
        }

        public Task Send(params Event[] events)
        {
            if (events.Length == 1)
                return SendSingle(events.Single());

            if (events.Length > 1)
                return SendMany(events);

            return Task.CompletedTask;
        }

        private async Task SendSingle(Event evnt)
        {
            try
            {
                await _sender.SendMessageAsync(evnt.AsServiceBusMessage(_sourceIdentifier));
            }
            catch (Exception ex)
            {
                var exception = new SendEventException(evnt, ex);

                _logger.LogError(exception.Message);

                throw exception;
            }
        }

        private async Task SendMany(Event[] events)
        {
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                await SendAll(events);

                scope.Complete();
            }
            catch (Exception ex)
            {
                var exception = new SendEventException(events, ex);

                _logger.LogError(exception.Message);

                throw exception;
            }
        }

        private async Task SendAll(Event[] events)
        {
            var remaining = events;
            do
            {
                remaining = await SendInBatch(remaining);
            } while (remaining.Any());
        }

        private async Task<Event[]> SendInBatch(Event[] events)
        {
            var remaining = new List<Event>();

            using var batch = await _sender.CreateMessageBatchAsync();

            for (var i = 0; i < events.Length; i++)
            {
                var evnt = events[i];

                if (!batch.TryAddMessage(evnt.AsServiceBusMessage(_sourceIdentifier)))
                {
                    remaining.AddRange(events[i..^1]);
                    break;
                }
            }

            await _sender.SendMessagesAsync(batch);

            return remaining.ToArray();
        }

        public ValueTask DisposeAsync()
        {
            return _sender.DisposeAsync();
        }
    }
}