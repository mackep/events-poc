using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Events.Handling.AzureServiceBus
{
    public static class ServiceBusReceiverExtensions
    {
        public static Task Respond(this ServiceBusReceiver receiver,
            EventHandlingResult result,
            ServiceBusReceivedMessage msg)
        {
            return result switch
            {
                EventHandlingResult.PotentiallyIntermittentFailure => receiver.AbandonMessageAsync(msg),
                EventHandlingResult.Failure => receiver.DeadLetterMessageAsync(msg),
                EventHandlingResult.Success => receiver.CompleteMessageAsync(msg),
                _ => throw new ArgumentOutOfRangeException($"Unknown result {result}")
            };
        }
    }
}