using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostPeopleArrival : MonoBehaviour
{
    [SerializeField] private float onBoardDelay;

    private BoatSeatings _seatings;

    [SerializeField] private Transform arrivalPoint;
    [SerializeField] private Transform parentObject;

    private Transform _player;

    private int placedPeople = 0;

    private PlayerFollowers _playerFollowers;

    private void Awake()
    {
        _seatings = FindObjectOfType<BoatSeatings>();
        _player = GameObject.Find("Player").transform;
        _playerFollowers = _player.GetComponent<PlayerFollowers>();
    }

    void Start()
    {
        PlayerManager.SwitchedToPlayer.AddListener(StartOnBoard);
    }

    private void StartOnBoard() //Check if boat has people in it, and then resets if it is empty
    {
        if (_seatings.seatingsTaken > 0)
        {
            Invoke("PlaceNextPerson", onBoardDelay);
        }
        else
        {
            foreach (BoatSeating savedPerson in _seatings.seatings)
            {
                if (savedPerson != null) savedPerson.savedPerson = null;
            }
            placedPeople = 0;
        }
    }

    private void PlaceNextPerson()
    {
        if (_seatings.seatingsTaken <= 0)
        {
            _seatings.seatingsTaken = 0;
            return;
        }

        //Gets the next person from boat, sets new parent and resets rotation
        Transform nextPerson = _seatings.seatings[_seatings.seatingsTaken - 1].savedPerson;
        nextPerson.SetParent(parentObject);
        nextPerson.rotation = Quaternion.identity;

        //Place next person on island navmesh and set their agent destination to player or next in line
        nextPerson.GetComponent<LostPerson>().ArriveOnLand(arrivalPoint.position);
        if (_playerFollowers.CurrentFollowers.Count == 0)
        {
            nextPerson.GetComponent<LostPerson>().SetNewTarget(_player);
        }
        else
        {
            nextPerson.GetComponent<LostPerson>().SetNewTarget(_playerFollowers.CurrentFollowers[^1]);
        }
        _playerFollowers.CurrentFollowers.Add(nextPerson);

        _seatings.seatingsTaken--;

        StartOnBoard(); //Continue onboard process until done
    }
}
