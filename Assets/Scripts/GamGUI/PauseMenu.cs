using Singletons;
using UnityEngine;

namespace GamGUI
{
    public class PauseMenu : SingletonMonoBehaviour<PauseMenu>
    {
        public GameObject menuObject;

        public void Toggle()
        {
            if (menuObject.activeSelf)
                Resume();
            else
                Close();
        }

        public void Close()
        {
            Time.timeScale = 0f;
            menuObject.SetActive(true);
        }

        public void Resume()
        {
            menuObject.SetActive(false);
            Time.timeScale = 1f;
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}