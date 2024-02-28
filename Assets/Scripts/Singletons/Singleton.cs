using UnityEngine;

namespace Singletons
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : class
    {
        public static T Instance;

        protected virtual void Awake()
        {
            Instance = this as T;
        }
    }
}