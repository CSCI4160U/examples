using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float alertDistance = 12.0f;
    [SerializeField] private float attackDistance = 1.5f;

    private Animator animator;
    private NavMeshAgent agent;

    void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (animator.GetBool("Dead")) {
            return;
        }

        Vector3 direction = target.position - transform.position;
        direction.y = 0.0f;

        if (direction.magnitude < attackDistance) {
            // we are within range, but are not attacking
            // start the attack
            agent.enabled = false; // don't walk
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("Attacking", true);
            animator.SetInteger("AttackNum", Random.Range(1, 6));

            // we have previously started attacking
            // turn toward the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.1f);
        } else if (direction.magnitude < alertDistance) {
            // we are not close enough to attack, but are close enough to detect
            // navigate toward the target
            agent.enabled = true; // walk toward destination
            agent.SetDestination(target.position);
            animator.SetFloat("Speed", agent.velocity.magnitude);
            animator.SetBool("Attacking", false);
        }
    }
}
