using GamGUI;
using UnityEngine;

namespace Special
{
    public abstract class Special : IChestReward
    {
        protected AudioLibrary AudioLibrary;
        protected Player Player;

        public Special()
        {
            Player = Object.FindObjectOfType<Player>();
            AudioLibrary = Object.FindObjectOfType<AudioLibrary>();
        }

        public abstract string Name();

        public void Acquire()
        {
            Player.Special = this;
        }

        public Sprite GetSprite()
        {
            return Object.FindObjectOfType<GameGUI>().spriteLibrary.GetSprite("Specials", Name());
        }

        public virtual void Cast(Vector2 worldPosition)
        {
        }

        public virtual void CastStart()
        {
        }

        public virtual void CastEnd()
        {
        }

        public virtual void Update()
        {
        }
    }
}