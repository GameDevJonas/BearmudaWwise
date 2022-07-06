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

    private void Awake()
    {
        inputActions = new PlayerInputs();
        agent = GetComponent<NavMeshAgent>();
        mainCam = Camera.main.transform;
    }

    private void OnEnable()
    {
        movement = inputActions.Player.Movement;
        movement.Enable();

        inputActions.Player.Interact.performed += DoInteractButton;
        inputActions.Player.Interact.Enable();
    }

    private void DoInteractButton(InputAction.CallbackContext obj)
    {
        PlayerManager.InteractPushed.Invoke(this.gameObject);

    }

    private void FixedUpdate()
    {
        //if (movement.ReadValue<Vector2>() != Vector2.zero) Debug.Log("Reading movement values: " + movement.ReadValue<Vector2>());
        
    }

    private void Update()
    {
        Vector2 inputAxis = movement.ReadValue<Vector2>();
        Vector3 desiredInput = new Vector3(inputAxis.x * 2, 0, inputAxis.y * 2);

        var forward = mainCam.forward;
        //forward.y = 0;
        //forward.Normalize();
        var right = mainCam.right;
        //right.y = 0;
        //right.Normalize();

        navObject.localPosition = new Vector3(desiredInput.x, 0, desiredInput.z);
        navObject.parent.rotation = Quaternion.LookRotation(forward);

        agent.SetDestination(navObject.position);
    }

    private void OnDisable()
    {
        movement.Disable();
        inputActions.Player.Interact.Disable();
    }
}
