using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Skeleton : Enemy
    {
        private static readonly int Throw = Animator.StringToHash("Throw");
        private static readonly int EndThrow = Animator.StringToHash("EndThrow");
        public GameObject bone;
        public AudioClip throwClip;
        public AudioClip shieldClip;
        public float knockbackHeight = 20f;
        private float _stateTimer;
        private bool _throwing;
        public override float MaxHealth => 3f;

        protected override void AliveUpdate()
        {
            if (!WithinTriggerDistanceOfPlayer()) return;

            Utils.FlipXToFace(transform, Player.Instance.transform.position);

            if (_throwing) return;

            _stateTimer += Time.deltaTime;
            if (_stateTimer > 3f)
            {
                _stateTimer = 0f;
                _throwing = true;
                Animator.SetTrigger(Throw);
            }
        }

        public void OnThrow()
        {
            var thrownBone = Instantiate(bone, transform, false);
            var boneRigidBody = thrownBone.GetComponent<Rigidbody2D>();
            thrownBone.transform.SetParent(null);
            var target = Player.Instance.transform.position + Vector3.right * Random.Range(-2f, 2f);
            Utils.ShootAt(boneRigidBody, target, 10f);
            boneRigidBody.AddTorque(200f);
            AudioSource.PlayOneShot(throwClip);
            thrownBone.AddComponent<Bone>();
        }

        public void OnThrowsComplete()
        {
            Animator.SetTrigger(EndThrow);
            _throwing = false;
        }

        public override void OnHit(Vector2 impactVector, float damage, GameObject projectile = null)
        {
            if (_throwing || projectile is null)
            {
                base.OnHit(impactVector, damage, projectile);
            }
            else
            {
                projectile.transform.Rotate(Vector3.forward, 180f);
                var projectileComponent = projectile.GetComponent<Projectile>();
                projectileComponent.SetCanDamagePlayer();
                projectileComponent.direction = projectile.transform.right;
                AudioSource.PlayOneShot(shieldClip);
            }
        }

        protected override void OnNonLethalHit(Vector2 impactVector)
        {
            var knockbackX = impactVector.x >= 0.3f ? 1f : impactVector.x <= -0.3f ? -1f : 0f;
            var knockbackVector = new Vector2(knockbackX * 2f, knockbackHeight);
            RigidBody.AddForce(knockbackVector, ForceMode2D.Impulse);
        }

        public override void SetOnFire()
        {
            if (_throwing) base.SetOnFire();
        }

        public override void Freeze()
        {
            if (_throwing) base.Freeze();
        }
    }
}