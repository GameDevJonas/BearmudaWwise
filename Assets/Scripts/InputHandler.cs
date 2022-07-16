using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    public enum ControlsSchemes { gamepad, keyboard };
    public static ControlsSchemes currentControlScheme;

    [SerializeField] private ControlsSchemes ActiveScheme;

    private bool isPaused;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        PauseMenu.pauseGame.AddListener(PauseGame);
        PauseMenu.unpauseGame.AddListener(UnpauseGame);
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) isPaused = true;
        CheckForPause();
    }

    private void CheckForPause()
    {
        switch (ActiveScheme)
        {
            case ControlsSchemes.gamepad:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case ControlsSchemes.keyboard:
                if (isPaused)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                break;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
    }

    public void UnpauseGame()
    {
        isPaused = false;
    }
}
