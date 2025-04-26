using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Copyright 2025 Scott Do
/// Retains the means of saving and loading currency JSON data.
/// </summary>
public class CurrencyManager : MonoBehaviour {
  private static Currency playerCurrency = new Currency();
  private static IDataService dataService = new JsonDataService();
  private const string CURRENCY_PATH = "currency.json";
  [SerializeField] private TextMeshProUGUI currencyText;

  private void Start() {
    if (!LoadJsonCurrency()) {
      Debug.Log("Failed to load currency data, creating new one.");
      playerCurrency = new Currency();
      SerializeJson();
    }
    UpdateCurrencyText();
  }

  private void SerializeJson() {
    if (dataService.SaveData(CURRENCY_PATH, playerCurrency)) {
      Debug.Log("Data saved successfully");
    } else {
      Debug.Log("Data failed to save");
    }
  }

  private bool LoadJsonCurrency() {
    try {
      playerCurrency = dataService.LoadData<Currency>(CURRENCY_PATH);
      return true;

    } catch (Exception e) {
      Debug.LogError($"Could not load file! {e.Message} {e.StackTrace}");
      return false;
    }
  }


  /// <summary>
  /// Returns reference type of Currency.
  /// </summary>
  /// <returns></returns>
  public Currency ReturnCurrency() {
    return playerCurrency;
  }

  /// <summary>
  /// Updates the currency text in scene.
  /// </summary>
  /// <param name="amount"></param>
  /// <returns>True if operation success, false otherwise.</returns>
  public bool UpdateCurrency(int amount) {
    playerCurrency.CurrencyAmount += amount;
    UpdateCurrencyText();
    SerializeJson();
    return true;
  }

  private void UpdateCurrencyText() {
    currencyText.text = "Coins: " + playerCurrency.CurrencyAmount.ToString();
  }
}
