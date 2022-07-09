using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using Cinemachine;
using UnityEngine.Events;
using System;

public class PlayerManager : MonoBehaviour
{
    public enum ActivePlayerState { GroundPlayer, BoatPlayer, VillageBuilder};
    public static ActivePlayerState PlayerState;

    [SerializeField] private ActivePlayerState startingState;

    [SerializeField] private GroundPlayerVariables GroundPlayerVariables;
    [SerializeField] private BoatPlayerVariables BoatPlayerVariables;
    [SerializeField] private VillageBuilderVariables VillageBuilderVariables;

    public static UnityEvent<GameObject> InteractPushed = new();
    public static UnityEvent SwitchedToPlayer = new();
    public static UnityEvent SwitchedToBoat = new();
    public static UnityEvent SwitchedToBuilder = new();

    private void Start()
    {
        SwitchPlayerState(startingState);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    [ContextMenu("Change to builder")]
    public void ChangeToBuilderInspector()
    {
        SwitchPlayerState(ActivePlayerState.VillageBuilder);
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
            case ActivePlayerState.VillageBuilder:
                TurnOnOffBuilderVariables(true);
                break;
        }
    }

    private void TurnOnOffGroundVariables(bool turnOn)
    {
        switch (turnOn)
        {
            case true:
                TurnOnOffBoatVariables(false);
                TurnOnOffBuilderVariables(false);
                SwitchedToPlayer.Invoke();

                AkSoundEngine.SetRTPCValue("RTPC_Ambx_Location", 1);

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
                TurnOnOffBuilderVariables(false);
                SwitchedToBoat.Invoke();

                AkSoundEngine.SetRTPCValue("RTPC_Ambx_Location", 2);

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

    private void TurnOnOffBuilderVariables(bool turnOn)
    {
        switch (turnOn)
        {
            case true:
                TurnOnOffBoatVariables(false);
                TurnOnOffGroundVariables(false);
                SwitchedToBuilder.Invoke();

                AkSoundEngine.SetRTPCValue("RTPC_Ambx_Location", 2);

                PlayerState = ActivePlayerState.VillageBuilder;
                //Debug.Log("Builder mode on");

                VillageBuilderVariables.CinemachineVirtualCamera.Priority = 11;
                break;
            case false:
                //Debug.Log("Builder mode off");
                VillageBuilderVariables.CinemachineVirtualCamera.Priority = 9;
                break;
        }
    }
}

[Serializable]
public class GroundPlayerVariables
{
    public PlayerController Controller;
    public GameObject Visuals;
    public CinemachineFreeLook CinemachineFreeLook;
}

[Serializable]
public class BoatPlayerVariables
{
    public BoatController BoatController;
    public GameObject Visuals;
    public CinemachineFreeLook CinemachineFreeLook;
}

[Serializable]
public class VillageBuilderVariables
{
    public CinemachineVirtualCamera CinemachineVirtualCamera;
}
