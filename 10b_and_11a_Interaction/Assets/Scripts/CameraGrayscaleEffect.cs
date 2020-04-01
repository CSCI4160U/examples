using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class CameraGrayscaleEffect : MonoBehaviour {
    [SerializeField] [Range(0, 1)] private float effectAmount = 1.0f;
    [SerializeField] private bool audoFade = true;
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private Shader shader = null;

    private Material material = null;

    private void Update() {
        // implement the auto fade
        if (audoFade && effectAmount > 0.0f) {
            effectAmount -= fadeSpeed * Time.deltaTime;
            effectAmount = Mathf.Max(effectAmount, 0.0f);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (shader == null) {
            return;
        }

        if (material == null) {
            material = new Material(shader);
        }

        // pass in the effect amount parameter
        material.SetFloat("_EffectAmount", effectAmount);

        // copy the image data, from the material output
        Graphics.Blit(source, destination, material);
    }
}
