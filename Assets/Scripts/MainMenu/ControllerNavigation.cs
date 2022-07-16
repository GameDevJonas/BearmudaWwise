using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using AK.Wwise;

public class ControllerNavigation : MonoBehaviour
{
    [SerializeField] private Selectable[] selectables;
    [SerializeField] private int activeSelectable;

    private PlayerInputs inputActions;
    private InputAction navigate, adjust;

    private EventSystem eventSystem;

    [SerializeField] private AK.Wwise.Event changeButtonSound, submitSound;

    private void Awake()
    {
        inputActions = new();
    }

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        navigate = inputActions.UI.Navigate;
        navigate.performed += Navigate_performed;
        navigate.Enable();

        adjust = inputActions.UI.Adjust;
        adjust.performed += Adjust_performed;
        adjust.Enable();

        inputActions.UI.Submit.performed += Submit_performed;
        inputActions.UI.Submit.Enable();

        activeSelectable = 0;
        ActiveSelectableVisual();
    }

    private void Submit_performed(InputAction.CallbackContext obj)
    {
        if(eventSystem.currentSelectedGameObject == null)
        {
            ActiveSelectableVisual();
        }
    }

    private void Adjust_performed(InputAction.CallbackContext obj)
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            ActiveSelectableVisual();
        }
    }

    private void Navigate_performed(InputAction.CallbackContext obj)
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            ActiveSelectableVisual();
        }
    }

    private void ActiveSelectableVisual()
    {
        selectables[activeSelectable].Select();
    }

    private void OnDisable()
    {
        navigate.Disable();
        adjust.Disable();
        inputActions.UI.Submit.Enable();
    }

}
