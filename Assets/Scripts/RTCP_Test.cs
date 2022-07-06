using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

[ExecuteInEditMode]
public class RTCP_Test : MonoBehaviour
{
    [Range(1, 5)] public float FootstepRTPC;
    //public GameObject gameObjectRTPC;


    private void OnValidate()
    {
        AkSoundEngine.SetRTPCValue("RTPC_Player_Footsteps", FootstepRTPC);
    }
}
