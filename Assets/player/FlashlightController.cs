using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 100f;
    private bool isOn = true;
    public GameObject flashlight;

    // Start is called before the first frame update
    void Start()
    {
        flashlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RotateFlashlight();
        if (Input.GetKeyDown(KeyCode.F)) {
          ToggleFlashlight();
        }
    }

    void RotateFlashlight() {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector3 direction = mousePos - player.transform.position;
    direction.z = 0;

    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    transform.rotation = Quaternion.Euler(0,0,angle);
  }

  void ToggleFlashlight() {
    isOn = !isOn;
    flashlight.SetActive(isOn);
  }
}
