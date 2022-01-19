using Controller;
using Events.Sending;
using Repository;

namespace ProductZ.Character
{
    public class Controller : CharacterController
    {
        public Controller(ICharacterRepository repository, IEventSender sender) : base(repository, sender)
        {
        }

        protected override char Char => 'Z';
    }
}
