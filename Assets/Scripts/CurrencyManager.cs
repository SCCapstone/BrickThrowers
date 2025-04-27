using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Copyright 2025 Scott Do
// Retains the means of saving and loading currency JSON data.
public class CurrencyManager : MonoBehaviour {

  // If you want to cheat with money, path is:
  // C:\Users\<your-username>\AppData\LocalLow\Brick Throwers\Daredivers\currency.json

  // Singleton instance
  private static Currency playerCurrency = new Currency();
  private static IDataService dataService = new JsonDataService();
  private const string CURRENCY_PATH = "currency.json";
  [SerializeField] private TextMeshProUGUI currencyText;

  #region Setup
  private void Start() {
    if (!LoadJsonCurrency()) {
      Debug.Log("Failed to load currency data, creating new one.");
      playerCurrency = new Currency();
      SerializeJson();
    }
    UpdateCurrencyText();
  }
  private void OnEnable() {
    CurrencyTransporter.transportMoneyNow += GotMoneyFromGame;
  }
  private void OnDisable() {
    CurrencyTransporter.transportMoneyNow -= GotMoneyFromGame;
  }
  #endregion
  #region JSON Serialization
  /// <summary>
  /// Serializes the currency data to JSON format and saves it to a file.
  /// </summary>
  private void SerializeJson() {
    if (dataService.SaveData(CURRENCY_PATH, playerCurrency)) {
      Debug.Log("Data saved successfully");
    } else {
      Debug.Log("Data failed to save");
    }
  }
  /// <summary>
  /// Loads the currency data from a JSON file.
  /// </summary>
  /// <returns>True if operation success, false otherwise.</returns>
  private bool LoadJsonCurrency() {
    try {
      playerCurrency = dataService.LoadData<Currency>(CURRENCY_PATH);
      return true;

    } catch (Exception e) {
      Debug.LogError($"Could not load file! {e.Message} {e.StackTrace}");
      return false;
    }
  }
  #endregion

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

  /// <summary>
  /// Recieves the signal from CurrencyTransporter to update the currency.
  /// </summary>
  /// <param name="money"></param>
  public void GotMoneyFromGame(int money) {
    Debug.Log($"currency manager got your money: {money}");
    UpdateCurrency(money);
    Debug.Log($"the money is now {playerCurrency.CurrencyAmount}");
  }

  private void UpdateCurrencyText() {
    currencyText.text = "Coins: " + playerCurrency.CurrencyAmount.ToString();
  }
}
