using System;
using Events.Base;
using Events.Contracts;
using Model;

namespace Controller
{
    public static class CharacterExtensions
    {
        public static Event AsUpdatedEvent(this Character c)
        {
            return char.ToUpper(c.Char) switch
            {
                'X' => new XUpdated(c.Id),
                'Y' => new YUpdated(c.Id, c.Char, c.Version),
                'Z' => new ZUpdated(c.Id),
                _ => throw new ArgumentException($"Unknown character '{c}'")
            };
        }

        public static Event AsCreatedEvent(this Character c)
        {
            return char.ToUpper(c.Char) switch
            {
                'X' => new XCreated(c.Id),
                'Y' => new YCreated(c.Id, c.Char, c.Version),
                'Z' => new ZCreated(c.Id),
                _ => throw new ArgumentException($"Unknown character '{c}'")
            };
        }

        public static char ToggleCase(this char c)
        {
            return char.IsUpper(c) ?
                char.ToLower(c) :
                char.ToUpper(c);
        }
    }
}