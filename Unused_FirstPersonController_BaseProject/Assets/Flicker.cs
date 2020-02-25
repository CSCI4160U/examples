using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] private float maxIntensity = 10.0f;

    private Light light;

    void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Start() {
        light.type = LightType.Point;
        light.shadows = LightShadows.Soft;
        light.range = 15f;
    }
    void Update()
    {
        light.intensity = Mathf.PerlinNoise(Time.time, 0) * maxIntensity;
    }
}
