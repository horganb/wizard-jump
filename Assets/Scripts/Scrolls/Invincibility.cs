using UnityEngine;

namespace Scrolls
{
    public class Invincibility : Scroll
    {
        protected override string ScrollName()
        {
            return "Invincibility";
        }

        public override void Cast(Vector2 worldPosition)
        {
            Player.Scroll = null;
            Player.StartCoroutine(Player.SetInvincibleFor(5f));
        }
    }
}