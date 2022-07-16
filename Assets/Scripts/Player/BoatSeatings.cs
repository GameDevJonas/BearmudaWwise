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

        person.GetChild(1).gameObject.SetActive(false);
        person.GetComponent<Collider>().enabled = false;
        seatings[seatingsTaken].savedPerson = person;
        person.SetParent(seatings[seatingsTaken].seat);
        person.localRotation = Quaternion.identity;
        person.localPosition = Vector3.zero;
        Transform icebear = transform.GetChild(0).Find("Icebear (1)");
        person.LookAt(icebear.position);
        person.localRotation = Quaternion.Euler(new Vector3(0,person.localRotation.eulerAngles.y, 0));
        seatingsTaken++;
    }

}

[System.Serializable]
public class BoatSeating
{
    public Transform seat;
    public Transform savedPerson;
}
