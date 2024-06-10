using GamGUI;
using Interactable;
using UnityEngine;

namespace Scrolls
{
    public abstract class Scroll : IChestReward
    {
        public void Acquire()
        {
            Player.Instance.Scroll = this;
        }

        public string Name()
        {
            return $"Scroll of {ScrollName()}";
        }

        public Sprite GetSprite()
        {
            return GameGUI.Instance.spriteLibrary.GetSprite("Scrolls", ScrollName());
        }

        protected abstract string ScrollName();

        public virtual void Cast(Vector2 worldPosition)
        {
        }
    }
}