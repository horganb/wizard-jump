using GamGUI;
using Interactable;
using UnityEngine;

namespace Special
{
    public abstract class Special : IChestReward
    {
        public abstract string Name();

        public void Acquire()
        {
            if (Player.Instance.Special == null)
                GameGUI.Instance.DisplayMessage("New ability added!");
            else
                GameGUI.Instance.DisplayMessage("Ability replaced!", GameGUI.MessageTone.Neutral);
            Player.Instance.Special = this;
            Player.Instance.orbs = Player.Instance.maxOrbs;
        }

        public Sprite GetSprite()
        {
            return GameGUI.Instance.spriteLibrary.GetSprite("Specials", Name());
        }

        public virtual void Cast(Vector2 worldPosition)
        {
            if (Player.Instance.orbs < 1) return;
            Player.Instance.UseOrb();
            OnCast(worldPosition);
        }

        public virtual void OnCast(Vector2 worldPosition)
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