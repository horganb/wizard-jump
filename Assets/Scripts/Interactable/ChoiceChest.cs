﻿using GamGUI;
using Projectiles;
using Singletons;
using UnityEngine;

namespace Interactable
{
    public class ChoiceChest : MonoBehaviour, IInteractable
    {
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Looted = Animator.StringToHash("Looted");
        public SpriteRenderer reward1SpriteRenderer;
        public SpriteRenderer reward2SpriteRenderer;
        public AudioClip openClip;
        public AudioClip lootClip;
        private Animator _animator;
        private AudioSource _audioSource;
        private IChestReward _contents1;
        private IChestReward _contents2;
        private bool _looted;
        private bool _opened;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Player.Instance.lastStandingPlatform && Player.Instance.lastStandingPlatform.isReward &&
                Vector2.Distance(Player.Instance.transform.position, gameObject.transform.position) <= 2f)
            {
                DisplayPrompt();
            }
            else
            {
                if (ReferenceEquals(InteractionPrompt.Instance.ActiveObject, this)) InteractionPrompt.Instance.Hide();
                if (ReferenceEquals(ChoiceInteractionPrompt.Instance.ActiveObject, this))
                    ChoiceInteractionPrompt.Instance.Hide();
            }
        }

        public void Interact(bool alternate)
        {
            if (!_opened) OpenChest();
            else if (!_looted) LootChest(alternate);
        }

        private void OpenChest()
        {
            _contents1 = PrefabLibrary.Instance.iceSpike.GetComponent<IceSpike>();
            _contents2 = PrefabLibrary.Instance.fireball.GetComponent<Fireball>();
            reward1SpriteRenderer.sprite = _contents1.GetSprite();
            reward2SpriteRenderer.sprite = _contents2.GetSprite();
            _opened = true;
            _animator.SetTrigger(Open);
            _audioSource.PlayOneShot(openClip);
            LevelManager.Instance.StartNextLevel();
        }

        private void LootChest(bool alternate)
        {
            _looted = true;
            _animator.SetTrigger(Looted);
            if (alternate)
                _contents1.Acquire();
            else
                _contents2.Acquire();
            _audioSource.PlayOneShot(lootClip);
        }

        private void DisplayPrompt()
        {
            if (!_opened)
            {
                InteractionPrompt.Instance.ActiveObject = this;
                InteractionPrompt.Instance.Display("Open Chest");
            }
            else if (!_looted)
            {
                InteractionPrompt.Instance.Hide();
                ChoiceInteractionPrompt.Instance.ActiveObject = this;
                ChoiceInteractionPrompt.Instance.Display("Take", _contents1.Name(), "Take", _contents2.Name());
            }
            else
            {
                ChoiceInteractionPrompt.Instance.Hide();
            }
        }
    }
}