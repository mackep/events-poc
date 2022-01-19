namespace Events.Handling.AzureServiceBus
{
    public enum EventHandlingResult
    {
        PotentiallyIntermittentFailure,
        Failure,
        Success
    }
}