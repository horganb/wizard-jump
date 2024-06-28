using System;
using System.Collections;
using Attacks;
using Enemies;
using GamGUI;
using Level;
using Scrolls;
using Singletons;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : SingletonMonoBehaviour<Player>
{
    private const float SpeedFractionWhileJumping = 0.5f;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsInvincible = Animator.StringToHash("IsRespawning");

    public float speed = 500f;
    public float maxSpeed = 10f;
    public float jump = 5f;
    public float jumpAddition = 5f;
    public float jumpAdditionDuration = 0.5f;
    public float health = 3;
    public float maxHealth = 3;
    public int orbs;
    public int maxOrbs = 1;
    public float attackSpeed = 0.5f;
    public Platform lastPlatform;
    public Platform lastStandingPlatform;
    public GameObject wandObject;
    public Light2D wandLight;
    public bool invincible;
    public AudioClip jumpClip;
    public AudioSource audioSource;
    public AudioClip hitClip;
    public float lavaDamage = 1f;
    public float damage = 1f;
    public float thorns;
    public float dodgeChance;
    public float dropModifier;
    private Animator _animator;
    private float _attackCooldown;
    private float _hVelocity;
    private bool _isAttackHeld;
    private bool _isDead;
    private bool _isJumpHeld;
    private float _jumpHeldDuration;
    private JumpStage _jumpStage = JumpStage.NoJump;
    private Vector2 _looking;
    private Camera _mainCamera;
    private bool _prevGrounded;
    private float _prevXVelocity;

    private Rigidbody2D _rigidBody;
    private float _spawnTimer;

    private SpriteRenderer _spriteRenderer;
    private bool _stuck;
    public Attack ActiveAttack;
    public Scroll Scroll;
    public Special.Special Special;

    protected override void Awake()
    {
        base.Awake();
        // IMPORTANT: this has to run first
        Random.InitState(DateTime.Now.Millisecond);
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        _mainCamera = Camera.main;
        wandObject.SetActive(false);
    }

    private void Update()
    {
        ActiveAttack = new IceSpikeAttack();

        if (ControlsDisabled()) return;

        // moving
        var justLanded = IsGrounded() && !_prevGrounded && lastPlatform != null;
        if (justLanded)
        {
            _rigidBody.velocity = new Vector2(_prevXVelocity, _rigidBody.velocity.y);
            LevelManager.Instance.PlayerOnPlatform(lastPlatform);
            lastStandingPlatform = lastPlatform;
        }

        _animator.SetBool(IsWalking, _hVelocity != 0);
        if (_hVelocity != 0f)
        {
            var scale = transform.localScale;
            scale.x = _hVelocity < 0 ? -1f : 1f;
            transform.localScale = scale;
            var horizontalForce = Time.deltaTime * (_hVelocity * speed);
            if (!IsGrounded()) horizontalForce *= SpeedFractionWhileJumping;
            _rigidBody.AddForce(Vector2.right * horizontalForce);
        }

        var clampedHVelocity = Math.Clamp(_rigidBody.velocity.x, -maxSpeed, maxSpeed);
        _rigidBody.velocity = new Vector2(clampedHVelocity, _rigidBody.velocity.y);
        _prevXVelocity = clampedHVelocity;

        // jumping
        if (_isJumpHeld)
        {
            if (IsGrounded() && _jumpStage == JumpStage.NoJump)
            {
                _jumpStage = JumpStage.JumpHeld;
                _jumpHeldDuration = 0f;
                _rigidBody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                audioSource.PlayOneShot(jumpClip);
            }
            else if (_jumpStage == JumpStage.JumpHeld && _jumpHeldDuration <= jumpAdditionDuration)
            {
                _jumpHeldDuration += Time.deltaTime;
                _rigidBody.AddForce(Vector2.up * (jumpAddition * Time.deltaTime), ForceMode2D.Impulse);
            }
        }
        else
        {
            if (IsGrounded())
                _jumpStage = JumpStage.NoJump;
            else
                _jumpStage = JumpStage.JumpReleased;
        }

        _prevGrounded = IsGrounded();

        _animator.SetBool(IsJumping, !IsGrounded());


        // spawn timer
        _animator.SetBool(IsInvincible, HasInvincibility());
        if (_spawnTimer > 0f) _spawnTimer -= Time.deltaTime;

        // attack timer
        if (_attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }
        else if (_isAttackHeld)
        {
            _attackCooldown = attackSpeed;
            Shoot();
        }

        // wand
        wandObject.SetActive(Special != null);
        if (Special != null) Special.Update();

        // orbs
        wandLight.intensity = orbs >= 1 ? 1f : 0f;

        // passing to other side of screen
        if (Math.Abs(transform.position.x) > CameraUtil.Instance.worldWidth / 2)
        {
            var transform1 = transform;
            var pos = transform1.position;
            pos.x = -pos.x * 0.95f;
            transform1.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var platform = col.gameObject.GetComponent<Platform>();
        if (platform != null) lastPlatform = platform;

        var lava = col.gameObject.GetComponent<Lava>();
        if (lava != null) OnLava(lava);
    }

    private void OnLava(Lava lava)
    {
        audioSource.PlayOneShot(lava.lavaDeathClip);
        var isDead = LoseHealth(lavaDamage);
        if (!isDead)
        {
            GameObject lowestPlatform = null;
            foreach (var platform in FindObjectsOfType<Platform>())
                if (platform.transform.position.y >= lava.transform.position.y + 1f && (lowestPlatform == null ||
                        platform.transform.position.y < lowestPlatform.transform.position.y))
                    lowestPlatform = platform.gameObject;

            if (lowestPlatform != null)
                gameObject.transform.position = (Vector2)lowestPlatform.transform.position + Vector2.up;
        }
    }

    public void OnGainHealth(float gainedHealth)
    {
        ChangeHealth(gainedHealth);
    }

    public bool LoseHealth(float lostHealth)
    {
        if (HasInvincibility()) return false;

        ChangeHealth(-lostHealth);
        _isDead = health == 0f;
        if (_isDead) OnDie();
        else _spawnTimer = 1f;
        return _isDead;
    }

    public void ChangeHealth(float healthChange)
    {
        health = Math.Clamp(health + healthChange, 0f, maxHealth);
    }

    public void UseOrb()
    {
        ChangeOrbs(-1);
    }

    public void ChangeOrbs(int orbsChange)
    {
        orbs = Math.Clamp(orbs + orbsChange, 0, maxOrbs);
    }

    public void OnDie()
    {
        Time.timeScale = 0f;
        GameGUI.Instance.gameOverScreen.SetActive(true);
    }

    public bool HasInvincibility()
    {
        return _spawnTimer > 0f || invincible;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (_isDead) return;

        if (context.performed || context.canceled)
        {
            var value = context.ReadValue<Vector2>();
            _hVelocity = value.x;
            _isJumpHeld = value.y > 0;
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (_isDead) return;
        _isAttackHeld = context.ReadValueAsButton();
    }

    public void CastSpell(InputAction.CallbackContext context)
    {
        if (ControlsDisabled()) return;
        if (Special != null)
        {
            if (context.performed)
            {
                Special.Cast(GetMousePositionInWorld());
                Special.CastStart();
            }
            else if (context.canceled)
            {
                Special.CastEnd();
            }
        }
    }

    public void UseScroll(InputAction.CallbackContext context)
    {
        if (ControlsDisabled()) return;
        audioSource.Play();
        if (context.performed && Scroll != null) Scroll.Cast(GetMousePositionInWorld());
    }


    public void Look(InputAction.CallbackContext context)
    {
        if (_isDead) return;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (_isDead) return;
        if (context.performed)
        {
            GameGUI.Instance.interactionPrompt.Interact(false);
            GameGUI.Instance.choiceInteractionPrompt.Interact(false);
        }
    }

    public void Cheat(InputAction.CallbackContext context)
    {
        if (ControlsDisabled()) return;
        if (context.performed) LevelManager.Instance.SkipToNextLevel();
    }

    public void Retry(InputAction.CallbackContext context)
    {
        if (context.performed && _isDead)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Quit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isDead) Application.Quit();
            else GameGUI.Instance.choiceInteractionPrompt.Interact(true);
        }
    }

    private Vector2 GetMousePositionInWorld()
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePosition.z = 0f;
        return mousePosition;
    }

    private void Shoot()
    {
        if (ActiveAttack == null || ControlsDisabled()) return;
        audioSource.PlayOneShot(ActiveAttack.GetClip());
        var mousePosition = GetMousePositionInWorld();
        Vector2 playerPosition = gameObject.transform.position;
        var fireballDirectionVector = (mousePosition - playerPosition).normalized;
        var fireballStartPosition = playerPosition + fireballDirectionVector * 0.5f;
        var rotation = Quaternion.FromToRotation(Vector2.right, fireballDirectionVector);
        Instantiate(ActiveAttack.GetPrefab(), fireballStartPosition, rotation);
    }

    private bool IsGrounded()
    {
        return _rigidBody.velocity.y == 0;
    }

    public IEnumerator SetInvincibleFor(float seconds)
    {
        GameGUI.Instance.DisplayMessage("Invincible!", playerMessage: true);
        invincible = true;
        yield return new WaitForSeconds(seconds);
        GameGUI.Instance.DisplayMessage("Invincibility ended!", GameGUI.MessageTone.Negative, true);
        invincible = false;
    }

    public void OnHit(float hitDamage, GameObject hitBy)
    {
        if (HasInvincibility()) return;
        if (Random.value <= dodgeChance)
        {
            GameGUI.Instance.DisplayMessage("Dodged!", playerMessage: true);
            return;
        }

        var impactVector = transform.position - hitBy.transform.position;
        var knockbackVector = new Vector2(impactVector.x * 10f, 5f);
        _rigidBody.AddForce(knockbackVector, ForceMode2D.Impulse);
        LoseHealth(hitDamage);
        audioSource.PlayOneShot(hitClip);
        if (thorns > 0f)
        {
            var hittable = hitBy.GetComponent<Hittable>();
            if (hittable != null) hittable.OnHit(-knockbackVector.normalized, hitDamage * thorns);
        }
    }

    private bool ControlsDisabled()
    {
        return _isDead || _stuck;
    }

    public void OnStuck()
    {
        GameGUI.Instance.DisplayMessage("Stuck!", GameGUI.MessageTone.Negative, true);
        _stuck = true;
        _animator.SetBool(IsWalking, false);
        StartCoroutine(UnStuck());
    }

    private IEnumerator UnStuck()
    {
        yield return new WaitForSeconds(1f);
        _stuck = false;
    }

    private enum JumpStage
    {
        NoJump,
        JumpHeld,
        JumpReleased
    }
}