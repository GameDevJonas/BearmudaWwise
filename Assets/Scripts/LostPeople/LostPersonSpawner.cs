using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostPersonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject lostPersonPrefab;
    [SerializeField] private Transform lostPersonParent;
    [SerializeField] private int amount;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <= amount; i++)
        {
            SphereCollider collider = GetComponent<SphereCollider>();
            Vector3 spawnPoint = new(Random.Range(collider.bounds.min.x, collider.bounds.max.x), transform.position.y, Random.Range(collider.bounds.min.z, collider.bounds.max.z));
            GameObject lostPerson = Instantiate(lostPersonPrefab, spawnPoint, Quaternion.identity, lostPersonParent);
            lostPerson.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(lostPerson.transform.position);
        }
    }
}
