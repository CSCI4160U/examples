using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float health = 100f;
    [SerializeField] bool isDead = false;

    private Animator animator = null;
    [SerializeField] NavMeshAgent agent = null;

    public Ragdoller ragdoll = null;

    private void Start() {
        animator = GetComponent<Animator>();
        //agent = GetComponentInParent<NavMeshAgent>();
    }

    public void TakeDamage(float amount) {
        health -= amount;

        if (health <= 0) {
            health = 0;

            isDead = true;

            if (ragdoll != null) {
                ragdoll.gameObject.SetActive(true);
                transform.gameObject.SetActive(false);
                ragdoll.Ragdoll(transform);
            }

            if (agent != null) {
                agent.enabled = false;
            }

            if (animator != null) {
                animator.SetBool("Dead", true);
            }
        }
    }
}
