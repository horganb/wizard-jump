using System.Collections;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamGUI
{
    public class PersistentUI : SingletonMonoBehaviour<PersistentUI>
    {
        private static readonly int Loading = Animator.StringToHash("Loading");
        public static string Level = "Level";
        public static string Camp = "Cave";
        public Animator transitionAnimator;
        private string _loadingScene;

        protected override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                base.Awake();
            }
        }

        public void LoadScene(string sceneName)
        {
            _loadingScene = sceneName;
            transitionAnimator.SetBool(Loading, true);
        }

        public IEnumerator FinishLoadingScene()
        {
            Time.timeScale = 1f;
            var asyncLoad = SceneManager.LoadSceneAsync(_loadingScene);
            while (!asyncLoad.isDone) yield return null;
            yield return new WaitForSeconds(0.01f);
            transitionAnimator.SetBool(Loading, false);
        }
    }
}