using GamGUI;
using Interactable;
using Singletons;
using UnityEngine;

namespace Level
{
    public class Flag : MonoBehaviour, IInteractable
    {
        private static readonly int RaiseFlag = Animator.StringToHash("RaiseFlag");
        public AudioClip flappingClip;
        private Animator _animator;
        private AudioSource _audioSource;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Player.Instance.lastStandingPlatform && Player.Instance.lastStandingPlatform.isReward &&
                Vector2.Distance(Player.Instance.transform.position, gameObject.transform.position) <= 1f)
            {
                ChoiceInteractionPrompt.Instance.ActiveObject = this;
                ChoiceInteractionPrompt.Instance.Display("Continue");
            }
            else
            {
                if (ReferenceEquals(ChoiceInteractionPrompt.Instance.ActiveObject, this))
                    ChoiceInteractionPrompt.Instance.Hide();
            }
        }

        public void Interact(bool alternate)
        {
            _animator.SetTrigger(RaiseFlag);
            _audioSource.PlayOneShot(flappingClip);
            LevelManager.Instance.StartNextLevel();
        }
    }
}