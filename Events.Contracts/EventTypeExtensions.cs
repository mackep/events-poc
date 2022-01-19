using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Events.Base;

namespace Events.Contracts
{
    public static class EventTypes
    {
        /// <summary>
        /// Retrieves all concrete event types in this assembly.
        /// </summary>
        /// <returns>A list of all event types</returns>
        public static IEnumerable<Type> AllInContractsLibrary()
        {
            return Assembly.GetAssembly(typeof(XCreated))?
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(Event)));
        }
    }
}