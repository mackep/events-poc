using System.Threading.Tasks;
using Client;
using Events.Contracts;
using Events.Handling;
using Repository;

namespace Handlers
{
    public class ZUpdatedEventHandler : DomainEventHandler<ZUpdated>
    {
        private readonly ICharacterRepository _repository;
        private readonly ZApiClient _apiClient;

        public ZUpdatedEventHandler(ICharacterRepository repository, ZApiClient apiClient)
        {
            _repository = repository;
            _apiClient = apiClient;
        }

        public override async Task Handle(ZUpdated evnt)
        {
            var updated = await _apiClient.GetZ(evnt.Id);

            await _repository.Upsert(updated);
        }
    }
}