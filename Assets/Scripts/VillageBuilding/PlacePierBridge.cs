using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

public class PlacePierBridge : MonoBehaviour
{
    private BuilderController Controller;
    private Spline Spline;
    private GameObject tempSpline;

    public GameObject pierPrefab;
    [SerializeField] private Transform startPoint, endPoint;
    [SerializeField] private Transform pierParent;

    private bool updateEndPos, activePlacement;



    private void Awake()
    {
        Controller = GetComponent<BuilderController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Controller.ConfirmEvent.AddListener(ConfirmPress);
        Controller.CancelEvent.AddListener(CancelPress);

        activePlacement = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateEndPos) UpdateEndPositionPoint();

        if (!updateEndPos)
        {
            Vector3 placementPosition = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);
            startPoint.position = placementPosition;
        }

        if (activePlacement) startPoint.GetChild(0).gameObject.SetActive(!updateEndPos);
    }

    public void ConfirmPress()
    {
        if (!updateEndPos) PlacePierStart();
        else if (updateEndPos) PlacePierEnd();
    }

    public void CancelPress()
    {
        if (!updateEndPos) Controller.EndBuilding();
        if (updateEndPos) PlacePierCancel();
    }

    [ContextMenu("Test positions of pier")]
    public void UpdateSplinePositions()
    {
        Spline.nodes[0].Position = startPoint.localPosition;
        Spline.nodes[^1].Position = endPoint.localPosition;
    }

    private void PlacePierStart()
    {
        Vector3 instantiatePos = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);
        tempSpline = Instantiate(pierPrefab, instantiatePos, Quaternion.identity, pierParent);
        tempSpline.SetActive(true);
        Spline = tempSpline.GetComponent<Spline>();
        startPoint.SetParent(tempSpline.transform);
        endPoint.SetParent(tempSpline.transform);

        endPoint.position = startPoint.position;
        Spline.nodes[0].Position = startPoint.localPosition;
        updateEndPos = true;
    }

    private void UpdateEndPositionPoint()
    {
        Vector3 placementPosition = new Vector3(transform.position.x, startPoint.position.y, transform.position.z);
        endPoint.position = placementPosition;
        Spline.nodes[^1].Position = endPoint.localPosition;
    }

    private void PlacePierEnd()
    {
        updateEndPos = false;
        tempSpline = null;
        startPoint.SetParent(null);
        endPoint.SetParent(null);
        Spline = null;
    }

    private void PlacePierCancel()
    {
        activePlacement = false;
        updateEndPos = false;
        Destroy(tempSpline);
        tempSpline = null;
        startPoint.SetParent(null);
        endPoint.SetParent(null);
        Spline = null;
        startPoint.GetChild(0).gameObject.SetActive(false);
    }
}
