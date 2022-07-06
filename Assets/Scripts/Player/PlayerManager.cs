using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum ActivePlayerState { GroundPlayer, BoatPlayer};
    public ActivePlayerState PlayerState;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class GroundPlayerVariables
{

}

[System.Serializable]
public class BoatPlayerVariables
{

}
