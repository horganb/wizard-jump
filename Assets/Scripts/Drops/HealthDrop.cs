﻿using UnityEngine;

namespace Drops
{
    public class HealthDrop : MonoBehaviour
    {
        private void Update()
        {
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) <= 1f &&
                Player.Instance.health < Player.Instance.maxHealth)
            {
                Player.Instance.OnGainHealth(0.5f);
                Destroy(gameObject);
            }
        }
    }
}