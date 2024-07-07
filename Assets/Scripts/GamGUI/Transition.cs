using UnityEngine;

namespace GamGUI
{
    public class Transition : MonoBehaviour
    {
        public void OnComplete()
        {
            StartCoroutine(PersistentUI.Instance.FinishLoadingScene());
        }
    }
}