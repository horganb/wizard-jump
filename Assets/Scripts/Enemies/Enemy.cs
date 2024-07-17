using System;
using System.Collections;
using Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy : Hittable
    {
        public float verticalTriggerDistance = 6f;
        public float horizontalTriggerDistance = 10f;
        public AudioClip hitClip;
        public AudioClip deathClip;
        public float health;
        public GameObject onFireEffect;
        public GameObject frozenEffect;
        public bool isDead;
        private bool _frozen;
        private bool _onFire;
        protected Animator Animator;
        protected AudioSource AudioSource;
        protected Collider2D Collider;
        protected float Damage = 0.5f;
        protected Rigidbody2D RigidBody;
        protected SpriteRenderer SpriteRenderer;
        public virtual float MaxHealth => 1f;


        protected virtual void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            AudioSource = GetComponent<AudioSource>();
            Collider = GetComponent<Collider2D>();
            Animator = GetComponent<Animator>();
            health = MaxHealth;
        }

        protected virtual void Update()
        {
            if (isDead)
                Utils.DestroyIfOffscreen(gameObject);
            else if (!_frozen) AliveUpdate();
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            var player = col.gameObject.GetComponent<Player>();
            if (player != null && !_frozen) player.OnHit(Damage, gameObject);

            base.OnCollisionEnter2D(col);
        }

        public override void OnHit(Vector2 impactVector, float damage, GameObject projectile = null)
        {
            if (projectile != null)
                Destroy(projectile);
            health -= damage;
            StopCoroutine(HitColorEffect());
            StartCoroutine(HitColorEffect());
            if (health > 0f)
            {
                if (hitClip is not null) AudioSource.PlayOneShot(hitClip);
                OnNonLethalHit(impactVector);
            }
            else
            {
                if (deathClip is not null) AudioSource.PlayOneShot(deathClip);
                OnDie(impactVector);
            }
        }

        public override void SetOnFire()
        {
            _onFire = true;
            onFireEffect.SetActive(true);
            StopCoroutine(TakePeriodicFireDamage());
            StartCoroutine(TakePeriodicFireDamage());
        }

        private IEnumerator HitColorEffect()
        {
            SpriteRenderer.color = new Color(1f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.1f);
            SpriteRenderer.color = Color.white;
        }

        private IEnumerator TakePeriodicFireDamage()
        {
            while (_onFire)
            {
                yield return new WaitForSeconds(2f);
                OnHit(Vector2.zero, 0.5f);
            }
        }

        public override void Freeze()
        {
            _frozen = true;
            frozenEffect.SetActive(true);
            if (Animator is not null) Animator.speed = 0f;
            StopCoroutine(UnfreezeAfterDelay());
            StartCoroutine(UnfreezeAfterDelay());
        }

        private IEnumerator UnfreezeAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            _frozen = false;
            frozenEffect.SetActive(false);
            Animator.speed = 1f;
        }

        protected virtual void AliveUpdate()
        {
        }

        protected virtual void OnNonLethalHit(Vector2 impactVector)
        {
            RigidBody.AddForce(impactVector * 5f, ForceMode2D.Impulse);
        }

        public virtual void OnDie(Vector2 impactVector, bool endOfLevel = false)
        {
            isDead = true;
            var color = SpriteRenderer.color;
            color.a = 0.5f;
            SpriteRenderer.color = color;
            RigidBody.AddForce(impactVector * 3f, ForceMode2D.Impulse);
            Collider.enabled = false;
            if (Random.value <= 0.2f)
                Instantiate(PrefabLibrary.Instance.orbDrop, transform.position, Quaternion.identity);
        }

        protected bool WithinTriggerDistanceOfPlayer()
        {
            var vectorToPlayer = Player.Instance.transform.position - gameObject.transform.position;
            return Math.Abs(vectorToPlayer.x) < horizontalTriggerDistance &&
                   Math.Abs(vectorToPlayer.y) < verticalTriggerDistance;
        }
    }
}