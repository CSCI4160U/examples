using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    [SerializeField] Transform[] waypoints;
    [SerializeField] float closeEnoughDistance = 1f;
    [SerializeField] bool loop = false;

    private int waypointIndex = 0;
    private bool patrolling = true;

    private NavMeshAgent agent = null;
    private Animator animator = null;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.SetDestination(waypoints[waypointIndex].position);
    }

    void Update() {
        if (!patrolling) {
            return;
        }

        if (animator != null && animator.GetBool("Dead") == true) {
            agent.enabled = false;
        }

        float distanceToTarget = Vector3.Distance(agent.transform.position,
                                                  waypoints[waypointIndex].position);
        if (distanceToTarget < closeEnoughDistance) {
            // go to the next waypoint
            waypointIndex++;

            if (waypointIndex >= waypoints.Length) {
                if (loop) {
                    waypointIndex = 0;
                } else {
                    patrolling = false;
                    animator.SetFloat("Speed", 0);
                    return;
                }
            }

            agent.SetDestination(waypoints[waypointIndex].position);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
