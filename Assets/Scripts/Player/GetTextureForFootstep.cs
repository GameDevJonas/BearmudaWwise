using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AK.Wwise;

public class GetTextureForFootstep : MonoBehaviour
{
    [SerializeField] private LayerMask FloorLayer;
    [SerializeField] private TextureSound[] TextureSounds;
    [SerializeField] private NavMeshAgent Agent;

    [SerializeField] private int RTPCValue;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Agent.velocity != Vector3.zero &&
            Physics.Raycast(transform.position - new Vector3(0,.5f,0),
            Vector3.down,
            out RaycastHit hit,
            1f,
            FloorLayer))
        {
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
            {
                GetTerrainTexture(terrain, hit.point);
            }
            else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
            {
                GetRendererTexture(renderer);
            }
        }

        Debug.DrawRay(transform.position - new Vector3(0, .5f, 0), Vector3.down, Color.red);
    }

    private void GetTerrainTexture(Terrain Terrain, Vector3 HitPoint)
    {
        Vector3 terrainPosition = HitPoint - Terrain.transform.position;
        Vector3 splatMapPosition = new Vector3(terrainPosition.x / Terrain.terrainData.size.x, 0, terrainPosition.z / Terrain.terrainData.size.z);

        int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
        int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

        float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

        int primaryIndex = 0;
        for (int i = 1; i < alphaMap.Length; i++)
        {
            if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
            {
                primaryIndex = i;
            }
        }

        foreach (TextureSound textureSound in TextureSounds)
        {
            foreach (Texture texture in textureSound.Albedos)
            {
                if (texture == Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
                {
                    RTPCValue = textureSound.FootstepID;
                    AkSoundEngine.SetRTPCValue("RTPC_Player_Footsteps", RTPCValue);
                }
            }
        }
    }

    private void GetRendererTexture(Renderer Renderer)
    {
        foreach (TextureSound textureSound in TextureSounds)
        {
            foreach (Texture texture in textureSound.Albedos)
            {
                if (texture == Renderer.material.GetTexture("_MainTex"))
                {
                    RTPCValue = textureSound.FootstepID;
                    AkSoundEngine.SetRTPCValue("RTPC_Player_Footsteps", RTPCValue);
                }
            }
        }
    }

    [System.Serializable]
    public class TextureSound
    {
        public Texture[] Albedos;
        public int FootstepID;
    }
}

