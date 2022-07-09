using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    public enum ControlsSchemes { gamepad, keyboard};
    public static ControlsSchemes currentControlScheme;

    [SerializeField] private ControlsSchemes ActiveScheme;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        switch (playerInput.currentControlScheme)
        {
            case "Gamepad":
                currentControlScheme = ControlsSchemes.gamepad;
                break;
            case "Keyboard":
                currentControlScheme = ControlsSchemes.keyboard;
                break;
        }
        ActiveScheme = currentControlScheme;
    }
}
