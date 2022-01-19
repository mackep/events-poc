using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using static System.String;

namespace Repository
{
    public class StateLoggingDecorator : ICharacterRepository
    {
        private readonly ICharacterRepository _inner;
        private readonly object _lock = new();
        private int _updateCount;

        public StateLoggingDecorator(ICharacterRepository inner)
        {
            _inner = inner;
        }

        public async Task Upsert(Character character)
        {
            await _inner.Upsert(character);

            LogState();
        }

        public IEnumerable<Character> GetAll(params char[] chars)
        {
            return _inner.GetAll(chars);
        }

        public Character Get(Guid id)
        {
            return _inner.Get(id);
        }

        public IOrderedEnumerable<Character> GetSequence()
        {
            return _inner.GetSequence();
        }

        public Character GetRandom(params char[] chars)
        {
            return _inner.GetRandom(chars);
        }

        private void LogState()
        {
            var current = _inner.GetAll().ToList();
            var sortedBySequence = Concat(_inner.GetSequence().Select(c => c.Char));
            var sortedAlphabetically = Concat(current.OrderBy(pair => pair.Char).Select(pair => pair.Char));

            lock (_lock)
            {
                _updateCount++;
                WriteLineWithColoredLetters($"Characters: {sortedBySequence}\tSorted: {sortedAlphabetically}\tTotal number of updates: {_updateCount}");
            }
        }

        private void WriteLineWithColoredLetters(string message)
        {
            foreach(var letter in message)
            {
                if (letter == 'X' || letter == 'x')
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(letter);
                    Console.ResetColor();
                    continue;
                }

                if (letter == 'Y' || letter == 'y')
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(letter);
                    Console.ResetColor();
                    continue;
                }

                if (letter == 'Z' || letter == 'z')
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(letter);
                    Console.ResetColor();
                    continue;
                }

                Console.Write(letter);
            }

            Console.WriteLine();
        }
    }
}