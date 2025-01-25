using GGJ.Player;

namespace GGJ.Prop
{
    public interface IInteractible
    {
        public void Interact(PlayerController pc);
        public bool CanInteract(PlayerController pc);
    }
}