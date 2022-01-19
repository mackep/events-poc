using System.Threading.Tasks;
using Events.Contracts;
using Events.Handling;
using Model;
using Repository;

namespace Handlers
{
    public class YCreatedEventHandler : DomainEventHandler<YCreated>
    {
        private readonly ICharacterRepository _repository;

        public YCreatedEventHandler(ICharacterRepository repository)
        {
            _repository = repository;
        }

        public override async Task Handle(YCreated evnt)
        {
            await _repository.Upsert(new Character(evnt.Id, evnt.NewValue, evnt.EntityVersion));
        }
    }
}
