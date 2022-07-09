using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LostPerson : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool inWater = true;
    [SerializeField] private bool canMove = true;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!inWater && canMove && target) UpdateDestination();
    }

    private void UpdateDestination()
    {
        agent.SetDestination(target.position);
    }

    public void ArriveOnLand(Vector3 newPosition)
    {
        inWater = false;
        canMove = true;
        GetComponent<Rigidbody>().isKinematic = false;
        agent.Warp(newPosition);
    }

    public void SetNewTarget(Transform newTarget)
    {
        target = newTarget;
    }

    [ContextMenu("Reset navmesh values")]
    public void ResetNavMeshThings()
    {
        agent.Warp(transform.position);
        agent.SetDestination(target.position);
    }
}
