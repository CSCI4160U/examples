using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            HP hp = other.gameObject.GetComponent<HP>();
            hp.Damage(10);
        }
    }
}
