using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(1, 24)] private float TimeOfDay;
    [SerializeField] private float TimeScale;

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += TimeScale * Time.deltaTime;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

        //Set RTPC value for Wwise
        AkSoundEngine.SetRTPCValue("RTPC_Ambx_Time", TimeOfDay);

        //Changes skybox materials
        ChangeSkyboxMaterial();
    }

    private void ChangeSkyboxMaterial()
    {
        Material currentSkybox = RenderSettings.skybox;

        if (TimeOfDay <= 5 && currentSkybox != Preset.NightMaterial) //First night
        {
            RenderSettings.skybox = Preset.NightMaterial;
        }
        else if (TimeOfDay > 5 && TimeOfDay <= 9 && currentSkybox != Preset.MorningMaterial) //Morning
        {
            RenderSettings.skybox = Preset.MorningMaterial;
        }
        else if (TimeOfDay > 9 && TimeOfDay <= 16.5f && currentSkybox != Preset.DayMaterial) //Day
        {
            RenderSettings.skybox = Preset.DayMaterial;
        }
        else if (TimeOfDay > 16.5f && TimeOfDay <= 20 && currentSkybox != Preset.SunsetMaterial) //Sunset
        {
            RenderSettings.skybox = Preset.SunsetMaterial;
        }
        else if(TimeOfDay > 20 && currentSkybox != Preset.NightMaterial)//Second night
        {
            RenderSettings.skybox = Preset.NightMaterial;
        }
    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }

        //Changes skybox materials
        ChangeSkyboxMaterial();
    }
}