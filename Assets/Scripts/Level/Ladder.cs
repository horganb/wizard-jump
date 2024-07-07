using GamGUI;

namespace Level
{
    public class Ladder : Interactable.Interactable
    {
        public override void Interact(bool alternate)
        {
            PersistentUI.Instance.LoadScene(PersistentUI.Level);
        }

        protected override string PromptText()
        {
            return "Begin Run";
        }
    }
}