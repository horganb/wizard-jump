using UnityEngine;

namespace Enemies
{
    public class SkellyBat : Enemy
    {
        public float lookSpeed = 5f;
        public float speed = 5f;
        public override float MaxHealth => 2f;

        protected override void AliveUpdate()
        {
            var playerPosition = Player.Instance.transform.position;

            // flip to face player
            var transform1 = transform;
            var targetToLeft = playerPosition.x < transform1.position.x;
            var scale = transform1.localScale;
            scale.y = targetToLeft ? 1f : -1f;
            transform1.localScale = scale;

            // source: https://discussions.unity.com/t/look-rotation-2d-equivalent/728105/2
            var vectorToTarget = playerPosition - transform.position;
            var rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
            rotatedVectorToTarget *= -1;
            var targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            RigidBody.SetRotation(Quaternion.Lerp(transform.rotation, targetRotation,
                Time.deltaTime *
                lookSpeed));
            RigidBody.AddForce(vectorToTarget.normalized * speed);
        }


        public override void OnDie(Vector2 impactVector, bool endOfLevel = false)
        {
            RigidBody.gravityScale = 1f;
            RigidBody.AddTorque(-3f, ForceMode2D.Impulse);
            AudioSource.Stop();
            base.OnDie(impactVector, endOfLevel);
        }
    }
}