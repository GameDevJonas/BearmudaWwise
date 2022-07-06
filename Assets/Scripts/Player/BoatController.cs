using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    //Input system
    private PlayerInputs inputActions;
    private InputAction movement;

    //Movement variables
    [SerializeField] private float maxSpeed;
    private Vector3 moveDirection;
    private float movementSpeed;
    private Rigidbody rb;

    //Acceleration variables
    [SerializeField] private float accelerationValue;
    private bool isAccelerating, isDecelerating;
    private float accelTimer;
    private float startAccelValue;
    private float fakeX, fakeY;

    //Rotation variables
    private Transform mainCam;
    [SerializeField] private Transform lookAtPoint;

    //NOT BELONG HERE JUST TEST
    private Animator anim;
    private bool isTiltLeft, isTiltRight;

    private void Awake()
    {
        inputActions = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        isAccelerating = false;
        isDecelerating = true;
        accelTimer = 0;
        //lookAtPoint.SetParent(null);
    }

    private void OnEnable()
    {
        movement = inputActions.Boat.Movement;

        movement.started += StartMovement;
        movement.canceled += StopMovement;

        movement.Enable();

        inputActions.Boat.Interact.performed += DoInteractButton;
        inputActions.Boat.Interact.Enable();

        inputActions.Boat.TiltLeft.started += TiltLeft;
        inputActions.Boat.TiltLeft.canceled += TiltLeft;

        inputActions.Boat.TiltLeft.Enable();

        inputActions.Boat.TiltRight.started += TiltRight;
        inputActions.Boat.TiltRight.canceled += TiltRight;

        inputActions.Boat.TiltRight.Enable();
    }

    private void DoInteractButton(InputAction.CallbackContext obj)
    {
        PlayerManager.InteractPushed.Invoke(this.gameObject);
    }

    private void TiltLeft(InputAction.CallbackContext obj)
    {
        if(!isTiltLeft && !isTiltRight)
        {
            isTiltLeft = true;
            anim.SetBool("TiltLeft", isTiltLeft);
        }
        else if(isTiltLeft && !isTiltRight)
        {
            isTiltLeft = false;
            anim.SetBool("TiltLeft", isTiltLeft);
        }
    }

    private void TiltRight(InputAction.CallbackContext obj)
    {
        if (!isTiltRight && !isTiltLeft)
        {
            isTiltRight = true;
            anim.SetBool("TiltRight", isTiltRight);
        }
        else if (isTiltRight && !isTiltLeft)
        {
            isTiltRight = false;
            anim.SetBool("TiltRight", isTiltRight);
        }
    }

    private void StartMovement(InputAction.CallbackContext obj)
    {
        startAccelValue = movementSpeed;
        isDecelerating = false;
        isAccelerating = true;
        accelTimer = 0;
    }

    private void StopMovement(InputAction.CallbackContext obj)
    {
        startAccelValue = movementSpeed;
        isAccelerating = false;
        isDecelerating = true;
        accelTimer = 0;
    }

    private void FixedUpdate()
    {
        var forward = mainCam.forward;
        forward.y = 0;
        forward.Normalize();

        var right = mainCam.right;
        right.y = 0;
        right.Normalize();

        Vector3 desiredMoveDir = new Vector3(moveDirection.x, 0, moveDirection.z);
        //rb.velocity = (forward * desiredMoveDir.z) + (right * desiredMoveDir.x);

        //Vector3 desiredMoveDir = new Vector3(moveDirection.x, 0, moveDirection.z);
        rb.velocity = new Vector3(desiredMoveDir.x, 0, desiredMoveDir.z);
    }

    private void Update()
    {
        MovementSpeedCalc();
        AccelerationCalc();
        if (movement.ReadValue<Vector2>() != Vector2.zero) RotateWithMovement();
    }

    private void AccelerationCalc()
    {
        if (isAccelerating && (movementSpeed <= maxSpeed))
        {
            movementSpeed = Mathf.Lerp(startAccelValue, maxSpeed, accelTimer / 1);
            accelTimer += Time.deltaTime;
        }
        else if (isDecelerating && (movementSpeed >= 0))
        {
            movementSpeed = Mathf.Lerp(startAccelValue, 0, accelTimer / 1);
            accelTimer += Time.deltaTime;
        }
    }

    private void MovementSpeedCalc()
    {
        Vector2 inputAxis = movement.ReadValue<Vector2>();

        if (isAccelerating)
        {
            fakeX = inputAxis.x;
            fakeY = inputAxis.y;
        }

        if (!isDecelerating) moveDirection = new Vector3(inputAxis.x * movementSpeed, 0, inputAxis.y * movementSpeed) + mainCam.forward;
        else moveDirection = new Vector2(fakeX * movementSpeed, fakeY * movementSpeed);
    }

    private void RotateWithMovement()
    {
        Vector3 desiredMoveDir = new Vector3(moveDirection.x, 0, moveDirection.z);

        var forward = mainCam.forward;
        forward.y = 0;
        forward.Normalize();

        var right = mainCam.right;
        right.y = 0;
        right.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((forward * desiredMoveDir.z) + (right * desiredMoveDir.x)), Time.deltaTime * 5f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDir), Time.deltaTime * 40f);
    }

    private void OnDisable()
    {
        movement.Disable();
        inputActions.Boat.Interact.Disable();
        inputActions.Boat.TiltLeft.Disable();
        inputActions.Boat.TiltRight.Disable();
    }
}
