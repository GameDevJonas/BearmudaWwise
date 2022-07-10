using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.AI.Navigation;
using UnityEngine.Events;

public class BuilderController : MonoBehaviour
{
    private PlayerManager manager;

    private PlayerInputs inputActions;
    private InputAction movement, rotate;

    public List<NavMeshSurface> NavMeshSurfaces = new List<NavMeshSurface>();

    private Rigidbody rb;
    public bool canMove;

    [SerializeField] private float movementSpeed;

    [HideInInspector] public UnityEvent ConfirmEvent = new UnityEvent(), CancelEvent = new UnityEvent();

    private void Awake()
    {
        inputActions = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        BuildNavMeshes();
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
        CancelEvent.Invoke();
    }

    private void Confirm(InputAction.CallbackContext obj)
    {
        ConfirmEvent.Invoke();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (canMove) ApplyMovement();
    }

    public void EndBuilding()
    {
        BuildNavMeshes(); //Maybe do an async here
        manager.SwitchPlayerState(PlayerManager.ActivePlayerState.GroundPlayer);
    }

    private void ApplyMovement()
    {
        Vector2 inputAxis = movement.ReadValue<Vector2>();
        rb.velocity = new Vector3(inputAxis.x * movementSpeed, 0, inputAxis.y * movementSpeed);
    }

    [ContextMenu("Bake surfaces")]
    public void BuildNavMeshes()
    {
        foreach (NavMeshSurface surface in NavMeshSurfaces)
        {
            surface.BuildNavMesh();
        }
    }
}
