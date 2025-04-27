// Copyright 2025 Brick Throwers
// PlayerUI.cs - Manages the player UI, including oxygen and stamina bars.
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
  public Slider oxygenBar;
  public Slider staminaBar;
  private Player player;

  public Color normalColor = Color.green;
  public Color lowColor = Color.red;
  public float lowThreshold;

  public float smoothSpeed = 5f;

  [SerializeField] private GameObject staminaWarning;
  [SerializeField] private GameObject oxygenWarning;
  private CanvasGroup cgStamina;
  private CanvasGroup cgHealth;
  [SerializeField] private float flashSpeed = 2f;

  void Start() {
    player = FindObjectOfType<Player>();
    oxygenBar.maxValue = player.maxOxygen;
    staminaBar.maxValue = player.maxStamina;

    lowThreshold = player.lowStaminaThreshold;

    oxygenBar.fillRect.GetComponent<Image>().color = normalColor;
    staminaBar.fillRect.GetComponent<Image>().color = normalColor;
    cgStamina = staminaWarning.GetComponent<CanvasGroup>();
    cgHealth = oxygenWarning.GetComponent<CanvasGroup>();

  }

  void Update() {
    OxygenBarUpdate();
    StaminaBarUpdate();

  }

  private void OxygenBarUpdate() {
    float currentOxygen = player.GetOxygenLevel();
    oxygenBar.value = Mathf.Lerp(oxygenBar.value, currentOxygen, smoothSpeed * Time.deltaTime);

    if (currentOxygen <= lowThreshold) {
      oxygenBar.fillRect.GetComponent<Image>().color = Color.Lerp(
          oxygenBar.fillRect.GetComponent<Image>().color,
          lowColor,
          smoothSpeed * Time.deltaTime
      );
    } else {
      oxygenBar.fillRect.GetComponent<Image>().color = Color.Lerp(
          oxygenBar.fillRect.GetComponent<Image>().color,
          normalColor,
          smoothSpeed * Time.deltaTime
      );
    }
    FadeWarning(currentOxygen, cgHealth);
  }

  private void StaminaBarUpdate() {
    float currentStamina = player.GetStaminaLevel();
    staminaBar.value = Mathf.Lerp(
        staminaBar.value,
        currentStamina,
        smoothSpeed * Time.deltaTime
    );

    if (currentStamina <= lowThreshold) {
      staminaBar.fillRect.GetComponent<Image>().color = Color.Lerp(
          staminaBar.fillRect.GetComponent<Image>().color,
          lowColor,
          smoothSpeed * Time.deltaTime
      );
    } else {
      staminaBar.fillRect.GetComponent<Image>().color = Color.Lerp(
          staminaBar.fillRect.GetComponent<Image>().color,
          normalColor,
          smoothSpeed * Time.deltaTime
      );
    }
    FadeWarning(currentStamina, cgStamina);

  }
  private void FadeWarning(float value, CanvasGroup cg) {
    if (value <= lowThreshold)
      cg.alpha = Mathf.PingPong(Time.time * flashSpeed, 1f);
    else
      cg.alpha = 0f;
  }
}
