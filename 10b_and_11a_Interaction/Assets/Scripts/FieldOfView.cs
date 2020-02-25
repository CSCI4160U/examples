using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour {
    public Transform eye;
    [Range(0, 360)] public float fovAngle;
    public float visionRadius;

    public bool isAlerted = false;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask wallLayer;

    private EnemyAIStateMachine stateMachine = null;

    public void OnDrawGizmos() {
        if (stateMachine == null) {
            return;
        }

        FieldOfView fov = GetComponent<FieldOfView>();

        Gizmos.matrix = Matrix4x4.identity;

        if (stateMachine.GetState() == EnemyState.TargetVisible) {
            Gizmos.color = Color.red;
        } else if (stateMachine.GetState() == EnemyState.Alerted) {
            Gizmos.color = Color.yellow;
        } else {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(fov.eye.position, 0.2f);
        Vector3 focusPos = fov.eye.position + fov.eye.forward * visionRadius;
        focusPos.y += 1.5f;
        Gizmos.DrawLine(fov.eye.position, focusPos);

        Gizmos.color = Color.black;
        float step = 0.1f;
        float radius = 10.0f;
        for (float theta = 0; theta < 6.28f; theta += step) {
            Vector3 direction = fov.eye.up * radius * Mathf.Sin(theta) +
                                fov.eye.right * radius * Mathf.Cos(theta) +
                                fov.eye.forward * visionRadius;
            Gizmos.DrawLine(fov.eye.position, fov.eye.position + direction * visionRadius);
        }
    }

    private void Start() {
        stateMachine = GetComponent<EnemyAIStateMachine>();
        StartCoroutine("KeepSearchingForTargets", 0.25f);
    }

    private void LookForTargets() {
        bool canSeeTarget = false;
        // get all targets within range of our vision
        Collider[] targets = Physics.OverlapSphere(eye.position, visionRadius, targetLayer);
        for (int i = 0; i < targets.Length; i++) {
            // determine if the target is out of our field of view
            Vector3 targetPos = targets[i].transform.position;
            targetPos.y += 1.5f;
            Vector3 targetDirection = (targetPos - eye.position).normalized;
            if (Vector3.Angle(eye.forward, targetDirection) < fovAngle / 2) {
                // finally, determine if there is line of sight
                float distance = Vector3.Distance(eye.position, targets[i].transform.position);
                if (!Physics.Raycast(eye.position, targetDirection, distance, wallLayer)) {
                    // we have line of sight (no walls in the way)
                    canSeeTarget = true;
                    isAlerted = true;
                    if (stateMachine.GetState() != EnemyState.TargetVisible) {
                        Debug.Log("I can see you! state: " + stateMachine.GetState());
                        stateMachine.SetState(EnemyState.TargetVisible);
                    }
                }
            }
        }

        if (!canSeeTarget && isAlerted) {
            stateMachine.SetState(EnemyState.Alerted);
            isAlerted = false;
        }
    }

    IEnumerator KeepSearchingForTargets(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            LookForTargets();
        }
    }
}
