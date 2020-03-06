using UnityEngine;

public class Health : MonoBehaviour {
    public int maxHP = 100;
    public int hp = 100;
    public bool isDead = false;

    private Animator animator = null;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        this.hp -= damage;

        if (this.hp <= 0) {
            this.hp = 0;
            this.isDead = true;

            if (animator != null) {
                animator.SetBool("Dead", true);
            }
        } else {
            this.isDead = false;
        }
    }
}
