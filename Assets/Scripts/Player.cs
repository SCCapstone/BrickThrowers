using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

/// <summary>
/// Defines what a player should do.
/// </summary>
public class Player : Diver {
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
  private InputAction subLeaveLevel;

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
  private const float DEFAULT_OXYGEN_DEPLETION_RATE = 1f;

  public float maxStamina = 20f;
  public float staminaDepletionRate = 2f;
  public float staminaRecoveryRate = 1f;
  public float lowStaminaThreshold = 5f;
  private const float DEFAULT_STAMINA_DEPLETION_RATE = 2f;

  public GameObject SummaryScreen;
  [SerializeField] private SummaryScreenUI summaryScreenUI;
  public GameObject Clock;
  public TimerScript timer;
  [SerializeField] private GameObject areYouSureScreen;
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

  // Submarine Controls
  public float submarineSpeed = 70f;
  public GameObject submarine;
  public Rigidbody2D submarineRb;
  public GameObject visibilityRadius;
  public float visibilityScaleFactor = 1.8f;

  // Submarine variables
  private bool isInSubmarine = false;
  private bool nearSubmarine = false;

  // Inventory
  [SerializeField] private InventoryManager inventory;
  #region Setup Functions
  private void Awake() {
    playerControls = new PlayerInputActions();
    sprint = GetComponent<PlayerInput>().actions["Sprint"];
    

    sprint.started += OnSprintPress;
    sprint.canceled += OnSprintRelease;
    
  }

  private void OnEnable() {
    move = playerControls.Player.Move;
    subInteract = playerControls.Player.InteractSubmarine;
    subLeaveLevel = playerControls.Player.ReturnToMainMenu;

    move.Enable();
    subInteract.Enable();
    subLeaveLevel.Enable();

    GodModeIndicator.onGodModeActivated += GodMode;
    SeaWeed.onPlayerSlowedDown += SeaweedSpeedSlowed;
    SeaWeed.onPlayerSpeedRestored += SeaweedSpeedRestored;
    Diver.onDamage += ChangeColor;
    Diver.onDeath += GameOver;
    subLeaveLevel.performed += SubmarineLeaveLevel;

    rb.simulated = true;
  }

  private void OnDisable() {
    move.Disable();
    sprint.Disable();
    subInteract.Disable();
    subLeaveLevel.Disable();

    GodModeIndicator.onGodModeActivated -= GodMode;
    SeaWeed.onPlayerSlowedDown -= SeaweedSpeedSlowed;
    SeaWeed.onPlayerSpeedRestored -= SeaweedSpeedRestored;
    Diver.onDamage -= ChangeColor;
    Diver.onDeath -= GameOver;
  }

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    rb.gravityScale = waterGravityScale;
    rb.drag = waterDrag;
    currentStamina = maxStamina;
  }

  public void Update() {
    base.Update();
    if (isInSubmarine) {
      MoveSubmarine();
      if (subInteract.WasPressedThisFrame()) {
        ToggleSubmarine();
      }
    } else {
      moveDirection = move.ReadValue<Vector2>();
    }
    if (nearSubmarine && subInteract.WasPressedThisFrame()) {
      ToggleSubmarine();
    }
    OxygenAndStamina();
    GameOver();
    moveDirection = move.ReadValue<Vector2>();
  }

  private void FixedUpdate() {
    if (!isInSubmarine) {
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
  private void Movement(Vector2 moveVec) {
    rb.velocity = moveVec;
    if (!swimsfx.isPlaying)
      swimsfx.Play();
  }

  /// <summary>
  /// Manages oxygen and stamina
  /// </summary>
  private void OxygenAndStamina() {
    if (isInSubmarine) {
      oxygenLevel += oxygenDepletionRate * Time.deltaTime * 2;
      oxygenLevel = Mathf.Clamp(oxygenLevel, 0, maxOxygen);
    }

    float oxygenDepletion = oxygenDepletionRate * Time.deltaTime;

    if (currentStamina <= lowStaminaThreshold) {
      oxygenDepletion *= lowStaminaMultiplier;
    }

    oxygenLevel -= oxygenDepletion;
    oxygenLevel = Mathf.Clamp(oxygenLevel, 0, maxOxygen);

    if (isSwimmingFast) {
      currentStamina -= staminaDepletionRate * Time.deltaTime;
    } else {
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

  public void OnSprintPress(InputAction.CallbackContext context) {
    isSwimmingFast = true;
    currentSpeedMultiplier = FAST_SPEED_MULTIPLIER;
  }

  public void OnSprintRelease(InputAction.CallbackContext context) {
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
  public override void ApplyPoison() {
    if (isPoisoned) return; // Prevents multiple poison applications
    isPoisoned = true;
    oxygenDepletionRate *= 5;
    staminaRecoveryRate = 0;
    Debug.Log("Player poisoned.");
  }

  /// <summary>
  /// Undos the "Poison" effect on the player. Restores oxygen decay and stamina recovery to normal.
  /// </summary>
  [ContextMenu("Relieve Poison")]
  public void RelievePoison() {
    isPoisoned = false;
    oxygenDepletionRate /= 5;
    staminaRecoveryRate = 1;
  }

  // Added becasue was in Squid.cs, and casuing compile errors
  public void Blind(float duration) { }

  /// <summary>
  /// Slows down the player's speed when they collide with seaweed.
  /// </summary>
  /// <param name="speed">The new speed value to apply after slowing down.</param>
  public void SeaweedSpeedSlowed(float speed) {
    currentSpeedMultiplier = SLOW_SPEED_MULTIPLIER;
    sprint.Disable();
  }

  /// <summary>
  /// Returns the player's speed to normal after colliding with seaweed.
  /// </summary>
  /// <param name="speed"></param>
  public void SeaweedSpeedRestored() {
    currentSpeedMultiplier = BASE_SPEED_MULTIPLIER;
    sprint.Enable();

  }

  #endregion

  // Added because was in PirateB.cs, and causing compile errors
  public bool HasArtifact() {
    return true;
  }

  //Counts up the artifacts you have gotten
  public string ArtifactCount() {
    if (HasArtifact()) {
      artifactsGot += 1;
    }
    string artifactstaken = artifactsGot.ToString();
    return artifactstaken;
  }

  //Getting timer for player
  public float getTimer() {
    playerTime = timer.TimeLeft;
    //Debug.Log(playerTime);
    return playerTime;
  }

  public void GameOver() {
    if (GetOxygenLevel() <= 0 && !isDead || getTimer() == 0) {
      isDead = true;
      Debug.Log("Game is Over!");
      move.Disable();
      sprint.Disable();
      subInteract.Disable();
      subLeaveLevel.Disable();
      rb.simulated = false;
      SummaryScreen.SetActive(true);
    }
  }

  public void RemoveArtifact() { }

  #region Octopus Latching Logic
  // Added becasue was in Octopus.cs, and causing compile errors.
  public void SuppressLight(bool val) {
    GameObject playerVisibility = this.gameObject.transform.GetChild(1).gameObject;
    if (val) {

      playerVisibility.SetActive(false);
    } else {
      playerVisibility.SetActive(true);
    }
  }

  public void SuppressMovement(bool val) {
    if (val) {
      playerControls.Disable();
    } else {
      playerControls.Enable();
    }
  }

  public int GetNearbyDivers() {
    return 0;
  }
  #endregion
  //Handling XP and Level up
  private void HandleExperienceChange(int newExperience) {
    currentXp += newExperience;
    if (currentXp >= maxXp) {
      LevelUp();
    }
  }

  //Focuses on Levelup and increases Xp cap
  private void LevelUp() {
    currentLevel++;
    currency += 10;
    maxXp += 100;
    currentXp = 0;
  }
  #region Stun Logic
  public override async void Stun(float duration) {
    isStunned = true;
    stunTimer = duration;

    // Stun player
    playerControls.Disable();

    await Task.Delay(TimeSpan.FromSeconds(duration));

    isStunned = false;
    playerControls.Enable();
    Debug.Log("Diver is no longer stunned.");

  }

  #endregion
  #region Cheat Mode
  /// <summary>
  /// Turns on a cheat mode for the player. This should be used for debugging purposes only.
  /// The player should not be able to use this in the final game - well, except if they really wanted to via Options.
  /// This prevents oxygen depletion, loss of stamina when sprinting,
  /// </summary>
  private void GodMode(bool godModeActivation) {
    godMode = godModeActivation;

    Debug.Log($"Acquired signal from GodModeIndicator. God mode is now " + (godMode ? "enabled." : "disabled."));

    if (godMode) {
      // If there is god mode, then the player should not lose oxygen or stamina.
      oxygenDepletionRate = 0;
      staminaDepletionRate = 0;
    } else {
      // No god mode? Return to normal.
      oxygenDepletionRate = DEFAULT_OXYGEN_DEPLETION_RATE;
      staminaDepletionRate = DEFAULT_STAMINA_DEPLETION_RATE;
    }
  }

  public override bool GodModeStatus() {
    return godMode;
  }
  #endregion
  #region Collision
  public void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject == submarine) {
      nearSubmarine = true;
    }
  }

  public void OnTriggerExit2D(Collider2D collision) {
    if (collision.gameObject == submarine) {
      nearSubmarine = false;
    }
  }
  #endregion
  #region Submarine movement
  private void MoveSubmarine() {
    if (!isInSubmarine || submarineRb == null)
      return;

    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    float x = horizontalInput * speed;
    float y = verticalInput * verticalSpeed;

    Vector2 move = new Vector2(horizontalInput, verticalInput) * submarineSpeed;
    submarineRb.velocity = move;
  }

  private void ToggleSubmarine() {
    isInSubmarine = !isInSubmarine;

    if (isInSubmarine) {
      playerSprite.enabled = false;
      rb.simulated = false;
      Vector3 subScale = visibilityRadius.transform.localScale;
      subScale *= visibilityScaleFactor;
      visibilityRadius.transform.localScale = subScale;
      // transform.position = submarine.transform.position;
      transform.SetParent(submarine.transform);
      transform.localPosition = Vector2.zero;
      submarineRb.constraints = RigidbodyConstraints2D.FreezeRotation;
      Debug.Log("Entering sub");
      inventory.RemoveArtifacts();
    } else {
      playerSprite.enabled = true;
      rb.simulated = true;
      Vector3 subScale = visibilityRadius.transform.localScale;
      subScale /= visibilityScaleFactor;
      visibilityRadius.transform.localScale = subScale;
      transform.SetParent(null);
      transform.position = submarine.transform.position + new Vector3(0, -1, -1);
      Debug.Log("Leaving sub");
      submarineRb.constraints = RigidbodyConstraints2D.FreezeAll;
      submarineRb.velocity = Vector2.zero;
    }
  }
  private void SubmarineLeaveLevel(InputAction.CallbackContext context) {
    if (isInSubmarine) {
      areYouSureScreen.SetActive(true);
      // SummaryScreen.GetComponent<SummaryScreenUI>().SetSummary();
      summaryScreenUI.SetSummary();

    }

  }
  #endregion
  #region Damage Color Change
  // FF7A7A - Red-ish color for damage color change
  private void ResetColor() {
    playerSprite.color = Color.white;
  }
  private void ChangeColor() {
    playerSprite.color = Color.red;
    Invoke("ResetColor", 0.3f);
  }
  #endregion
}
