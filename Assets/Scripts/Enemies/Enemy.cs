using System;
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
            if (!IsDead) AliveUpdate();
        }

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            var player = col.gameObject.GetComponent<Player>();
            if (player != null && !player.HasInvincibility()) OnHitPlayer(player);

            if (col.gameObject.GetComponent<Fireball>() != null)
            {
                Destroy(col.gameObject);
                var impactVector = gameObject.transform.position - col.gameObject.transform.position;
                OnHit(impactVector);
            }

            var lava = col.gameObject.GetComponent<Lava>();
            if (lava != null && SpriteRenderer.enabled)
            {
                AudioSource.PlayClipAtPoint(lava.lavaDeathClip, transform.position);
                Destroy(gameObject);
            }
        }

        protected virtual void OnHitPlayer(Player player)
        {
            var impactVector = player.gameObject.transform.position - gameObject.transform.position;
            var knockbackVector = new Vector2(impactVector.x * 10f, 5f);
            player.GetComponent<Rigidbody2D>().AddForce(knockbackVector, ForceMode2D.Impulse);
            player.LoseHealth(Damage);
            player.audioSource.PlayOneShot(player.hitClip);
        }

        protected virtual void AliveUpdate()
        {
        }

        private void OnHit(Vector2 impactVector)
        {
            health -= 1f;
            if (health > 0f)
                OnNonLethalHit(impactVector);
            else
                OnDie(impactVector);
            AudioSource.PlayOneShot(deathClip);
        }

        protected virtual void OnNonLethalHit(Vector2 impactVector)
        {
            RigidBody.AddForce(impactVector * 8f, ForceMode2D.Impulse);
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
            if (Random.value <= 0.1f)
                Instantiate(PrefabLibrary.Instance.healthDrop, transform.position, Quaternion.identity);
            else if (Random.value <= 0.2f)
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