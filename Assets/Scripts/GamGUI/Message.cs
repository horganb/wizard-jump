using UnityEngine;

namespace GamGUI
{
    public class Message : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}