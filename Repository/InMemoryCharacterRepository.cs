using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace Repository
{
    public class InMemoryCharacterRepository : ICharacterRepository
    {
        private readonly object _lock = new();
        private readonly Random _random = new();

        private readonly Dictionary<Guid, Character> _characters = new();
        private readonly Dictionary<Guid, long> _sequence = new();

        public Task Upsert(Character character)
        {
            lock (_lock)
            {
                _characters[character.Id] = character;
                _sequence[character.Id] = DateTime.UtcNow.Ticks;
            }

            return Task.CompletedTask;
        }

        public IEnumerable<Character> GetAll(params char[] chars)
        {
            lock (_lock)
            {
                if (chars.Length == 0)
                    return _characters.Values;

                return _characters.Where(pair => chars.Contains(pair.Value.Char)).Select(pair => pair.Value).ToList();
            }
        }

        public Character Get(Guid id)
        {
            lock (_lock)
            {
                if (_characters.ContainsKey(id))
                    return _characters[id];
            }

            return null;
        }

        public Character GetRandom(params char[] chars)
        {
            lock (_lock)
            {
                if (!_characters.Any())
                    return null;

                var characters = _characters.Where(pair => chars.Contains(pair.Value.Char)).ToList();
                return characters[_random.Next(characters.Count)].Value;
            }
        }

        public IOrderedEnumerable<Character> GetSequence()
        {
            lock (_lock)
            {
                return _characters.Select(pair => pair.Value).OrderBy(pair => _sequence[pair.Id]);
            }
        }
    }
}
