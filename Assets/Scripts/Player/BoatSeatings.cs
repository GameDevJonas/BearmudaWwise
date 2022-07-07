using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSeatings : MonoBehaviour
{
    public BoatSeating[] seatings;
    public int seatingsTaken;

    public void AddPerson(Transform person)
    {
        if (seatingsTaken == seatings.Length) return;

        person.GetComponent<Collider>().enabled = false;
        seatings[seatingsTaken].savedPerson = person;
        person.SetParent(seatings[seatingsTaken].seat);
        person.localRotation = Quaternion.identity;
        person.localPosition = Vector3.zero;
        seatingsTaken++;
    }

}

[System.Serializable]
public class BoatSeating
{
    public Transform seat;
    public Transform savedPerson;
}
