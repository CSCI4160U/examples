using System.Collections;
using UnityEngine;

public class BloodSpatterEffect : MonoBehaviour {
    [SerializeField] private Texture2D bloodTexture = null;
    [SerializeField] private Texture2D bloodNormalMap = null;
    [SerializeField] [Range(0.0f, 1.0f)] private float bloodAmount = 0.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float minBloodAmount = 0.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float distortion = 0.0f;
    [SerializeField] private bool autoFade = false;
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private Shader shader = null;

    private Material material = null;

    private void Update() {
        // implement the auto fade
        if (autoFade && bloodAmount > 0.0f) {
            bloodAmount -= fadeSpeed * Time.deltaTime;
            bloodAmount = Mathf.Max(bloodAmount, minBloodAmount);

            distortion = bloodAmount * 0.1f;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (shader == null) {
            return;
        }

        if (material == null) {
            material = new Material(shader);
        }

        if (bloodTexture != null) {
            material.SetTexture("_BloodTex", bloodTexture);
        }

        if (bloodNormalMap != null) {
            material.SetTexture("_BloodBump", bloodNormalMap);
        }

        material.SetFloat("_Distortion", distortion);
        material.SetFloat("_BloodAmount", bloodAmount);

        Graphics.Blit(source, destination, material);
    }
}
