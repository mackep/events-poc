using System.Threading.Tasks;
using Events.Contracts;
using Events.Handling;
using Model;
using Repository;

namespace Handlers
{
    public class YUpdatedEventHandler : DomainEventHandler<YUpdated>
    {
        private readonly ICharacterRepository _repository;

        public YUpdatedEventHandler(ICharacterRepository repository)
        {
            _repository = repository;
        }

        public override async Task Handle(YUpdated evnt)
        {
            await _repository.Upsert(new Character(evnt.Id, evnt.NewValue, evnt.EntityVersion));
        }
    }
}