using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour {
    public int maxHP = 100;
    public int hp = 100;
    public bool isDead = false;

    private Animator animator = null;
    private NavMeshAgent agent = null;

    private void Awake() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage) {
        this.hp -= damage;

        if (this.hp <= 0) {
            this.hp = 0;
            this.isDead = true;

            if (animator != null) {
                animator.SetBool("Dead", true);
                animator.SetFloat("Speed", 0.0f);
            }

            if (agent != null) {
                agent.enabled = false;
            }
        } else {
            this.isDead = false;
        }
    }
}
