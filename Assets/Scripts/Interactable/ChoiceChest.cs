using System.Linq;
using GamGUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interactable
{
    public class ChoiceChest : Interactable
    {
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Looted = Animator.StringToHash("Looted");
        private static readonly int RaiseSign = Animator.StringToHash("RaiseSign");
        public GameObject sign;
        public TMP_Text cost;
        public Transform rewardParent;
        public Image rewardPrefab;
        public AudioClip openClip;
        public AudioClip lootClip;
        public RewardType.RewardType rewardType;
        private Animator _animator;
        private AudioSource _audioSource;
        private bool _looted;
        private bool _opened;
        public IChestReward[] Contents;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();
            var costNum = rewardType.GetCost();
            cost.text = costNum == 0 ? "Free" : costNum.ToString();
            cost.color = CanBuy() ? new Color(113f, 135f, 255f) : Color.red;
        }

        public override void Interact(bool alternate)
        {
            if (!_opened) OpenChest();
            else if (!_looted) AttemptLootChest(alternate);
        }

        protected override string PromptText()
        {
            return rewardType.PurchasePrompt();
        }

        protected override ChoiceInteractionPrompt.Choice[] PromptChoices()
        {
            if (_opened && !_looted)
                return Contents.Select(content => new ChoiceInteractionPrompt.Choice
                    { ActionText = "Take", DescriptionText = content.Name() }).ToArray();
            return null;
        }

        public void OnReachLevel()
        {
            _animator.SetTrigger(RaiseSign);
        }

        private bool CanBuy()
        {
            return Player.Instance.gold >= rewardType.GetCost();
        }

        protected override bool CanInteract()
        {
            if (!Player.Instance.lastStandingPlatform || !Player.Instance.lastStandingPlatform.isReward) return false;
            if (!_opened && !CanBuy()) return false;
            if (_opened && _looted) return false;
            return true;
        }

        private void OpenChest()
        {
            Player.Instance.gold -= rewardType.GetCost();
            Contents = rewardType.GenerateRewards();
            foreach (var content in Contents)
            {
                var rewardObject = Instantiate(rewardPrefab.transform, rewardParent);
                var image = rewardObject.GetComponent<Image>();
                image.sprite = content.GetSprite();
            }

            _opened = true;
            _animator.SetTrigger(Open);
            _audioSource.PlayOneShot(openClip);
            Destroy(sign);
        }

        private void AttemptLootChest(bool alternate)
        {
            if (alternate)
            {
                if (Contents.Length > 1)
                    LootChest(Contents.First());
            }
            else
            {
                LootChest(Contents.Last());
            }
        }

        private void LootChest(IChestReward reward)
        {
            _looted = true;
            _animator.SetTrigger(Looted);
            reward.Acquire();
            _audioSource.PlayOneShot(lootClip);
        }
    }
}