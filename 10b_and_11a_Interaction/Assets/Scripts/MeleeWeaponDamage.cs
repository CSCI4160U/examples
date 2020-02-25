using UnityEngine;

public class MeleeWeaponDamage : MonoBehaviour {
    [SerializeField] private int damage = 5;

    public void OnTriggerEnter(Collider other) {
        Health health = other.GetComponent<Health>();
        if (health != null) {
            health.TakeDamage(damage);
        } else {
            Debug.Log("MeleeWeaponDamage::Collider does not have a Health component");
        }
    }
}
