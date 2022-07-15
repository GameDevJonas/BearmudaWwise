using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs inputActions;
    private InputAction movement;

    [SerializeField] private Transform navObject;
    [SerializeField] private NavMeshAgent agent;

    private Transform mainCam;

    private Animator anim;

    [SerializeField] private LostPerson lostPersonVillageHouse;

    private PlayerManager manager;

    private void Awake()
    {
        inputActions = new PlayerInputs();
        agent = GetComponent<NavMeshAgent>();
        mainCam = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        manager = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        movement = inputActions.Player.Movement;
        movement.Enable();

        inputActions.Player.Interact.performed += DoInteractButton;
        inputActions.Player.Interact.Enable();

        lostPersonVillageHouse = null;
    }

    private void DoInteractButton(InputAction.CallbackContext obj)
    {
        PlayerManager.InteractPushed.Invoke(this.gameObject);
        if(lostPersonVillageHouse != null)
        {
            FindObjectOfType<PlaceVillageHouse>().lostPerson = lostPersonVillageHouse;
            manager.SwitchPlayerState(PlayerManager.ActivePlayerState.VillageBuilder);
        }
    }

    public void GetNearestLostPerson(LostPerson person)
    {
        lostPersonVillageHouse = person;
    }

    private void Update()
    {
        Vector2 inputAxis = movement.ReadValue<Vector2>();
        Vector3 desiredInput = new Vector3(inputAxis.x * 2, 0, inputAxis.y * 2);

        var forward = mainCam.forward;
        var right = mainCam.right;

        navObject.localPosition = new Vector3(desiredInput.x, 0, desiredInput.z);
        navObject.parent.rotation = Quaternion.LookRotation(forward);

        agent.SetDestination(navObject.position);

        anim.SetBool("IsWalking", movement.ReadValue<Vector2>() != Vector2.zero);
    }

    private void OnDisable()
    {
        lostPersonVillageHouse = null;
        movement.Disable();
        inputActions.Player.Interact.Disable();
    }
}
