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
            var bombRigidBody =
                Utils.SpawnProjectile(PrefabLibrary.Instance.bomb, Player.Instance.gameObject, worldPosition);
            Vector2 playerPosition = Player.Instance.transform.position;
            var directionVector = (worldPosition - playerPosition).normalized;
            bombRigidBody.AddForce(directionVector * 15f, ForceMode2D.Impulse);
            bombRigidBody.AddTorque(4f, ForceMode2D.Impulse);
        }
    }
}