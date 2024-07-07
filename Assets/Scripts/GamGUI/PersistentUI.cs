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

        public void SetLoading(bool loading)
        {
            transitionAnimator.SetBool(Loading, loading);
        }

        public void LoadScene(string sceneName)
        {
            SetLoading(true);
            StartCoroutine(LoadSceneAfterDelay(sceneName));
        }

        private IEnumerator LoadSceneAfterDelay(string sceneName)
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(sceneName);
            SetLoading(false);
        }
    }
}