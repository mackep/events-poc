using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace Repository
{
    public interface ICharacterRepository
    {
        Task Upsert(Character character);
        IEnumerable<Character> GetAll(params char[] chars);
        Character Get(Guid id);
        Character GetRandom(params char[] chars);
        IOrderedEnumerable<Character> GetSequence();
    }
}