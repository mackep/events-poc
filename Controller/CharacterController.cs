using System;
using System.Threading.Tasks;
using Events.Sending;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;

namespace Controller
{
    [ApiController]
    [Route("")]
    public abstract class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _repository;
        private readonly IEventSender _sender;

        protected abstract char Char { get; }
        protected char[] Characters => new[] {char.ToLower(Char), char.ToUpper(Char)};

        protected CharacterController(ICharacterRepository repository,
            IEventSender sender)
        {
            _repository = repository;
            _sender = sender;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult Get([FromRoute] Guid id)
        {
            var character = _repository.Get(id);

            if (character == null)
                return NoContent();

            return new OkObjectResult(character);
        }

        [HttpPut]
        public async Task<ActionResult> AddRandomCharacter()
        {
            var character = new Character(Guid.NewGuid(), char.ToUpper(Char), 1);

            await _repository.Upsert(character);

            await _sender.Send(character.AsCreatedEvent());

            return new OkObjectResult(_repository.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult> UpdateRandomCharacter()
        {
            var toUpdate = _repository.GetRandom(Characters);
            if (toUpdate == null)
                return BadRequest();

            var updated = new Character(toUpdate.Id, toUpdate.Char.ToggleCase(), toUpdate.Version + 1);

            await _repository.Upsert(updated);

            await _sender.Send(updated.AsUpdatedEvent());

            return new OkObjectResult(_repository.GetAll());
        }
    }
}
