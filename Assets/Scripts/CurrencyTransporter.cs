using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copyright 2025 Brick Throwers CSCE-492
 * Transport money from the game scenes into the lobby scene and add to the current currency.
 */
public class CurrencyTransporter : MonoBehaviour
{
  // Money to transport
  private static int moneyFromGame;

  private void OnEnable() {
    SummaryScreenUI.transferCurrency += AssignCurrency;
  }
  private void OnDisable() {
    SummaryScreenUI.transferCurrency -= AssignCurrency;
  }

  private void AssignCurrency(int value) {
    moneyFromGame = value;
  }
}
