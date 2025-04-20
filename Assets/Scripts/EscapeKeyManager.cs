using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeKeyCancel : MonoBehaviour
{
    private PlayerInputActions controls;
    private InputAction escape;

    #region Setup Functions
    private void Awake()
    {
        controls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        escape = controls.UI.Escape;
        escape.Enable();
        escape.performed += OnCancel;
    }

    private void OnDisable()
    {
        escape.Disable();
        escape.performed -= OnCancel;
    }

    private void OnCancel(InputAction.CallbackContext contex)
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}
