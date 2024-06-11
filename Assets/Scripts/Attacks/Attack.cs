using GamGUI;
using Interactable;
using UnityEngine;

namespace Attacks
{
    public abstract class Attack : IChestReward
    {
        public abstract string Name();

        public void Acquire()
        {
            Player.Instance.ActiveAttack = this;
        }

        public Sprite GetSprite()
        {
            return GameGUI.Instance.spriteLibrary.GetSprite("Attacks", Name());
        }

        public abstract GameObject GetPrefab();

        public abstract AudioClip GetClip();
    }
}