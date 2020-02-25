using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotator : MonoBehaviour {
    private float rotation = 0.1f;

    void Update() {
        transform.Rotate(new Vector3(0.0f, rotation, 0.0f));
    }
}
