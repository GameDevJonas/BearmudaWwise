using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AK.Wwise;

public class BoatController : MonoBehaviour
{
    //Input system
    
    private PlayerInputs inputActions;
    private InputAction movement;
    private InputAction tilt;

    //Movement variables
    [Header("Movement variables")]
    [SerializeField] private float maxSpeed;
    private Vector3 moveDirection;
    private float movementSpeed;
    private Rigidbody rb;

    //Acceleration variables
    [Header("Acceleration variables")]
    [SerializeField] private float accelerationValue;
    private bool isAccelerating, isDecelerating;
    private float accelTimer;
    private float startAccelValue;
    private float fakeX, fakeY;

    //Rotation variables
    [Header("Rotation variables")]
    private Transform mainCam;
    [SerializeField] private Transform lookAtPoint;

    //Tilt variables
    [Header("Tilt Variables")]
    private Animator anim;
    [SerializeField] private CheckForCollidersInsideTrigger leftCollectTrigger, rightCollectTrigger;
    [SerializeField] private AK.Wwise.Event TiltEventStart, TiltEventStop;

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
        leftCollectTrigger.gameObject.SetActive(false);
        rightCollectTrigger.gameObject.SetActive(false);
        //lookAtPoint.SetParent(null);
    }

    private void OnEnable()
    {
        movement = inputActions.Boat.Movement;

        movement.started += StartMovement;
        movement.canceled += StopMovement;

        movement.Enable();

        tilt = inputActions.Boat.Tilt;

        tilt.started += DoTilt;
        tilt.canceled += DoTilt;

        tilt.Enable();

        inputActions.Boat.Interact.performed += DoInteractButton;
        inputActions.Boat.Interact.Enable();

        inputActions.Boat.TiltRight.Enable();
    }

    private void DoInteractButton(InputAction.CallbackContext obj)
    {
        PlayerManager.InteractPushed.Invoke(this.gameObject);
    }

    private void StartMovement(InputAction.CallbackContext obj)
    {
        startAccelValue = movementSpeed;
        isDecelerating = false;
        isAccelerating = true;
        accelTimer = 0;
    }

    private void DoTilt(InputAction.CallbackContext obj)
    {
        int inputValue = (int)tilt.ReadValue<float>();

        anim.SetInteger("TiltValue", inputValue);

        if(inputValue < 0) //left tilt
        {
            leftCollectTrigger.gameObject.SetActive(true);
            AkSoundEngine.PostEvent(TiltEventStart.Id, gameObject);
            
        }
        else if(inputValue > 0) // right tilt
        {
            rightCollectTrigger.gameObject.SetActive(true);
            AkSoundEngine.PostEvent(TiltEventStart.Id, gameObject);
        }
        else //centered
        {
            AkSoundEngine.PostEvent(TiltEventStop.Id, gameObject);
            if(leftCollectTrigger.CollidersInsideMe.Count > 0)
            {
                HoistPerson(leftCollectTrigger.CollidersInsideMe[0]);
            }
            else if(rightCollectTrigger.CollidersInsideMe.Count > 0)
            {
                HoistPerson(rightCollectTrigger.CollidersInsideMe[0]);
            }

            leftCollectTrigger.gameObject.SetActive(false);
            rightCollectTrigger.gameObject.SetActive(false);
        }
    }

    private void HoistPerson(Collider hoistie)
    {
        Debug.Log(hoistie.name + "Was hoisted");
        GetComponent<BoatSeatings>().AddPerson(hoistie.transform);
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
        rb.velocity = (forward * desiredMoveDir.z) + (right * desiredMoveDir.x);

        //Vector3 desiredMoveDir = new Vector3(moveDirection.x, 0, moveDirection.z);
        //rb.velocity = new Vector3(desiredMoveDir.x, 0, desiredMoveDir.z);
    }

    private void Update()
    {
        MovementSpeedCalc();
        AccelerationCalc();
        if (movement.ReadValue<Vector2>() != Vector2.zero) RotateWithMovement();

        AkSoundEngine.SetRTPCValue("RTPC_Boat_Speed", movementSpeed);
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
        else moveDirection = new Vector3(fakeX * movementSpeed, 0, fakeY * movementSpeed);
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
        tilt.Disable();
        inputActions.Boat.Interact.Disable();
        inputActions.Boat.TiltLeft.Disable();
        inputActions.Boat.TiltRight.Disable();
        rb.velocity = Vector3.zero;
    }
}
