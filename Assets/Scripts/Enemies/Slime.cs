using System;
using UnityEngine;

namespace Enemies
{
    public class Slime : Enemy
    {
        public float jumpHeight = 2f;
        public float knockbackHeight = 20f;
        public float jumpDistance = 0.5f;
        public float minJumpDistance = 4f;
        public float maxJumpDistance = 6f;
        public float jumpInterval = 1f;
        public AudioClip[] jumps;

        private float _jumpTimer;

        protected override void AliveUpdate()
        {
            var vectorToPlayer = Player.Instance.transform.position - gameObject.transform.position;
            if (WithinTriggerDistanceOfPlayer())
            {
                _jumpTimer += Time.deltaTime;
                if (_jumpTimer > jumpInterval)
                {
                    _jumpTimer = 0f;
                    var jumpX = vectorToPlayer.x * jumpDistance;
                    if (jumpX < 0)
                        jumpX = Math.Clamp(jumpX, -maxJumpDistance, -minJumpDistance);
                    else
                        jumpX = Math.Clamp(jumpX, minJumpDistance, maxJumpDistance);
                    var jumpVector = new Vector2(jumpX, jumpHeight);
                    RigidBody.AddForce(jumpVector, ForceMode2D.Impulse);
                    AudioSource.PlayOneShot(Utils.RandomFromArray(jumps));
                }
            }
            else
            {
                _jumpTimer = 0f;
            }
        }

        protected override void OnNonLethalHit(Vector2 impactVector)
        {
            RigidBody.velocity = Vector2.zero;
            var knockbackX = impactVector.x >= 0.3f ? 1f : impactVector.x <= -0.3f ? -1f : 0f;
            var knockbackVector = new Vector2(knockbackX * 3f, knockbackHeight);
            RigidBody.AddForce(knockbackVector, ForceMode2D.Impulse);
            _jumpTimer = 0f;
        }
    }
}