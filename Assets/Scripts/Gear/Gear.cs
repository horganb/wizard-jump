using GamGUI;
using Interactable;
using UnityEngine;

namespace Gear
{
    public abstract class Gear : IChestReward
    {
        public string Name()
        {
            // TODO: append "Upgraded" to the name if the player already has it
            return DisplayName();
        }

        public abstract void Acquire();

        public Sprite GetSprite()
        {
            return GameGUI.Instance.spriteLibrary.GetSprite("Gear", SpriteName());
        }

        public abstract string DisplayName();

        protected abstract string SpriteName();
    }
}