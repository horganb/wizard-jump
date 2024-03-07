using GamGUI;
using UnityEngine;

namespace Gear
{
    public abstract class Gear : IChestReward
    {
        public abstract string Name();

        public abstract void Acquire();

        public Sprite GetSprite()
        {
            return GameGUI.Instance.spriteLibrary.GetSprite("Gear", SpriteName());
        }

        protected abstract string SpriteName();
    }
}