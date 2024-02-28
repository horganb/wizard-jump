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
            Player.Instance.Scroll = null;
            Player.Instance.StartCoroutine(Player.Instance.SetInvincibleFor(5f));
        }
    }
}