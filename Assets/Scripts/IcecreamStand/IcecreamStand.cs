using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcecreamStand : MonoBehaviour
{
    [SerializeField] private bool DoFinishCustomer;

    private PlayerFollowers _playerFollowers;

    [SerializeField] private Transform firstInLinePoint;
    [SerializeField] private Transform waitingPositionParent;
    [SerializeField] private Collider islandCollider;

    [SerializeField] private List<Transform> peopleInLine = new List<Transform>();
    [SerializeField] private List<Transform> waitingPositions = new List<Transform>();


    private bool _activeLine;

    [SerializeField] private GameObject[] icecreams;

    private void Awake()
    {
        _playerFollowers = FindObjectOfType<PlayerFollowers>();
    }

    private void OnValidate()
    {
        if (DoFinishCustomer)
        {
            if (peopleInLine.Count > 0)
            {
                FinishCustomer();
            }
            else
            {
                Debug.LogError("No people waiting in line");
            }
            DoFinishCustomer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _playerFollowers.CurrentFollowers.Count > 0)
        {
            if (_activeLine)
            {
                foreach (Transform lostPerson in _playerFollowers.CurrentFollowers)
                {
                    lostPerson.GetComponent<LostPerson>().SetNewTarget(peopleInLine[^1]);
                    peopleInLine.Add(lostPerson);
                }
            }
            else
            {
                peopleInLine.AddRange(_playerFollowers.CurrentFollowers);
            }

            _activeLine = true;
            _playerFollowers.CurrentFollowers.Clear();
            NextPersonInLine();
        }
    }

    private void NextPersonInLine()
    {
        if (peopleInLine.Count < 1)
        {
            _activeLine = false;
            return;
        }
        peopleInLine[0].GetComponent<LostPerson>().SetNewTarget(firstInLinePoint);
    }

    public void FinishCustomer()
    {
        if (peopleInLine.Count < 1)
        {
            Debug.LogError("No people waiting in line");
            return;
        }

        GameObject newPos = new(peopleInLine[0].name + "'s New WaitPosition");
        newPos.transform.SetParent(waitingPositionParent);
        newPos.transform.position = FindPositionOnTerrain();
        waitingPositions.Add(newPos.transform);
        LostPerson nextPersonInLine = peopleInLine[0].GetComponent<LostPerson>();
        nextPersonInLine.SetNewTarget(waitingPositions[^1]);

        GameObject newIceCream = Instantiate(icecreams[Random.Range(0, icecreams.Length)], nextPersonInLine.transform.GetChild(0).GetChild(0));
        nextPersonInLine.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        peopleInLine.RemoveAt(0);
        NextPersonInLine();
    }

    private Vector3 FindPositionOnTerrain()
    {
        Collider myArea = GetComponent<Collider>();

        Vector3 newRandomPos;

        newRandomPos.x = Random.Range(islandCollider.bounds.min.x, islandCollider.bounds.max.x);
        newRandomPos.y = islandCollider.transform.position.y;
        newRandomPos.z = Random.Range(islandCollider.bounds.min.z, islandCollider.bounds.max.z);

        if (myArea.bounds.Contains(newRandomPos)) return FindPositionOnTerrain();
        else return newRandomPos;
    }
}
