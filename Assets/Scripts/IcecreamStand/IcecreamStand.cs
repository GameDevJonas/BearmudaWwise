using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IcecreamStand : MonoBehaviour
{
    private PlayerInputs playerInput;

    private PlayerManager manager;

    [SerializeField] private bool DoFinishCustomer;

    [SerializeField] private Transform icebearPosition;

    private PlayerFollowers _playerFollowers;

    [SerializeField] private Transform firstInLinePoint;
    [SerializeField] private Transform waitingPositionParent;
    [SerializeField] private Collider islandCollider;

    [SerializeField] private List<Transform> peopleInLine = new List<Transform>();
    [SerializeField] private List<Transform> waitingPositions = new List<Transform>();


    private bool _activeLine, _activeStand;

    [SerializeField] private GameObject[] icecreams;

    private void Awake()
    {
        _playerFollowers = FindObjectOfType<PlayerFollowers>();
        manager = FindObjectOfType<PlayerManager>();
        playerInput = new PlayerInputs();
        playerInput.Player.Interact.performed += Interact;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        FinishCustomer();
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
                    lostPerson.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 7;
                    peopleInLine.Add(lostPerson);
                }
            }
            else
            {
                peopleInLine.AddRange(_playerFollowers.CurrentFollowers);
                foreach(Transform lostP in _playerFollowers.CurrentFollowers)
                {
                    lostP.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 7;
                }
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
        peopleInLine[0].GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 0;
    }

    public void FinishCustomer()
    {
        if (peopleInLine.Count < 1)
        {
            _activeStand = false;
            manager.SwitchPlayerState(PlayerManager.ActivePlayerState.GroundPlayer);
            playerInput.Player.Interact.Disable();
            Debug.LogError("No people waiting in line");
            return;
        }

        if (!_activeStand)
        {
            EnterIcecreamStand();
            return;
        }

        _playerFollowers.GetComponentInChildren<Animator>().ResetTrigger("DoHappy");
        _playerFollowers.GetComponentInChildren<Animator>().SetTrigger("DoHappy");

        GameObject newPos = new(peopleInLine[0].name + "'s New WaitPosition");
        newPos.transform.SetParent(waitingPositionParent);
        newPos.transform.position = FindPositionOnTerrain();
        waitingPositions.Add(newPos.transform);
        LostPerson nextPersonInLine = peopleInLine[0].GetComponent<LostPerson>();
        nextPersonInLine.SetNewTarget(waitingPositions[^1]);
        nextPersonInLine.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 4;
        nextPersonInLine.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 3;

        GameObject newIceCream = Instantiate(icecreams[Random.Range(0, icecreams.Length)], nextPersonInLine.transform.GetChild(0).GetChild(0));
        nextPersonInLine.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        nextPersonInLine.GetIceCream();

        GetComponent<PostWwiseEvent>().PostEvent();

        peopleInLine.RemoveAt(0);
        NextPersonInLine();
    }

    public void EnterIcecreamStand()
    {
        _activeStand = true;
        playerInput.Player.Interact.Enable();
        manager.SwitchPlayerState(PlayerManager.ActivePlayerState.IcecreamStand);
        PlaceIcebear();
        //Invoke(nameof(PlaceIcebear), .5f);
    }

    private void PlaceIcebear()
    {
        _playerFollowers.transform.position = icebearPosition.position;
        _playerFollowers.transform.rotation = icebearPosition.rotation;
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
