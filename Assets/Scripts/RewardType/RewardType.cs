using Interactable;
using UnityEngine;

namespace RewardType
{
    public abstract class RewardType : MonoBehaviour
    {
        public abstract IChestReward[] GenerateRewards();
        public abstract bool CanSpawn();
        public abstract int GetCost();
    }
}