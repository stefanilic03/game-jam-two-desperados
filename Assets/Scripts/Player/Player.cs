using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, PlayerEntity
{
    public PlayerController playerController;
    PlayerInput playerInput;
    public Rigidbody2D rigidBody2D;

    public static UnityAction timeTunnelTravel;
    public bool inTheTimeTunnel = false;
    public string playerLayer = "Player";
    public string groundLayer = "Ground";
    public float platformCollisionCooldown = 0f;
    public Animator animator;
    public GameObject backgroundTintWhileInATunnel;

    public static UnityAction takeDamage;

    //Movement
    public float defaultMovementSpeed;
    public float movementSpeed;
    public float speedMultiplier;
    public float currentSpeed;


    public HealthBar healthBar;
    public SpriteRenderer spriteRenderer;
    private Color takeDamageColor = new Color(1f, 0.45f, 0.55f, 0.6f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    private float takeDamageTimer = 0;
    private float takeDamageColorShiftTimer = 1f;

    float randomDamageTaken;

    float healthLossRate = 3f;

    public float MaximumHealthPoints { get; set; }
    public float CurrentHealthPoints { get; set; }

    //int minimunHealFromDefeatedEnemy = 25;
    //int maximunHealFromDefeatedEnemy = 40;

    const string deathAnimation = "death";

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        takeDamage += TakeDamage;

        MaximumHealthPoints = 100;
        SetCurrentHealthToMaximum();

        timeTunnelTravel += TimeTunnelTravel;

        defaultMovementSpeed = movementSpeed;
    }

    private void Start()
    {
        CurrentHealthPoints = MaximumHealthPoints;

        healthBar.SetMaximumHealth(MaximumHealthPoints);

        InvokeRepeating(nameof(LoseHealthOverTime), 3, 1);
    }

    private void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(movementSpeed * speedMultiplier, rigidBody2D.velocity.y);

        if (inTheTimeTunnel)
        {
            CurrentHealthPoints += 2.5f;
            healthBar.SetHealth(CurrentHealthPoints);
        }
        if (inTheTimeTunnel)
        {
            playerController.jetpackBar.currentJetpackFuel += 2f;
        }
    }

    private void Update()
    {
        if (Time.time >= takeDamageTimer)
        {
            spriteRenderer.color = normalColor;
        }
        if (Time.time > platformCollisionCooldown && !inTheTimeTunnel)
        {
            Physics2D.IgnoreLayerCollision(3, 6, false);
        }
    }

    void LoseHealthOverTime()
    {
        CurrentHealthPoints -= healthLossRate;
        healthBar.SetHealth(CurrentHealthPoints);
        if (CurrentHealthPoints <= 0)
        {
            Death();
            return;
        }
    }

    public void HealHealth()
    {
        //TODO: Heal health over time based on the value from the killed enemy
    }

    public void SetCurrentHealth(float health)
    {
        CurrentHealthPoints = health;
    }

    public void SetCurrentHealthToMaximum()
    {
        CurrentHealthPoints = MaximumHealthPoints;
    }

    public void TakeDamage()
    {
        randomDamageTaken = Random.Range(10f, 20f);
        CurrentHealthPoints -= randomDamageTaken;
        healthBar.SetHealth(CurrentHealthPoints);
        if (CurrentHealthPoints <= 0)
        {
            Death();
            return;
        }

        spriteRenderer.color = takeDamageColor;
        takeDamageTimer = Time.time + takeDamageColorShiftTimer;      
    }

    public void Death()
    {
        animator.Play(deathAnimation);
        Destroy(gameObject);
    }

    void TimeTunnelTravel()
    {
        inTheTimeTunnel = !inTheTimeTunnel;
        if (inTheTimeTunnel)
        {
            movementSpeed *= 1.5f;
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), LayerMask.NameToLayer(groundLayer), true);
            Physics2D.IgnoreLayerCollision(3, 6, true);
            playerInput.DeactivateInput();
            playerController.jumpsAvailable = 2;
            playerController.isFalling = true;
            backgroundTintWhileInATunnel.SetActive(true);
        }
        if (!inTheTimeTunnel)
        {
            movementSpeed = defaultMovementSpeed;
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            platformCollisionCooldown = Time.time + 0.5f;
            playerInput.ActivateInput();
            playerController.isFalling = false;
            backgroundTintWhileInATunnel.SetActive(false);
        }
    }
}
