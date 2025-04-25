using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

  public float TimeLeft;
  public bool TimerOn = false;

  public Text TimerText;

  public GameObject SummaryScreen;

  // Start is called before the first frame update
  void Start() {
    TimerOn = true;

  }

  // Update is called once per frame
  void Update() {



    if (TimerOn) {
      if (TimeLeft > 0) {
        TimeLeft -= Time.deltaTime;
        UpdateTime(TimeLeft);
      } else {
        Debug.Log("Time is UP!");
        TimeLeft = 0;
        TimerOn = false;

        if (TimerOn == false) {
          SummaryScreen.SetActive(true);
        }
      }
    }

  }

  void UpdateTime(float currentTime) {
    currentTime += 1;

    float minutes = Mathf.FloorToInt(currentTime / 60);
    float seconds = Mathf.FloorToInt(currentTime % 60);

    TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
  }

}
