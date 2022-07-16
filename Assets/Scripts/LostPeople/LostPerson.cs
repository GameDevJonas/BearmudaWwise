using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LostPerson : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool inWater = true;
    [SerializeField] private bool canMove = true;
    [SerializeField] private GameObject[] models;
    [SerializeField] private Transform myHouse;

    private NavMeshAgent agent;
    private Transform boatPlayer;

    private bool needsHouse;
    private PlayerController groundPlayer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        boatPlayer = FindObjectOfType<BoatController>().transform.GetChild(0).Find("Icebear (1)");
        groundPlayer = FindObjectOfType<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Transform prevChild = transform.GetChild(0).GetChild(1);
        GameObject randomAvatar = models[Random.Range(0, models.Length)];
        Instantiate(randomAvatar, prevChild.position, prevChild.rotation, transform.GetChild(0));
        Destroy(prevChild.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (inWater)
        {
            transform.LookAt(boatPlayer.position);
            transform.localRotation = Quaternion.Euler(new Vector3(0, transform.localRotation.eulerAngles.y, 0));
        }
        if (!inWater && canMove && target) UpdateDestination();
    }

    private void UpdateDestination()
    {
        agent.SetDestination(target.position);
    }

    public void ArriveOnLand(Vector3 newPosition)
    {
        inWater = false;
        GetComponent<Rigidbody>().isKinematic = false;
        agent.enabled = true;
        agent.Warp(newPosition);
        canMove = true;
    }

    [ContextMenu("Test warp")]
    public void WarpHere()
    {
        agent.Warp(transform.position);
    }

    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void GetIceCream()
    {
        needsHouse = true;
    }

    public void GetHouse(Transform house)
    {
        myHouse = house;
        needsHouse = false;
        SetNewTarget(house);
    }

    [ContextMenu("Reset navmesh values")]
    public void ResetNavMeshThings()
    {
        agent.Warp(transform.position);
        agent.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && needsHouse)
        {
            groundPlayer.GetNearestLostPerson(this);
        }
    }
}
