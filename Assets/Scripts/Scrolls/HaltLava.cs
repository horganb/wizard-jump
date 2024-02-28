using Singletons;
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
            Player.Instance.Scroll = null;
            Lava.Instance.StartCoroutine(Lava.Instance.HaltLavaFor(5f));
        }
    }
}