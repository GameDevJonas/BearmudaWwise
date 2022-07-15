using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePierBridge : MonoBehaviour
{
    [SerializeField] private List<Transform> pierStartPoints = new List<Transform>();
    private List<float> distances = new List<float>();

    [SerializeField] private float pierPointRadius;

    [SerializeField] private Transform houseRef;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PierStartPos"))
        {
            pierStartPoints.Add(obj.transform);
        }
    }

    public void PlaceHouse(Transform newHouse)
    {
        houseRef = newHouse;
        LookForNearestStartPoint();
    }

    private void LookForNearestStartPoint()
    {
        Transform closestPoint = null;
        float smallestDistance = 1000000000000;
        foreach(Transform transform in pierStartPoints)
        {
            float distance = Vector3.Distance(houseRef.position, transform.position);
            if (distance < smallestDistance)
            {
                closestPoint = transform;
                smallestDistance = distance;
            }
        }
        if (closestPoint == null)
        {
            Debug.LogError("No close points"); 
            return;
        }

        Debug.Log("Closest point is: " + closestPoint.name + ", with a distance of: " + smallestDistance, closestPoint.gameObject);
        LookForNearestEndPoint(closestPoint);
    }

    private void LookForNearestEndPoint(Transform startPos)
    {
        Transform closestPoint = null;
        float smallestDistance = 1000000000000;
        foreach (Transform transform in houseRef.GetChild(1))
        {
            float distance = Vector3.Distance(startPos.position, transform.position);
            if (distance < smallestDistance)
            {
                closestPoint = transform;
                smallestDistance = distance;
            }
        }
        if (closestPoint == null)
        {
            Debug.LogError("No close points");
            return;
        }

        Debug.Log("Full path is from: " + startPos.name + " to: " + closestPoint.name, closestPoint.gameObject);

        GetComponent<PlacePierBridge>().PlacePier(startPos, closestPoint, houseRef);
    }
}
