using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;
using System;

public class PlaceVillageHouse : MonoBehaviour
{
    private BuilderController Controller;

    [SerializeField] private GameObject housePrefab;
    [SerializeField] private Transform houseParent;
    [SerializeField] private float houseYHeight;
    [SerializeField] private float rotationValue;

    private GameObject housePlacementRef;
    private float rotDirection;
    private bool doRotate;

    private void Awake()
    {
        Controller = GetComponent<BuilderController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Controller.ConfirmEvent.AddListener(ConfirmPress);
        Controller.CancelEvent.AddListener(CancelPress);
        Controller.RotateEvent.AddListener(RotateHouse);
        Controller.StopRotate.AddListener(StopRotate);
    }


    private void OnEnable()
    {
        housePlacementRef = Instantiate(housePrefab, transform.position, Quaternion.identity, houseParent);
    }

    void Update()
    {
        if (housePlacementRef != null) UpdateHousePosition();

        if(doRotate) housePlacementRef.transform.Rotate(new Vector3(0, rotDirection * rotationValue * Time.deltaTime, 0));
    }

    private void UpdateHousePosition()
    {
        Vector3 placementPosition = new Vector3(transform.position.x, houseYHeight, transform.position.z);
        housePlacementRef.transform.position = placementPosition;
    }

    private void RotateHouse(float rotValue)
    {
        rotDirection = rotValue;
        doRotate = true;
    }

    private void StopRotate()
    {
        rotDirection = 0;
        doRotate = false;
    }

    public void ConfirmPress()
    {
        PlaceHouse();
    }

    private void PlaceHouse()
    {
        //Generate pier bridge
        GeneratePierBridge generatePierBridge = GetComponent<GeneratePierBridge>();
        generatePierBridge.PlaceHouse(housePlacementRef.transform);

        //Turn on own pier start points

        housePlacementRef = null;
        housePlacementRef = Instantiate(housePrefab, transform.position, Quaternion.identity, houseParent);
        //Controller.EndBuilding();
        //this.enabled = false;
    }

    public void CancelPress()
    {
        Destroy(housePlacementRef);
        housePlacementRef = null;
        Controller.EndBuilding();
        this.enabled = false;
    }
}
