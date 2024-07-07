using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamGUI
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Level");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}