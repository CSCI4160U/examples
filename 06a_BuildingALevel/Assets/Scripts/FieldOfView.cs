using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public Transform eye;
    [Range(0, 360)] public float fovAngle = 60f;
    public float visionRadius = 10f;

    public bool isAlerted = false;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float targetScanDelay = 0.25f;

    public void OnDrawGizmos() {
        Gizmos.matrix = Matrix4x4.identity;

        // draw colour by state

        Vector3 focusPos = eye.position + eye.forward * visionRadius;
        focusPos.y = eye.position.y;

        Gizmos.DrawWireSphere(eye.position, 0.2f);
        Gizmos.DrawWireSphere(eye.position, visionRadius);
        Gizmos.DrawLine(eye.position, focusPos);
    }

    private void Start() {
        StartCoroutine("KeepSearchingForTargets", targetScanDelay);
        //StartCoroutine(KeepSearchingForTargets(targetScanDelay)); // equivalent
    }

    IEnumerator KeepSearchingForTargets(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            LookForTargets();
        }
    }

    private void LookForTargets() {
        // 1. any objects within our vision radius
        // 2. is the object within fov?
        // 3. do we have line of sight?
        Debug.Log("LookForTargets()");
    }


}
