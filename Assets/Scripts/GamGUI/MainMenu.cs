using UnityEngine;

namespace GamGUI
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            PersistentUI.Instance.LoadScene(PersistentUI.Level);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}