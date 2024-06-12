using Singletons;
using UnityEngine;

namespace Special
{
    public class Bomb : Special
    {
        public override string Name()
        {
            return "Bomb";
        }

        public override void OnCast(Vector2 worldPosition)
        {
            Vector2 playerPosition = Player.Instance.transform.position;
            var directionVector = (worldPosition - playerPosition).normalized;
            var startPosition = playerPosition + directionVector * 0.5f;
            var bomb = Object.Instantiate(PrefabLibrary.Instance.bomb, startPosition, Quaternion.identity);
            var bombRigidBody = bomb.GetComponent<Rigidbody2D>();
            bombRigidBody.AddForce(directionVector * 15f, ForceMode2D.Impulse);
            bombRigidBody.AddTorque(4f, ForceMode2D.Impulse);
        }
    }
}