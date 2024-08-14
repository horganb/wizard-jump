using System;
using System.Linq;
using Level;
using Levels;
using Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class KingSlime : Enemy
    {
        private static readonly int PreJump = Animator.StringToHash("Pre Jump");
        private static readonly int PostJump = Animator.StringToHash("Post Jump");
        public float burstInterval = 2f;
        public float shootInterval = 0.5f;
        public float burstSize = 3f;
        public float shootSpeed = 5f;
        public float jumpInterval = 2f;
        public float shedInterval = 4f;
        public float jumpHeight = 10f;
        public float dampeningDistance = 2f;
        public float horizontalJumpSpeed = 2f;
        public Transform shootSource;
        public GameObject slimeBlobPrefab;
        public GameObject slimePrefab;
        public GameObject bigSlimePrefab;
        public AudioClip throwSlimeClip;
        private float _burstTimer;
        private bool _evolved;
        private float _shedTimer;
        private bool _shooting;
        private float _shootTimer;
        private float _shotsFiredInBurst;
        private Vector2 _startPos;
        private State _state;
        private float _stateTimer;
        private Platform _target;

        public override float MaxHealth => 100f;

        protected override void Start()
        {
            base.Start();
            _target = FindObjectOfType<SlimeKingLevel>().kingStartPlatform;
            _startPos = transform.position;
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            var lava = col.gameObject.GetComponent<Lava>();
            if (lava != null && !isDead)
            {
                AudioSource.PlayClipAtPoint(lava.lavaDeathClip, transform.position);
                transform.position = _startPos;
                _target = Utils.RandomFromArray(Platform.GetAllEnabledPlatforms());
                health -= 5f;
            }
        }

        protected override void AliveUpdate()
        {
            if (!_evolved && health <= MaxHealth / 2f)
            {
                _evolved = true;
                shedInterval /= 2;
                jumpInterval /= 2;
            }

            var position = transform.position;
            _shedTimer += Time.deltaTime;
            if (_shedTimer >= shedInterval)
            {
                _shedTimer = 0f;
                var prefabToSpawn = Random.value > 0.8f ? bigSlimePrefab : slimePrefab;
                var slimeObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
                Utils.ShootAt(slimeObject.GetComponent<Rigidbody2D>(),
                    ChooseOtherPlatform().transform.position + Vector3.up, 8f);
            }

            _stateTimer += Time.deltaTime;
            if (_state == State.OnPlatform)
            {
                if (_stateTimer > jumpInterval)
                {
                    _stateTimer = 0f;
                    _shootTimer = 0f;
                    _burstTimer = 0f;
                    _shooting = false;
                    _state = State.PreparingJump;
                    Animator.SetTrigger(PreJump);
                }
                else
                {
                    if (_shooting || _evolved)
                    {
                        _shootTimer += Time.deltaTime;
                        if (_shootTimer >= shootInterval)
                        {
                            _shootTimer = 0f;
                            Shoot();
                            if (!_evolved)
                            {
                                _shotsFiredInBurst++;
                                if (_shotsFiredInBurst > burstSize) _shooting = false;
                            }
                        }
                    }
                    else
                    {
                        _burstTimer += Time.deltaTime;
                        if (_burstTimer >= burstInterval)
                        {
                            _burstTimer = 0f;
                            _shotsFiredInBurst = 0f;
                            _shooting = true;
                        }
                    }
                }
            }
            else if (_state == State.PreparingJump)
            {
                if (_stateTimer >= 1f) BeginJump();
            }
            else if (_state == State.Jumping)
            {
                var yDiff = _target.transform.position.y - transform.position.y;
                var justAboveTarget = RigidBody.velocity.y < 0f && yDiff <= -1f;
                gameObject.layer = justAboveTarget
                    ? LayerMask.NameToLayer("TransparentFX")
                    : LayerMask.NameToLayer("Default");
                if (_stateTimer >= 0.2f && IsGrounded())
                    EndJump();
                else
                    MidJump();
            }
        }

        private void Shoot()
        {
            AudioSource.PlayOneShot(throwSlimeClip);
            var playerPosition = Player.Instance.transform.position;
            var blobRigidBody = Utils.SpawnProjectile(slimeBlobPrefab, shootSource.gameObject,
                playerPosition);
            Utils.ShootAt(blobRigidBody, playerPosition, shootSpeed);
            blobRigidBody.AddTorque(8f, ForceMode2D.Impulse);
        }

        private void BeginJump()
        {
            _stateTimer = 0f;
            _state = State.Jumping;
            _target = ChooseOtherPlatform();
            var verticalDistance = _target.transform.position.y - transform.position.y;
            var targetAdjustment = Vector2.up * (Math.Clamp(verticalDistance, 0f, 5f) * 3f);
            RigidBody.AddForce(Vector2.up * jumpHeight + targetAdjustment, ForceMode2D.Impulse);
        }

        private void MidJump()
        {
            var xDiff = _target.transform.position.x - transform.position.x;
            var velocity = RigidBody.velocity;
            if (xDiff < -0.1f)
                velocity.x = -horizontalJumpSpeed;
            else if (xDiff > 0.1f) velocity.x = horizontalJumpSpeed;
            else velocity.x = 0f;
            var absDiff = Math.Abs(xDiff);
            if (absDiff <= dampeningDistance)
                velocity.x *= 2f * absDiff / (dampeningDistance + absDiff);
            RigidBody.velocity = velocity;
        }

        private void EndJump()
        {
            RigidBody.velocity = Vector2.zero;
            _stateTimer = 0f;
            _state = State.OnPlatform;
            Animator.SetTrigger(PostJump);
        }

        private Platform ChooseOtherPlatform()
        {
            var platformChoices = Platform.GetAllEnabledPlatforms().Where(p => p != _target).ToArray();
            return Utils.RandomFromArray(platformChoices);
        }

        protected override void OnNonLethalHit(Vector2 impactVector)
        {
            // no knockback
        }

        public override void Freeze()
        {
            // no effect
        }

        public override void SetOnFire()
        {
            // no effect
        }

        private enum State
        {
            OnPlatform,
            PreparingJump,
            Jumping
        }
    }
}