using UnityEngine;

namespace Singletons
{
    [CreateAssetMenu]
    public class GameStages : ScriptableObject
    {
        public Stage[] stages;
    }
}