using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    Patrolling, Alerted, TargetVisible, Dead
}

public class EnemyAIStateMachine : MonoBehaviour {
    [SerializeField] private EnemyState currentState = EnemyState.Patrolling;

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int waypointIndex = 0;
    [SerializeField] private bool patrolLoop = true;
    [SerializeField] private float closeEnoughDistance;

    [SerializeField] private float lastAlertTime = 0.0f;
    [SerializeField] private float alertCooldown = 8.0f;
    [SerializeField] private Vector3 lastKnownTargetPosition;

    [SerializeField] private float lastShootTime = 0.0f;
    [SerializeField] private float shootCooldown = 1.0f;
    [SerializeField] private Transform target = null;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Start() {
        currentState = EnemyState.Patrolling;

        animator = GetComponent<Animator>();
        animator.speed = 0.5f;

        agent = GetComponent<NavMeshAgent>();

        if (agent != null && waypoints.Length > 0) {
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }

    public EnemyState GetState() {
        return currentState;
    }

    public void SetState(EnemyState newState) {
        if (currentState == newState)
            return;

        currentState = newState;

        if (newState == EnemyState.Patrolling) {
            // resume patrol
            agent.enabled = true;
            waypointIndex = 0;
        } else if (newState == EnemyState.Alerted) {
            // investigate the last known position
            agent.enabled = true;
            agent.SetDestination(lastKnownTargetPosition);

            // remember when we were alerted
            lastAlertTime = Time.time;
        } else if (newState == EnemyState.TargetVisible) {
            // shoot at the player
            agent.enabled = false;
            animator.SetFloat("Speed", 0.0f);
            lastKnownTargetPosition = target.transform.position;
        } else if (newState == EnemyState.Dead) {
            // disable navigation and play death animation
            agent.enabled = false;
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Dead", true);
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
    }

    private void Update() {
        if (currentState == EnemyState.Dead) {
            return;
        } else if (currentState == EnemyState.Patrolling) {
            Patrol();
        } else if (currentState == EnemyState.Alerted) {
            // check for timeout
            if (Time.time > (lastAlertTime + alertCooldown)) {
                SetState(EnemyState.Patrolling);
                Patrol();
            } else {
                Alert();
            }
        } else if (currentState == EnemyState.TargetVisible) {
            Shoot();
        }
    }

    private void Patrol() {
        Vector3 destination = waypoints[waypointIndex].position;
        float distanceToTarget = Vector3.Distance(agent.transform.position, destination);

        if (distanceToTarget < closeEnoughDistance) {
            waypointIndex++;

            // loop, if desired
            if (waypointIndex >= waypoints.Length) {
                if (patrolLoop) {
                    waypointIndex = 0;
                } else {
                    animator.SetFloat("Speed", 0);
                    return;
                }
            }

            agent.SetDestination(waypoints[waypointIndex].position);
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Alert() {
        float distanceToTarget = Vector3.Distance(agent.transform.position, lastKnownTargetPosition);

        if (distanceToTarget < closeEnoughDistance) {
            // stay here
            animator.SetFloat("Speed", 0.0f);

            // TODO:  Look around animation
        } else {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void Shoot() {
        if (Time.time > (lastShootTime + shootCooldown)) {
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);

            lastShootTime = Time.time;
            animator.SetTrigger("Shoot");

            // TODO:  Do raycast to calculate damage
        }

    }
}
