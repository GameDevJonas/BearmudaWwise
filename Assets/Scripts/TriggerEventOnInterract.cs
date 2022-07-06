using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnInterract : MonoBehaviour
{
    public UnityEvent OnInteract;

    private List<GameObject> _objectsInsideCollider = new List<GameObject>();

    private void Awake()
    {
        PlayerManager.InteractPushed.AddListener(OnInteractMe);
    }

    private void OnInteractMe(GameObject whereFrom)
    {
        if (_objectsInsideCollider.Contains(whereFrom)) OnInteract.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        _objectsInsideCollider.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        _objectsInsideCollider.Remove(other.gameObject);
    }
}
