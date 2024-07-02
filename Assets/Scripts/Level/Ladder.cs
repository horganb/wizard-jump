using UnityEngine.SceneManagement;

namespace Level
{
    public class Ladder : Interactable.Interactable
    {
        public override void Interact(bool alternate)
        {
            SceneManager.LoadScene("Level");
        }

        protected override string PromptText()
        {
            return "Begin Run";
        }
    }
}