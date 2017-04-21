﻿using UnityEngine;
using UnityEngine.UI;

public class SliderAction11 : MonoBehaviour {

    private Slider sliderComp;
    private GameObject[] lights;

    void Start()
    {
        sliderComp = GetComponent<Slider>();
        sliderComp.value = 1;
    }

    public void LightChange()
    {
        lights = GameObject.FindGameObjectsWithTag("CeilingLight");

        for (int i = 0; i < lights.Length; i++)
        {
            Light lightComp = lights[i].GetComponent<Light>();
            lightComp.intensity = (sliderComp.value * 2) + 0.3f;
        }

    }

}
