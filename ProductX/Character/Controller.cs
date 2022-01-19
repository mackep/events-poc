using Controller;
using Events.Sending;
using Repository;

namespace ProductX.Character
{
    public class Controller : CharacterController
    {
        public Controller(ICharacterRepository repository, IEventSender sender) : base(repository, sender)
        {
        }

        protected override char Char => 'X';
    }
}
