using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForCollidersInsideTrigger : MonoBehaviour
{
    public List<Collider> CollidersInsideMe = new List<Collider>();
    public string TagToCheckFor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagToCheckFor)) CollidersInsideMe.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagToCheckFor)) CollidersInsideMe.Remove(other);
    }

    private void OnDisable()
    {
        CollidersInsideMe.Clear();
    }
}
