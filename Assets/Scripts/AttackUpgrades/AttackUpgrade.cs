using GamGUI;
using Interactable;
using UnityEngine;

namespace AttackUpgrades
{
    public abstract class AttackUpgrade : IChestReward
    {
        public abstract string Name();

        public abstract void Acquire();

        public Sprite GetSprite()
        {
            return GameGUI.Instance.spriteLibrary.GetSprite("Attack Upgrades", Name());
        }
    }
}