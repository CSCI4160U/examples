using UnityEngine;

public class MeleeWeaponDamage : MonoBehaviour {
    [SerializeField] private int damage = 5;

    public void OnTriggerEnter(Collider other) {
        Health health = other.GetComponent<Health>();

        if (health != null) {
            Debug.Log(transform.name + " dealing " + damage + " damage to " + other.name);
            health.TakeDamage(damage);
        }
    }
}
