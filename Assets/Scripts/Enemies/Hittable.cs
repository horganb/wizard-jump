using UnityEngine;

namespace Enemies
{
    public abstract class Hittable : MonoBehaviour
    {
        public abstract void OnHit(Vector2 impactVector, float damage);

        public virtual void SetOnFire()
        {
        }

        public virtual void Freeze()
        {
        }
    }
}