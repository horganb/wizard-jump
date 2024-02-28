using GamGUI;
using UnityEngine;

namespace Special
{
    public abstract class Special : IChestReward
    {
        public abstract string Name();

        public void Acquire()
        {
            Player.Instance.Special = this;
        }

        public Sprite GetSprite()
        {
            return GameGUI.Instance.spriteLibrary.GetSprite("Specials", Name());
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