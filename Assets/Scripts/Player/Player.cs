using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour, PlayerEntity
{
    public PlayerController playerController;
    public PlayerInput playerInput;
    public Rigidbody2D rigidBody2D;

    public JetpackResourceBar jetpackBar;

    public UnityAction timeTunnelTravel;
    public bool inTheTimeTunnel = false;
    public float platformCollisionCooldown = 0f;

    public Animator animator;
    public GameObject backgroundTintWhileInATunnel;

    public GameObject deathMenu;
    public TMP_Text currentScoreDeathMenuText;

    public UnityAction takeDamage;
    public UnityAction timePortal;
    public UnityAction healthPowerUp;
    public UnityAction jetpackPowerUp;

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

    float healthLossRate = 6f;

    public float MaximumHealthPoints { get; set; }
    public float CurrentHealthPoints { get; set; }

    public GameObject pauseMenu;
    public bool gamePaused = false;
    public Button resumeButton;
    public Button retryButton;
    int minimumHeal = 22;
    int maximumHeal = 35;
    public bool endGame = false;

    const string deathAnimation = "death";

    const string UIActionMapName = "UI";
    const string playerActionMapName = "PlayerActions";

    void Awake()
    {
        MaximumHealthPoints = 100;
        SetCurrentHealthToMaximum();

        defaultMovementSpeed = movementSpeed;
    }

    private void Start()
    {
        if (playerInput.currentActionMap.name == "UIActionMapName")
        {
            playerInput.SwitchCurrentActionMap(playerActionMapName);
        }

        CurrentHealthPoints = MaximumHealthPoints;

        healthBar.SetMaximumHealth(MaximumHealthPoints);

        InvokeRepeating(nameof(GraduallyIncreaseMovementSpeed), 0, 7);
        InvokeRepeating(nameof(LoseHealthOverTime), 4, 1.1f);
    }

    private void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(movementSpeed * speedMultiplier, rigidBody2D.velocity.y);

        InTheTimeTunnel();
    }

    private void GraduallyIncreaseMovementSpeed()
    {
        speedMultiplier += 0.08f;
    }

    private void InTheTimeTunnel()
    {
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
        if (!endGame)
        {
            CurrentHealthPoints -= healthLossRate;
            healthBar.SetHealth(CurrentHealthPoints);
            if (CurrentHealthPoints <= 0)
            {
                Death();
                return;
            }
        }
    }

    public void HealHealth()
    {
        CurrentHealthPoints += Random.Range(minimumHeal, maximumHeal);
        if (CurrentHealthPoints > MaximumHealthPoints)
        {
            CurrentHealthPoints = MaximumHealthPoints;
        }
        healthBar.SetHealth(CurrentHealthPoints);        
    }    
    
    public void JetpackPowerUp()
    {
        jetpackBar.currentJetpackFuel += Random.Range(minimumHeal, maximumHeal);
        if (jetpackBar.currentJetpackFuel > jetpackBar.maximumJetpackFuel)
        {
            jetpackBar.currentJetpackFuel = jetpackBar.maximumJetpackFuel;
        }
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
        deathMenu.SetActive(true);
        currentScoreDeathMenuText.text = "Current score: " + GameMaster.currentScore.ToString();
        GameMaster.gameMaster.gameIsOn = false;
        this.enabled = false;
        playerInput.SwitchCurrentActionMap(UIActionMapName);
        retryButton.Select();
    }

    void TimeTunnelTravel()
    {
        inTheTimeTunnel = !inTheTimeTunnel;
        if (inTheTimeTunnel)
        {
            movementSpeed *= 1.5f;
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            Physics2D.IgnoreLayerCollision(3, 6, true);
            playerInput.DeactivateInput();
            playerController.jumpsAvailable = 3;
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

    public void PauseGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && !gamePaused)
        {
            gamePaused = true;
            pauseMenu.SetActive(true);
            resumeButton.Select();
            playerInput.SwitchCurrentActionMap(UIActionMapName);
            Time.timeScale = 0;
            return;
        }
        if (callbackContext.performed && gamePaused)
        {
            gamePaused = false;
            pauseMenu.SetActive(false);
            playerInput.SwitchCurrentActionMap(playerActionMapName);
            Time.timeScale = 1;
        }
    }

    void TimePortal()
    {
        movementSpeed = 0;
        endGame = true;
    }

    private void OnEnable()
    {
        jetpackPowerUp += JetpackPowerUp;
        healthPowerUp += HealHealth;
        timeTunnelTravel += TimeTunnelTravel;
        takeDamage += TakeDamage;
        timePortal += TimePortal;
    }

    private void OnDisable()
    {
        jetpackPowerUp -= JetpackPowerUp;
        healthPowerUp -= HealHealth;
        timeTunnelTravel -= TimeTunnelTravel;
        takeDamage -= TakeDamage;
        timePortal -= TimePortal;
    }
}
