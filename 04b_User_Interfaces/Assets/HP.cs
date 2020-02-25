using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour {
    public int hitPoints = 100;
    public Slider healthBar = null;

    public void Damage(int amount) {
        Debug.Log("Damage taken: " + amount);
        hitPoints -= amount;

        if (hitPoints <= 0) {
            Debug.Log("We died");
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar() {
        if (healthBar != null) {
            healthBar.value = this.hitPoints;
        }
    }
}
