using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using Cinemachine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public enum ActivePlayerState { GroundPlayer, BoatPlayer };
    public static ActivePlayerState PlayerState;

    [SerializeField] private GroundPlayerVariables GroundPlayerVariables;
    [SerializeField] private BoatPlayerVariables BoatPlayerVariables;

    public static UnityEvent<GameObject> InteractPushed = new();

    private void Start()
    {
        SwitchPlayerState(ActivePlayerState.GroundPlayer);
    }

    [ContextMenu("Change to boat")]
    public void ChangeToBoatInspector()
    {
        SwitchPlayerState(ActivePlayerState.BoatPlayer);
    }

    [ContextMenu("Change to ground")]
    public void ChangeToGroundInspector()
    {
        SwitchPlayerState(ActivePlayerState.GroundPlayer);
    }

    public void SwitchPlayerState()
    {
        switch (PlayerState)
        {
            case ActivePlayerState.GroundPlayer:
                TurnOnOffBoatVariables(true);
                break;
            case ActivePlayerState.BoatPlayer:
                TurnOnOffGroundVariables(true);
                break;
        }
    }

    public void SwitchPlayerState(ActivePlayerState playerState)
    {
        switch (playerState)
        {
            case ActivePlayerState.GroundPlayer:
                TurnOnOffGroundVariables(true);
                break;
            case ActivePlayerState.BoatPlayer:
                TurnOnOffBoatVariables(true);
                break;
        }
    }

    private void TurnOnOffGroundVariables(bool turnOn)
    {
        switch (turnOn)
        {
            case true:
                TurnOnOffBoatVariables(false);
                PlayerState = ActivePlayerState.GroundPlayer;
                //Debug.Log("Ground mode on");

                GroundPlayerVariables.Controller.enabled = turnOn;
                GroundPlayerVariables.Visuals.SetActive(turnOn);
                GroundPlayerVariables.CinemachineFreeLook.Priority = 11;

                break;
            case false:
                //Debug.Log("Ground mode off");

                GroundPlayerVariables.Controller.enabled = turnOn;
                GroundPlayerVariables.Visuals.SetActive(turnOn);
                GroundPlayerVariables.CinemachineFreeLook.Priority = 9;
                break;
        }
    }

    private void TurnOnOffBoatVariables(bool turnOn)
    {
        switch (turnOn)
        {
            case true:
                TurnOnOffGroundVariables(false);
                PlayerState = ActivePlayerState.BoatPlayer;
                //Debug.Log("Boat mode on");

                BoatPlayerVariables.BoatController.enabled = turnOn;
                BoatPlayerVariables.Visuals.SetActive(turnOn);
                BoatPlayerVariables.CinemachineFreeLook.Priority = 11;
                break;
            case false:
                //Debug.Log("Boat mode off");

                BoatPlayerVariables.BoatController.enabled = turnOn;
                BoatPlayerVariables.Visuals.SetActive(turnOn);
                BoatPlayerVariables.CinemachineFreeLook.Priority = 9;
                break;
        }
    }
}

[System.Serializable]
public class GroundPlayerVariables
{
    public PlayerController Controller;
    public GameObject Visuals;
    public CinemachineFreeLook CinemachineFreeLook;
}

[System.Serializable]
public class BoatPlayerVariables
{
    public BoatController BoatController;
    public GameObject Visuals;
    public CinemachineFreeLook CinemachineFreeLook;
}
