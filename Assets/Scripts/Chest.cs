using GamGUI;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");
    private static readonly int Looted = Animator.StringToHash("Looted");
    public SpriteRenderer rewardSpriteRenderer;
    public AudioClip openClip;
    public AudioClip lootClip;
    private Animator _animator;
    private AudioSource _audioSource;
    private IChestReward _contents;
    private bool _looted;
    private bool _opened;
    private GameObject _player;
    private InteractionPrompt _prompt;

    private void Start()
    {
        _prompt = FindObjectOfType<InteractionPrompt>(true);
        _player = FindObjectOfType<Player>().gameObject;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Vector2.Distance(_player.transform.position, gameObject.transform.position) <= 2f)
        {
            _prompt.activeObject = gameObject;
            DisplayPrompt();
        }
        else if (_prompt.activeObject == gameObject)
        {
            _prompt.Hide();
        }
    }

    public void Interact()
    {
        if (!_opened) OpenChest();
        else if (!_looted) LootChest();
    }

    private void OpenChest()
    {
        _contents = Utils.InstantiateRandomChestReward();
        var rewardSprite = _contents.GetSprite();
        rewardSpriteRenderer.sprite = rewardSprite;
        _opened = true;
        _animator.SetTrigger(Open);
        _audioSource.PlayOneShot(openClip);
    }

    private void LootChest()
    {
        _looted = true;
        _animator.SetTrigger(Looted);
        _contents.Acquire();
        _audioSource.PlayOneShot(lootClip);
    }

    private void DisplayPrompt()
    {
        if (!_opened)
            _prompt.Display("Open Chest");
        else if (!_looted)
            _prompt.Display("Take", _contents.Name());
        else
            _prompt.Hide();
    }
}