using System;
using Level;
using UnityEngine;

namespace Enemies
{
    public class SkellyDog : Enemy
    {
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int Running = Animator.StringToHash("Running");
        public float maxSpeed = 5f;
        public float speed = 10000f;
        public float attackInterval = 3f;
        public float distanceToRunTo = 1.5f;
        public float distanceToStartFollowing = 3f;
        public Platform _targetPlatform;
        private float _attackCooldown;
        private Platform _lastPlatform;
        private bool _runningTowardsPlayer;

        public override float MaxHealth => 3f;


        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
            var platform = col.gameObject.GetComponent<Platform>();
            if (platform != null) _lastPlatform = platform;
            if (!isDead) EndPlatformPassThrough();
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
            if (!isDead) EndPlatformPassThrough();
        }

        protected override void AliveUpdate()
        {
            if (_attackCooldown > 0f) _attackCooldown -= Time.deltaTime;
            var vectorToPlayer = Player.Instance.transform.position - transform.position;
            if (IsGrounded())
                if (WithinTriggerDistanceOfPlayer())
                {
                    // if on the player's platform, run towards them and attack
                    if (_lastPlatform is not null && _lastPlatform == Player.Instance.lastStandingPlatform)
                    {
                        if (vectorToPlayer.magnitude < distanceToStartFollowing && _attackCooldown <= 0f)
                        {
                            var playerPos = Player.Instance.transform.position;
                            Utils.FlipXToFace(transform, playerPos);
                            Utils.ShootAt(RigidBody, playerPos, 5f);
                            _attackCooldown = attackInterval;
                        }
                        else
                        {
                            if (vectorToPlayer.magnitude >= distanceToStartFollowing)
                                _runningTowardsPlayer = true;
                            else if (vectorToPlayer.magnitude <= distanceToRunTo) _runningTowardsPlayer = false;
                            if (AtEdgeBasedOnVelocity())
                                _runningTowardsPlayer = false;

                            if (_runningTowardsPlayer)
                            {
                                RunTowards(Player.Instance.gameObject);
                            }
                            else
                            {
                                Animator.SetBool(Running, false);
                                RigidBody.velocity = Vector2.zero;
                            }
                        }
                    }
                    // if not on player's platform,
                    // 1. find a platform that is closer to them than current platform
                    // 2. run toward it
                    // 3. jump when within a certain x-distance from it
                    else
                    {
                        CalculateTargetPlatform();
                        if (_targetPlatform)
                        {
                            RunTowards(_targetPlatform.gameObject);
                            var vectorToTarget = _targetPlatform.transform.position - transform.position;
                            if (Math.Abs(vectorToTarget.x) < 3f || AtEdgeOfPlatform())
                            {
                                PlatformPassThrough();
                                RigidBody.velocity = Vector2.zero;
                                Utils.FlipXToFace(transform, GetTargetPlatformLandingLocation());
                                Utils.ShootAt(RigidBody, GetTargetPlatformLandingLocation(), 8f, 1f);
                            }
                        }
                    }
                }
                else
                {
                    Animator.SetBool(Running, false);
                    RigidBody.velocity = Vector2.zero;
                }

            if (_targetPlatform is not null && RigidBody.velocity.y < 0f &&
                Math.Abs(_targetPlatform.transform.position.y - transform.position.y) < 1.5f && IsOverTargetPlatform())
                EndPlatformPassThrough();

            Animator.SetBool(Jumping, !IsGrounded());
        }

        private void RunTowards(GameObject obj)
        {
            var position = obj.transform.position;
            Utils.FlipXToFace(transform, position);

            var targetToLeft = position.x < transform.position.x;
            Animator.SetBool(Running, true);
            var horizontalForce = Time.deltaTime * speed;
            if (targetToLeft)
                horizontalForce *= -1;
            RigidBody.AddForce(Vector2.right * horizontalForce);

            var clampedHVelocity = Math.Clamp(RigidBody.velocity.x, -maxSpeed, maxSpeed);
            RigidBody.velocity = new Vector2(clampedHVelocity, RigidBody.velocity.y);
        }

        public void PlatformPassThrough()
        {
            Collider.excludeLayers = LayerMask.GetMask("Platform");
        }

        private void EndPlatformPassThrough()
        {
            _targetPlatform = null;
            Collider.excludeLayers = new LayerMask();
        }

        private bool AtEdgeOfPlatform()
        {
            if (_targetPlatform is null || _lastPlatform is null) return false;
            var vectorToTarget = _targetPlatform.transform.position - transform.position;
            if (vectorToTarget.x < 0f)
                return transform.position.x < _lastPlatform.theCollider.bounds.min.x + 1f;
            return transform.position.x > _lastPlatform.theCollider.bounds.max.x - 1f;
        }

        private bool AtEdgeBasedOnVelocity()
        {
            if (_lastPlatform is null) return false;
            if (RigidBody.velocity.x < 0f)
                return transform.position.x < _lastPlatform.theCollider.bounds.min.x + 1f;
            return transform.position.x > _lastPlatform.theCollider.bounds.max.x - 1f;
        }

        private bool IsOverTargetPlatform()
        {
            if (_targetPlatform is null) return false;
            var position = transform.position;
            var bounds = _targetPlatform.theCollider.bounds;
            return position.x >= bounds.min.x && position.x <= bounds.max.x;
        }

        private void CalculateTargetPlatform()
        {
            _targetPlatform = Player.Instance.lastStandingPlatform;
            if (!_targetPlatform) return;
            foreach (var platform in FindObjectsOfType<Platform>())
                if (platform.isEnabled && _lastPlatform is not null && platform != _lastPlatform &&
                    Utils.IsBetween(
                        platform.gameObject,
                        _lastPlatform.gameObject, _targetPlatform.gameObject))
                    _targetPlatform = platform;
        }

        private Vector2 GetTargetPlatformLandingLocation()
        {
            var landingPosition = _targetPlatform.transform.position + Vector3.up * 2f;
            var bounds = _targetPlatform.theCollider.bounds;
            landingPosition.x = Math.Clamp(transform.position.x, bounds.min.x + 0.5f, bounds.max.x - 0.5f);
            return landingPosition;
        }

        protected override void OnNonLethalHit(Vector2 impactVector)
        {
            RigidBody.velocity = Vector2.zero;
            var knockbackX = impactVector.x >= 0.3f ? 1f : impactVector.x <= -0.3f ? -1f : 0f;
            var knockbackVector = new Vector2(knockbackX * 3f, 5f);
            RigidBody.AddForce(knockbackVector, ForceMode2D.Impulse);
        }
    }
}