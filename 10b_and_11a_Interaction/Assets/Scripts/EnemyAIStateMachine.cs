using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    Patrolling,
    Alerted,
    TargetVisible,
    Dead
}

public class EnemyAIStateMachine : MonoBehaviour {
    [SerializeField] private EnemyState currentState = EnemyState.Patrolling;

    [SerializeField] private Transform[] patrolWaypoints;
    [SerializeField] private int patrolWaypointIndex = 0;
    [SerializeField] private bool patrolLoop;

    [SerializeField] private float closeEnoughDistance;

    private float lastAlertTime = 0.0f;
    [SerializeField] private float alertCooldown = 8.0f;

    private float lastShootTime = 0.0f;
    [SerializeField] private float shootCooldown = 1.0f;

    [SerializeField] private Transform target = null;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    public Vector3 lastKnownTargetPosition;

    private void Start() {
        currentState = EnemyState.Patrolling;

        animator = GetComponent<Animator>();
        animator.speed = 0.5f;

        agent = GetComponent<NavMeshAgent>();

        if ((agent != null) && (patrolWaypoints.Length > 0)) {
            agent.SetDestination(patrolWaypoints[patrolWaypointIndex].position);
        }
    }

    public EnemyState GetState() {
        return currentState;
    }

    public void SetState(EnemyState newState) {
        if (currentState == newState) {
            return;
        }

        currentState = newState;

        if (newState == EnemyState.Patrolling) {
            // re-enable the patrolling behaviour
            agent.enabled = true;
            patrolWaypointIndex = 0;
        } else if (newState == EnemyState.Alerted) {
            // investigate last known position
            agent.enabled = true;
            agent.SetDestination(lastKnownTargetPosition);
            lastAlertTime = Time.time;
        } else if (newState == EnemyState.TargetVisible) {
            // stop and fire at the player
            agent.enabled = false;
            animator.SetFloat("Speed", 0.0f);
            lastKnownTargetPosition = target.transform.position;
        } else {
            // disable all movement, show death animation
            agent.enabled = false;
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Dead", true);
        }
    }

    private void Update() {
        if (currentState == EnemyState.Dead) {
            return; // do nothing
        } else if (currentState == EnemyState.Patrolling) {
            // continue toward next waypoint
            Patrol();
        } else if (currentState == EnemyState.Alerted) {
            // if it has been a while, timeout back to patrol state from alert
            if (Time.time > (lastAlertTime + alertCooldown)) {
                SetState(EnemyState.Patrolling);
                Patrol();
            } else {
                // continue toward last known position
                Alert();
            }
        } else if (currentState == EnemyState.TargetVisible) {
            // shoot target
            Shoot();
        }
    }

    private void Patrol() {
        Vector3 destination = patrolWaypoints[patrolWaypointIndex].position;
        float distanceToTarget = Vector3.Distance(agent.transform.position, destination);

        if (distanceToTarget < closeEnoughDistance) {
            // make the next waypoint active
            patrolWaypointIndex++;

            // loop, if desired
            if (patrolWaypointIndex >= patrolWaypoints.Length) {
                if (patrolLoop) {
                    patrolWaypointIndex = 0;
                } else {
                    animator.SetFloat("Speed", 0);
                    return;
                }
            }

            // navigate to the waypoint
            agent.SetDestination(patrolWaypoints[patrolWaypointIndex].position);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Alert() {
        float distanceToTarget = Vector3.Distance(agent.transform.position, lastKnownTargetPosition);

        if (distanceToTarget < closeEnoughDistance) {
            // stay here and look around
            animator.SetFloat("Speed", 0.0f);

            // TODO:  Add a look around animation, and play that here

        } else {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void Shoot() {
        Vector3 targetDirection = (target.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);

        if (Time.time > (lastShootTime + shootCooldown)) {
            lastShootTime = Time.time;
            animator.SetTrigger("Shoot");

            // TODO:  shooting itself (we've done this before, so we won't do it again)
        }
    }
}
