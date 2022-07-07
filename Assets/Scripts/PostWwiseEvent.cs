using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class PostWwiseEvent : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event Event;
    [SerializeField] private bool PlayOnStart;

    private void Start()
    {
        if (PlayOnStart) PostEvent();
    }

    [ContextMenu("Post Event")]
    public void PostEvent()
    {
        AkSoundEngine.PostEvent(Event.Id, gameObject);
    }

    [ContextMenu("Stop Event")]
    public void StopEvent()
    {
        AkSoundEngine.StopAll(gameObject);
    }
}
