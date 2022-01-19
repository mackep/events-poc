using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Events.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Handling
{
    public class EventMediator : IEventMediator
    {
        private readonly Dictionary<Type, Type> _eventHandlerTypes;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventMediator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            _eventHandlerTypes = new Dictionary<Type, Type>();

            RegisterHandlerTypes();
        }

        public IReadOnlyCollection<Type> SupportedEventTypes => _eventHandlerTypes.Keys;

        public async Task Handle(Event evnt)
        {
            if (!_eventHandlerTypes.TryGetValue(evnt.GetType(), out var eventHandlerType))
                throw new EventHandlerNotFoundException(evnt.GetType());

            try
            {
                using var scope = _scopeFactory.CreateScope();

                var eventHandler = (IEventHandler) scope.ServiceProvider.GetRequiredService(eventHandlerType);

                await eventHandler.Handle(evnt);
            }
            catch (Exception ex)
            {
                throw new EventHandlingException(evnt, ex);
            }
        }

        private void RegisterHandlerTypes()
        {
            using var scope = _scopeFactory.CreateScope();

            var registrations = scope.ServiceProvider.GetServices<IEventHandler>();
            foreach (var handler in registrations)
            {
                var eventHandlerType = handler.GetType();
                var eventType = handler.EventType;

                if (_eventHandlerTypes.ContainsKey(eventType))
                    throw new ArgumentException(
                        $"Failed to register {eventHandlerType.Name} as event handler for {eventType}." +
                        $"There's already a registered handler {_eventHandlerTypes[eventType].Name} for this event.");

                _eventHandlerTypes.Add(eventType, eventHandlerType);
            }
        }
    }
}