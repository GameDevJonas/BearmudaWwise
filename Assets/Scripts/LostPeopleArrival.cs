using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostPeopleArrival : MonoBehaviour
{
    [SerializeField] private float onBoardDelay;

    [SerializeField] private BoatSeatings seatings;

    [SerializeField] private Transform arrivalPoint;
    [SerializeField] private Transform player;

    private int placedPeople = 0;

    private void Awake()
    {
        seatings = FindObjectOfType<BoatSeatings>();
        player = GameObject.Find("Player").transform;
    }

    void Start()
    {
        PlayerManager.SwitchedToPlayer.AddListener(StartOnBoard);
    }

    void Update()
    {

    }

    private void StartOnBoard()
    {
        if (seatings.seatingsTaken > 0)
        {
            PlaceNextPerson();
        }
        else
        {
            foreach (BoatSeating savedPerson in seatings.seatings)
            {
                if (savedPerson != null) savedPerson.savedPerson = null;
            }
            placedPeople = 0;
        }
    }

    private void PlaceNextPerson()
    {
        if (seatings.seatingsTaken <= 0)
        {
            seatings.seatingsTaken = 0;
            return;
        }

        Transform nextPerson = seatings.seatings[seatings.seatingsTaken - 1].savedPerson;
        nextPerson.SetParent(null);
        nextPerson.rotation = Quaternion.identity;
        //nextPerson.position = arrivalPoint.position;

        nextPerson.GetComponent<LostPerson>().ArriveOnLand(arrivalPoint.position);
        if (placedPeople == 0)
        {
            nextPerson.GetComponent<LostPerson>().ChangeTarget(player);
            
        }
        else nextPerson.GetComponent<LostPerson>().ChangeTarget(seatings.seatings[seatings.seatingsTaken].savedPerson);

        //seatings.seatings[seatings.seatingsTaken].savedPerson = null;
        seatings.seatingsTaken--;
        placedPeople++;

        Invoke("StartOnBoard", onBoardDelay);
    }
}
