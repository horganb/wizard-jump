using System.Linq;
using Enemies;
using Level;
using Singletons;
using UnityEngine;

namespace Levels
{
    public class SlimeKingLevel : BossLevel
    {
        private static readonly int FightOver = Animator.StringToHash("FightOver");
        public KingSlime slimeKing;

        public AudioSource introSource;
        public AudioSource loopSource;
        public Platform kingStartPlatform;

        private Platform[] _allPlatforms;
        private Animator _animator;

        private bool _spawnedBoss;

        private void Start()
        {
            _allPlatforms = GetComponentsInChildren<Platform>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_spawnedBoss && _allPlatforms.Contains(Player.Instance.lastStandingPlatform))
            {
                _spawnedBoss = true;
                slimeKing.gameObject.SetActive(true);
                LevelManager.Instance.currentBoss = slimeKing;
                introSource.Play();
                loopSource.PlayDelayed(introSource.clip.length);
            }

            if (_spawnedBoss && LevelManager.Instance.currentBoss.isDead)
            {
                _animator.SetTrigger(FightOver);
                OnBeaten();
            }
        }
    }
}