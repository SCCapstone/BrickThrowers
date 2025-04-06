using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

/// <summary>
/// Defines what a player should do.
/// </summary>
public class Player : Diver
{
    /*
     * A player class should be a parent class where all the player related classes should inherit from.
     * As a parent class, it should have all the common properties and methods that all the player related classes should have.
     * Players should have movement, health, and other properties that are common to all players.
     * The setting takes place in water, so movement nautrually should be slow to simulate the resistance of water.
     * This game is also in a 2D environment, so movement should be restricted to the x and y axis.
     * Additionally, players should have inventory, and interaction with inventory items and other objects in the game.
     * Actual in-game player classes should inherit from this class and implement their own unique properties and methods.
     * Players all emit some form of light, so they should have a light source attached to them.
     */

    // Input actions
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction sprint;
    private InputAction subInteract;

    // Movement
    public float speed = 40f;
    public float verticalSpeed = 40f;

    // Multiplier
    private float currentSpeedMultiplier = 1f;
    private const float BASE_SPEED_MULTIPLIER = 1f;
    private const float FAST_SPEED_MULTIPLIER = 1.5f;
    private const float SLOW_SPEED_MULTIPLIER = 0.7f;


    public AudioSource swimsfx;
    private bool isSwimmingFast = false;
    private Vector2 moveDirection;

    private float baseSpeed = 45f;
    private float baseVerticalSpeed = 45f;

    // Water Physics
    public float waterDrag = 3f;
    public float waterGravityScale = 50f;

    // Oxygen and Stamina
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 1f;
    public float lowStaminaMultiplier = 5f;

    public float maxStamina = 20f;
    public float staminaDepletionRate = 2f;
    public float staminaRecoveryRate = 1f;
    public float lowStaminaThreshold = 5f;

    public GameObject SummaryScreen;
    public GameObject ruSureScreen;
    public GameObject Clock;
    public TimerScript timer;
    public float playerTime;
    private bool isDead;

    public float gameTime;

    // Stamina (oxygen for Players will be covered by the Diver's oxygenLevel)
    private float currentStamina;

    public Rigidbody2D rb;
    public SpriteRenderer playerSprite;
    private Vector2 movement;

    //Xp and Currency
    public int currentXp,
        maxXp,
        currency,
        currentLevel;

    public int artifactsGot = 0;

    // Status effects
    [SerializeField]
    public bool isPoisoned = false;

    // Debug mode
    private bool godMode = false;

    // Keybinds
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode completeMission = KeyCode.C;

    // Submarine Controls
    public float submarineSpeed = 70f;
    public GameObject submarine;
    public Rigidbody2D submarineRb;
    public GameObject visibilityRadius;
    public float visibilityScaleFactor = 1.8f;

    // Submarine variables
    private bool isInSubmarine = false;
    private bool nearSubmarine = false;

    //private KeyCode subInteract = KeyCode.L;

    // Inventory Management
    public static InventoryManager inventory;

    #region Setup Functions
    private void Awake()
    {
        playerControls = new PlayerInputActions();
        sprint = GetComponent<PlayerInput>().actions["Sprint"];

        sprint.started += OnSprintPress;
        sprint.canceled += OnSprintRelease;
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        subInteract = playerControls.Player.InteractSubmarine;

        move.Enable();
        subInteract.Enable();

        PauseMenu.onGodMode += GodMode;
        SeaWeed.onPlayerSlowedDown += SeaweedSpeedSlowed;
        SeaWeed.onPlayerSpeedRestored += SeaweedSpeedRestored;
    }

    private void OnDisable()
    {
        move.Disable();
        sprint.Disable();
        subInteract.Disable();

        PauseMenu.onGodMode -= GodMode;
        SeaWeed.onPlayerSlowedDown -= SeaweedSpeedSlowed;
        SeaWeed.onPlayerSpeedRestored -= SeaweedSpeedRestored;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = GetComponent<InventoryManager>();
        rb.gravityScale = waterGravityScale;
        rb.drag = waterDrag;
        currentStamina = maxStamina;
    }

    public void Update()
    {
        base.Update();
        if (isInSubmarine)
        {
            MoveSubmarine();
            if (subInteract.WasPressedThisFrame())
            {
                ToggleSubmarine();
            }
        }
        else
        {
            moveDirection = move.ReadValue<Vector2>();
        }
        if (nearSubmarine && subInteract.WasPressedThisFrame())
        {
            ToggleSubmarine();
        }
        OxygenAndStamina();
        gameOver();
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!isInSubmarine)
        {
            speed = baseSpeed * currentSpeedMultiplier;
            verticalSpeed = baseVerticalSpeed * currentSpeedMultiplier;
            Movement(new Vector2(moveDirection.x * speed, moveDirection.y * verticalSpeed));
        }
    }
    #endregion Setup Functions
    #region Movement, Oxygen, Stamina
    /// <summary>
    /// Determines movement and plays SFX of movement.
    /// </summary>
    /// <param name="moveVec"></param>
    private void Movement(Vector2 moveVec)
    { 
        rb.velocity = moveVec;
        if (!swimsfx.isPlaying)
            swimsfx.Play();
    }

    /// <summary>
    /// Manages oxygen and stamina
    /// </summary>
    private void OxygenAndStamina()
    {
        if (isInSubmarine)
        {
            oxygenLevel += oxygenDepletionRate * Time.deltaTime * 2;
            oxygenLevel = Mathf.Clamp(oxygenLevel, 0, maxOxygen);
        }

        float oxygenDepletion = oxygenDepletionRate * Time.deltaTime;

        if (currentStamina <= lowStaminaThreshold)
        {
            oxygenDepletion *= lowStaminaMultiplier;
        }

        oxygenLevel -= oxygenDepletion;
        oxygenLevel = Mathf.Clamp(oxygenLevel, 0, maxOxygen);

        if (isSwimmingFast)
        {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public float GetOxygenLevel() => oxygenLevel;

    public float GetStaminaLevel() => currentStamina;

    /// <summary>
    /// Allows the player to swim faster.
    /// </summary>
    //public void Sprint()
    //{
    //    if (sprint.IsPressed() && currentStamina > 0)
    //    {
    //        isSwimmingFast = true;
    //        // Use the base speeds multiplied by the sprint factor.
    //        speed = baseSpeed * FAST_SPEED_MULTIPLIER;
    //        verticalSpeed = baseVerticalSpeed * FAST_SPEED_MULTIPLIER;
    //    }
    //    else
    //    {
    //        isSwimmingFast = false;
    //        // Revert back to the normal speeds.
    //        speed = baseSpeed;
    //        verticalSpeed = baseVerticalSpeed;
    //    }
    //}

    public void OnSprintPress(InputAction.CallbackContext context)
    {
        isSwimmingFast = true;
        currentSpeedMultiplier = FAST_SPEED_MULTIPLIER;
    }

    public void OnSprintRelease(InputAction.CallbackContext context)
    {
        isSwimmingFast = false;
        currentSpeedMultiplier = BASE_SPEED_MULTIPLIER;
    }
    #endregion
    #region Status Effects

    // Added because was in Lionfish.cs, and causing compile errors
    /// <summary>
    /// Applies a "Poison" effect to the player. Posion should cause increased oxygen decay, and halt
    /// stamina recovery.
    /// </summary>
    [ContextMenu("Apply Poison")]
    public void ApplyPoison()
    {
        isPoisoned = true;
        oxygenDepletionRate *= 5;
        staminaRecoveryRate = 0;
    }

    /// <summary>
    /// Undos the "Poison" effect on the player. Restores oxygen decay and stamina recovery to normal.
    /// </summary>
    [ContextMenu("Relieve Poison")]
    public void RelievePoison()
    {
        isPoisoned = false;
        oxygenDepletionRate /= 5;
        staminaRecoveryRate = 1;
    }

    // Added becasue was in Squid.cs, and casuing compile errors
    public void Blind(float duration) { }

    // Added because was in Jellyfish.cs, and causing compile errors
    public void Stun(float duration) { }

    /// <summary>
    /// Slows down the player's speed when they collide with seaweed.
    /// </summary>
    /// <param name="speed">The new speed value to apply after slowing down.</param>
    public void SeaweedSpeedSlowed(float speed)
    {
        currentSpeedMultiplier = SLOW_SPEED_MULTIPLIER;
        sprint.Disable();
    }

    /// <summary>
    /// Returns the player's speed to normal after colliding with seaweed.
    /// </summary>
    /// <param name="speed"></param>
    public void SeaweedSpeedRestored()
    {
        currentSpeedMultiplier = BASE_SPEED_MULTIPLIER;
        sprint.Enable();

    }

    #endregion

    // Added because was in PirateB.cs, and causing compile errors
    public bool HasArtifact()
    {
        return true;
    }

    //Counts up the artifacts you have gotten
    public string ArtifactCount()
    {
        if (HasArtifact())
        {
            artifactsGot += 1;
        }
        string artifactstaken = artifactsGot.ToString();
        return artifactstaken;
    }

    //Getting timer for player
    public float getTimer()
    {
        playerTime = timer.TimeLeft;
        Debug.Log(playerTime);
        return playerTime;
    }

    public void gameOver()
    {
        if (GetOxygenLevel() <= 0 && !isDead || getTimer() == 0)
        {
            isDead = true;

            SummaryScreen.SetActive(true);
        }
        if(Input.GetKeyDown(completeMission))
        {
            // Might need to check if need to re-enable cursor
            ruSureScreen.SetActive(true);
        }
    }

    public void RemoveArtifact() { }

    // Added becasue was in Octopus.cs, and causing compile errors.
    public void SuppressLight(bool val) { }

    public void SuppressMovement(bool val) { }

    public int GetNearbyDivers()
    {
        return 0;
    }

    //Handling XP and Level up
    private void HandleExperienceChange(int newExperience)
    {
        currentXp += newExperience;
        if (currentXp >= maxXp)
        {
            LevelUp();
        }
    }

    //Focuses on Levelup and increases Xp cap
    private void LevelUp()
    {
        currentLevel++;
        currency += 10;
        maxXp += 100;
        currentXp = 0;
    }

    #region Cheat Mode
    /// <summary>
    /// Turns on a cheat mode for the player. This should be used for debugging purposes only.
    /// The player should not be able to use this in the final game.
    /// This prevents oxygen depletion, loss of stamina when sprinting,
    /// </summary>
    private void GodMode()
    {
        // First, invert the bool. This covers the button being used to enable/disable god mode.
        godMode = !godMode;

        if (godMode)
        {
            // If there is god mode, then the player should not lose oxygen or stamina.
            oxygenDepletionRate = 0;
            staminaDepletionRate = 0;
        }
        else
        {
            // No god mode? Return to normal.
            oxygenDepletionRate = 1;
            staminaDepletionRate = 2;
        }
    }

    public override bool GodModeStatus()
    {
        return godMode;
    }
    #endregion
    #region Collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == submarine)
        {
            nearSubmarine = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == submarine)
        {
            nearSubmarine = false;
            Debug.Log("Left the sub");
        }
    }
    #endregion
    #region Submarine movement
    private void MoveSubmarine()
    {
        if (!isInSubmarine || submarineRb == null)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float x = horizontalInput * speed;
        float y = verticalInput * verticalSpeed;

        Vector2 move = new Vector2(horizontalInput, verticalInput) * submarineSpeed;
        submarineRb.velocity = move;
    }

    private void ToggleSubmarine()
    {
        isInSubmarine = !isInSubmarine;

        if (isInSubmarine)
        {
            playerSprite.enabled = false;
            rb.simulated = false;
            Vector3 subScale = visibilityRadius.transform.localScale;
            subScale *= visibilityScaleFactor;
            visibilityRadius.transform.localScale = subScale;
            // transform.position = submarine.transform.position;
            transform.SetParent(submarine.transform);
            transform.localPosition = Vector2.zero;
            // Unfreeze rigid body constrains
            submarineRb.constraints = RigidbodyConstraints2D.None;
            Debug.Log("Entering sub");
            inventory.RemoveArtifacts();
        }
        else
        {
            playerSprite.enabled = true;
            rb.simulated = true;
            Vector3 subScale = visibilityRadius.transform.localScale;
            subScale /= visibilityScaleFactor;
            visibilityRadius.transform.localScale = subScale;
            transform.SetParent(null);
            transform.position = submarine.transform.position + new Vector3(0, -1, -1);
            Debug.Log("Leaving sub");
            // Add constraints to the submarine, freezing the entire submersible.
            submarineRb.constraints = RigidbodyConstraints2D.FreezeAll;
            submarineRb.velocity = Vector2.zero;
        }
    }

    #endregion
}
