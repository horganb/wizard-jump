using UnityEngine;

namespace Enemies
{
    public class BigSlime : Slime
    {
        public GameObject slimePrefab;
        public override float MaxHealth => 3f;

        public override void OnDie(Vector2 impactVector, bool endOfLevel = false)
        {
            base.OnDie(impactVector, endOfLevel);
            if (endOfLevel) return;
            var position = transform.position;
            var child1 = Instantiate(slimePrefab, position, Quaternion.identity);
            var child2 = Instantiate(slimePrefab, position, Quaternion.identity);
            child1.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f + Vector2.left * 3f, ForceMode2D.Impulse);
            child2.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5f + Vector2.right * 3f, ForceMode2D.Impulse);
        }
    }
}