using UnityEngine;

namespace Singletons
{
    public class AudioLibrary : SingletonMonoBehaviour<AudioLibrary>
    {
        public AudioClip teleport;
        public AudioClip fireball;
        public AudioClip iceSpike;
    }
}