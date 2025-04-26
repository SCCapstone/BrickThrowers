// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeMenu : MonoBehaviour {
  public void OpenNode(int nodeId) {
    string nodeName = "Node " + nodeId;
    SceneManager.LoadScene(nodeName);
  }
}
