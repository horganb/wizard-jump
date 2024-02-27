using UnityEngine;

namespace Enemies
{
    public class Wasp : Enemy
    {
        public float speed = 200f;
        public float slowdownSpeed = 100f;
        public float playerDistance = 2f;
        public float timeToAttack = 1f;

        private State _state;

        private float _stateTimer;


        // Update is called once per frame
        protected override void AliveUpdate()
        {
            var vectorToPlayer = Player.transform.position - gameObject.transform.position;
            if (WithinTriggerDistanceOfPlayer()) SpriteRenderer.flipX = vectorToPlayer.x > 0;
            var unitVectorToPlayer = vectorToPlayer.normalized;

            // determine state
            if (_state == State.Default && vectorToPlayer.magnitude <= playerDistance)
            {
                _state = State.PreparingAttack;
                _stateTimer = timeToAttack;
            }

            if (_state == State.PreparingAttack)
            {
                _stateTimer -= Time.deltaTime;
                if (_stateTimer <= 0f)
                {
                    _state = State.Attacking;
                    _stateTimer = timeToAttack;
                }
            }

            // movement
            if (_state == State.Attacking)
            {
                _stateTimer -= Time.deltaTime;
                if (_stateTimer <= 0f) _state = State.Default;
                else RigidBody.AddForce(unitVectorToPlayer * (Time.deltaTime * speed));
            }
            else
            {
                if (vectorToPlayer.magnitude > playerDistance && WithinTriggerDistanceOfPlayer())
                    RigidBody.AddForce(unitVectorToPlayer * (Time.deltaTime * speed));
                else
                    RigidBody.AddForce(-RigidBody.velocity * (Time.deltaTime * slowdownSpeed));
            }
        }

        protected override void OnDie(Vector2 impactVector)
        {
            RigidBody.gravityScale = 1f;
            RigidBody.freezeRotation = false;
            RigidBody.AddTorque(-3f, ForceMode2D.Impulse);
            AudioSource.Stop();
            base.OnDie(impactVector);
        }

        // private bool _preparingAttack;

        private enum State
        {
            Default,
            PreparingAttack,
            Attacking
        }
    }
}