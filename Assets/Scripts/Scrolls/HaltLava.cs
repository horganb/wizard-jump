using UnityEngine;

namespace Scrolls
{
    public class HaltLava : Scroll
    {
        protected override string ScrollName()
        {
            return "Halt Lava";
        }

        public override void Cast(Vector2 worldPosition)
        {
            Player.Scroll = null;
            var lava = Object.FindObjectOfType<Lava>();
            lava.StartCoroutine(lava.HaltLavaFor(5f));
        }
    }
}