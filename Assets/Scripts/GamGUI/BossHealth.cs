using Singletons;
using TMPro;
using UnityEngine;

namespace GamGUI
{
    public class BossHealth : MonoBehaviour
    {
        public TMP_Text text;
        public Transform health;
        public GameObject container;

        private void Update()
        {
            var currentBoss = LevelManager.Instance.currentBoss;
            if (currentBoss is null || currentBoss.isDead)
            {
                container.SetActive(false);
            }
            else
            {
                container.SetActive(true);
                var scale = health.localScale;
                scale.x = currentBoss.health / currentBoss.MaxHealth;
                health.localScale = scale;
                text.text = currentBoss.name;
            }
        }
    }
}