using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuilderController : MonoBehaviour
{
    private PlayerInputs inputActions;
    private InputAction movement, rotate;

    private Rigidbody rb;
    public bool canMove;

    [SerializeField] private float movementSpeed;

    private void Awake()
    {
        inputActions = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        movement = inputActions.VillagePlacement.Movement;
        movement.Enable();

        rotate = inputActions.VillagePlacement.Rotate;
        rotate.Enable();

        inputActions.VillagePlacement.Confirm.performed += Confirm;
        inputActions.VillagePlacement.Confirm.Enable();

        inputActions.VillagePlacement.Cancel.performed += Cancel;
        inputActions.VillagePlacement.Cancel.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        rotate.Disable();
        inputActions.VillagePlacement.Confirm.Disable();
        inputActions.VillagePlacement.Cancel.Disable();
    }

    private void Cancel(InputAction.CallbackContext obj)
    {

    }

    private void Confirm(InputAction.CallbackContext obj)
    {

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (canMove) ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector2 inputAxis = movement.ReadValue<Vector2>();
        rb.velocity = new Vector3(inputAxis.x * movementSpeed, 0, inputAxis.y * movementSpeed);
    }
}
