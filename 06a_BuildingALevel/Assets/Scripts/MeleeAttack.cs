using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float alertDistance = 10.0f;
    [SerializeField] private float attackDistance = 1.5f;

    private Animator animator = null;
    private NavMeshAgent agent = null;

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
            // we are in range, attack!
            agent.enabled = false; // don't walk
            animator.SetFloat("Speed", 0.0f);
            animator.SetInteger("AttackNum", 1);
            animator.SetTrigger("Attacking");

            // look toward the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.1f);
        } else if (direction.magnitude < alertDistance) {
            // navigate toward the target
            agent.enabled = true;
            agent.SetDestination(target.position);
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
}
