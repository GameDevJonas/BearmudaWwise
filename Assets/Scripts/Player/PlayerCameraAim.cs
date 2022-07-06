using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraAim : MonoBehaviour
{
    private Transform target;


    private void Awake()
    {
        target = transform.parent;
        transform.SetParent(null);
        //CinemachineVirtualCamera cam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        //cam.LookAt = transform;
        //cam.Follow = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
    }
}
