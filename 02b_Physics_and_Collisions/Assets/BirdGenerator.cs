using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGenerator : MonoBehaviour {
    [Header("Generation Region")]
    public float minimumX = -4.0f;
    public float maximumX = 4.0f;
    public float minimumY = 12.0f;
    public float maximumY = 16.0f;

    [Header("Generation Params")]
    public GameObject birdPrefab;
    public int numBirdToGenerate = 20;

    [ContextMenu("Generate")]
    void Generate() {
        for (var i = 0; i < numBirdToGenerate; i++) {
            // find the bird's position
            float x = Random.Range(minimumX, maximumX);
            float y = Random.Range(minimumY, maximumY);
            Vector3 birdPosition = new Vector3(x, y, 0.0f);

            // instantiate the bird
            GameObject newBird = Instantiate(birdPrefab, birdPosition, Quaternion.identity);

            // re-parent the bird to the current object
            newBird.transform.SetParent(transform);
        }
    }
}
