using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseObject;

    [SerializeField] private CinemachineFreeLook playerCam, boatCam;
    [SerializeField] private SensitivityVars horSensVars, verSensVars;
    [SerializeField] private ToggleVars horToggleVars, verToggleVars;

    private bool isPaused;

    private PlayerInputs inputActions;
    private InputAction navigation, adjust;

    public static UnityEvent pauseGame = new(), unpauseGame = new();

    private PlayerManager manager;
    private PlayerManager.ActivePlayerState prevState;

    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
        inputActions = new PlayerInputs();
        inputActions.UI.PauseButton.performed += PauseGameFromInput;
        inputActions.UI.PauseButton.Enable();
        isPaused = true;
        PauseGame();
    }

    public void PauseGameFromInput(InputAction.CallbackContext obj)
    {
        PauseGame();
    }

    public void PauseGame()
    {
        horSensVars.sliderText.text = horSensVars.slider.value.ToString("F0");
        verSensVars.sliderText.text = verSensVars.slider.value.ToString("F1");

        isPaused = !isPaused;
        pauseObject.SetActive(isPaused);

        switch (isPaused)
        {
            case true:
                prevState = PlayerManager.PlayerState;
                manager.SwitchPlayerState(PlayerManager.ActivePlayerState.MainMenu);
                pauseGame.Invoke();
                EnableInputs();
                Time.timeScale = 0;
                break;
            case false:
                manager.SwitchPlayerState(prevState);
                unpauseGame.Invoke();
                DisableInputs();
                Time.timeScale = 1;
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void EnableInputs()
    {
        GetComponent<ControllerNavigation>().enabled = true;
    }

    private void DisableInputs()
    {
        GetComponent<ControllerNavigation>().enabled = false;
    }

    public void UpdateSensitivty(bool horizontal)
    {
        switch (horizontal)
        {
            case true:
                playerCam.m_XAxis.m_MaxSpeed = horSensVars.slider.value;
                boatCam.m_XAxis.m_MaxSpeed = horSensVars.slider.value;
                horSensVars.sliderText.text = horSensVars.slider.value.ToString("F0");
                break;
            case false:
                playerCam.m_YAxis.m_MaxSpeed = verSensVars.slider.value;
                boatCam.m_YAxis.m_MaxSpeed = verSensVars.slider.value;
                verSensVars.sliderText.text = verSensVars.slider.value.ToString("F1");
                break;
        }
    }

    public void ToggleInvert(bool horizontal)
    {
        switch (horizontal)
        {
            case true:
                playerCam.m_XAxis.m_InvertInput = horToggleVars.toggle.isOn;
                boatCam.m_XAxis.m_InvertInput = horToggleVars.toggle.isOn;
                break;
            case false:
                playerCam.m_YAxis.m_InvertInput = verToggleVars.toggle.isOn;
                boatCam.m_YAxis.m_InvertInput = verToggleVars.toggle.isOn;
                break;
        }
    }
}

[System.Serializable]
public class SensitivityVars
{
    public Slider slider;
    public TextMeshProUGUI sliderText;
}

[System.Serializable]
public class ToggleVars
{
    public Toggle toggle;
}
