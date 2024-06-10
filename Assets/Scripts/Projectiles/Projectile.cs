using System;
using Interactable;
using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile : MonoBehaviour, IChestReward
    {
        public float speed = 2f;
        public AudioClip shootClip;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var fireballViewportPoint = _mainCamera.WorldToViewportPoint(gameObject.transform.position);
            if (fireballViewportPoint.x > 1.2 || Math.Abs(fireballViewportPoint.y) > 1.2) Destroy(gameObject);
            var o = gameObject;
            var pos = o.transform.position;
            pos += o.transform.right * (Time.deltaTime * speed);
            o.transform.position = pos;
        }

        public abstract string Name();

        public void Acquire()
        {
            Player.Instance.activeAttack = this;
        }

        public Sprite GetSprite()
        {
            return GetComponent<SpriteRenderer>().sprite;
        }
    }
}