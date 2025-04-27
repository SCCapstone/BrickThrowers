// Copyright 2025 Brick Throwers
// Submarine.cs - Handles the button prompt for entering the submarine.
// Note: You would think that this script would have the submarine logic, but eh...
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour {
  [SerializeField] private GameObject buttonPrompt;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.CompareTag("Player")) {
      buttonPrompt.SetActive(true);
    }
  }
  private void OnTriggerExit2D(Collider2D collision) {
    if (collision.gameObject.CompareTag("Player")) {
      buttonPrompt.SetActive(false);
    }
  }
}
