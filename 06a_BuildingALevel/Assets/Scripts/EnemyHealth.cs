using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float health = 100f;
    public bool isDead = false;

    public Ragdoller ragdoller = null;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float amount) {
        health -= amount;

        if (health <= 0) {
            health = 0;

            isDead = true;

            if (ragdoller != null) {
                Debug.Log("Ragdolling...");
                // do ragdoll
                ragdoller.gameObject.SetActive(true);
                transform.gameObject.SetActive(false);
                ragdoller.Ragdoll(transform);
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
