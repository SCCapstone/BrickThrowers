// Copyright 2025 Brick Throwers
// kjthao
// Opens the node menu.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeMenu : MonoBehaviour {
  /// <summary>
  /// Loads the scene based on the node ID.
  /// </summary>
  /// <param name="nodeId"></param>
  public void OpenNode(int nodeId) {
    string nodeName = "Node " + nodeId;
    SceneManager.LoadScene(nodeName);
  }
}
