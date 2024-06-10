using System;
using System.Collections;
using Projectiles;
using Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        public float verticalTriggerDistance = 6f;
        public float horizontalTriggerDistance = 10f;
        public AudioClip deathClip;
        public float health;
        public GameObject onFireEffect;
        public GameObject frozenEffect;
        private bool _frozen;
        private bool _onFire;
        protected AudioSource AudioSource;
        protected float Damage = 0.5f;
        protected bool IsDead;
        protected Rigidbody2D RigidBody;
        protected SpriteRenderer SpriteRenderer;
        public virtual float MaxHealth => 1f;


        protected virtual void Start()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            AudioSource = GetComponent<AudioSource>();
            health = MaxHealth;
        }

        protected virtual void Update()
        {
            if (!IsDead && !_frozen) AliveUpdate();
        }

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            var player = col.gameObject.GetComponent<Player>();
            if (player != null && !player.HasInvincibility() && !_frozen) OnHitPlayer(player);

            if (col.gameObject.GetComponent<Projectile>() != null)
            {
                Destroy(col.gameObject);
                var impactVector = gameObject.transform.position - col.gameObject.transform.position;
                OnHit(impactVector, Player.Instance.damage);
                if (col.gameObject.GetComponent<Fireball>() != null) SetOnFire();
                if (col.gameObject.GetComponent<IceSpike>() != null) Freeze();
            }

            var lava = col.gameObject.GetComponent<Lava>();
            if (lava != null && SpriteRenderer.enabled)
            {
                AudioSource.PlayClipAtPoint(lava.lavaDeathClip, transform.position);
                Destroy(gameObject);
            }
        }

        private void SetOnFire()
        {
            _onFire = true;
            onFireEffect.SetActive(true);
            StopCoroutine(TakePeriodicFireDamage());
            StartCoroutine(TakePeriodicFireDamage());
        }

        private IEnumerator TakePeriodicFireDamage()
        {
            while (_onFire)
            {
                yield return new WaitForSeconds(2f);
                OnHit(Vector2.zero, 0.5f);
            }
        }

        private void Freeze()
        {
            _frozen = true;
            frozenEffect.SetActive(true);
            StopCoroutine(UnfreezeAfterDelay());
            StartCoroutine(UnfreezeAfterDelay());
        }

        private IEnumerator UnfreezeAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            _frozen = false;
            frozenEffect.SetActive(false);
        }

        protected virtual void OnHitPlayer(Player player)
        {
            if (Random.value <= player.dodgeChance) return;

            var impactVector = player.gameObject.transform.position - gameObject.transform.position;
            var knockbackVector = new Vector2(impactVector.x * 10f, 5f);
            player.GetComponent<Rigidbody2D>().AddForce(knockbackVector, ForceMode2D.Impulse);
            player.LoseHealth(Damage);
            player.audioSource.PlayOneShot(player.hitClip);
            if (player.thorns > 0f) OnHit(-knockbackVector.normalized, Damage * player.thorns);
        }

        protected virtual void AliveUpdate()
        {
        }

        private void OnHit(Vector2 impactVector, float damage)
        {
            health -= damage;
            if (health > 0f)
                OnNonLethalHit(impactVector);
            else
                OnDie(impactVector);
            AudioSource.PlayOneShot(deathClip);
        }

        protected virtual void OnNonLethalHit(Vector2 impactVector)
        {
            RigidBody.AddForce(impactVector * 5f, ForceMode2D.Impulse);
        }

        public virtual void OnDie(Vector2 impactVector)
        {
            IsDead = true;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            color.a = 0.5f;
            spriteRenderer.color = color;
            RigidBody.AddForce(impactVector * 3f, ForceMode2D.Impulse);
            GetComponent<Collider2D>().enabled = false;
            if (Random.value <= 0.1f + Player.Instance.dropModifier)
                Instantiate(PrefabLibrary.Instance.healthDrop, transform.position, Quaternion.identity);
            else if (Random.value <= 0.2f + Player.Instance.dropModifier && Player.Instance.Special != null)
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