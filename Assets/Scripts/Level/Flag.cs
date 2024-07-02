using Singletons;
using UnityEngine;

namespace Level
{
    public class Flag : Interactable.Interactable
    {
        private static readonly int RaiseFlag = Animator.StringToHash("RaiseFlag");
        public AudioClip flappingClip;
        private Animator _animator;
        private AudioSource _audioSource;
        private bool _raised;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        public override void Interact(bool alternate)
        {
            _raised = true;
            _animator.SetTrigger(RaiseFlag);
            _audioSource.PlayOneShot(flappingClip);
            LevelManager.Instance.StartNextLevel();
        }

        protected override string PromptText()
        {
            return "Continue";
        }

        protected override bool CanInteract()
        {
            return !_raised && Player.Instance.lastStandingPlatform && Player.Instance.lastStandingPlatform.isReward;
        }
    }
}