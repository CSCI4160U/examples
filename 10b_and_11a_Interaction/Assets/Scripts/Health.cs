using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour {
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float health = 100f;
    public bool isDead = false;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Start() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float amount) {
        health -= amount;

        if (health <= 0) {
            health = 0;

            isDead = true;

            if (animator != null) {
                animator.SetBool("Dead", true);
                animator.SetBool("Attacking", false);
                animator.SetFloat("Speed", 0.0f);
            }

            if (agent != null) {
                agent.enabled = false;
            }
        }
    }
}
