using GamGUI;
using UnityEngine;

namespace Scrolls
{
    public abstract class Scroll : IChestReward
    {
        protected Player Player;

        public Scroll()
        {
            Player = Object.FindObjectOfType<Player>();
        }

        public void Acquire()
        {
            Player.Scroll = this;
        }

        public string Name()
        {
            return $"Scroll of {ScrollName()}";
        }

        public Sprite GetSprite()
        {
            return Object.FindObjectOfType<GameGUI>().spriteLibrary.GetSprite("Scrolls", ScrollName());
        }

        protected abstract string ScrollName();

        public virtual void Cast(Vector2 worldPosition)
        {
        }
    }
}