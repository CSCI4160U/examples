using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {
    [SerializeField] float explosionRadius = 5;
    [SerializeField] GameObject explosionEffect = null;
    [SerializeField] Transform explosionPoint = null;
    [SerializeField] float damage = 100f;
    [SerializeField] bool isDestroyed = false;
    [SerializeField] float explosionForce = 10000f;

    private List<Barrel> barrelsToExplode = new List<Barrel>();

    public void Explode() {
        Instantiate(explosionEffect, explosionPoint.position, Quaternion.identity);
        Destroy(transform.gameObject, 0.25f);

        // see if any players are nearby to damage
        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, playerMask);
        if (hits.Length > 0) {
            Health health = hits[0].GetComponent<Health>();
            health.TakeDamage(damage);
        }

        // see if any enemies are nearby to damage
        LayerMask enemyMask = LayerMask.GetMask("Enemies");
        hits = Physics.OverlapSphere(transform.position, explosionRadius, enemyMask);
        for (int i = 0; i < hits.Length; i++) {
            Debug.Log("Barrel is hitting enemy: " + hits[i].name);
            EnemyHealth health = hits[i].GetComponentInChildren<EnemyHealth>();
            if (health != null) {
                health.TakeDamage(damage);

                if (health.ragdoll != null) {
                    // apply an explosion force
                    Vector3 explosionDirection = (hits[i].transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(hits[i].transform.position, transform.position);
                    float force = explosionForce / distance;
                    health.ragdoll.ApplyForce(explosionDirection, force);
                }
            }
        }

        // prevent an infinite cascade, by remembering that this barrel is already destroyed
        isDestroyed = true;

        // see if any other barrels are nearby to cascade
        LayerMask barrelMask = LayerMask.GetMask("Barrels");
        hits = Physics.OverlapSphere(transform.position, explosionRadius, barrelMask);
        barrelsToExplode.Clear();
        for (int i = 0; i < hits.Length; i++) {
            Barrel barrel = hits[i].GetComponent<Barrel>();
            if (!barrel.isDestroyed) {
                barrelsToExplode.Add(barrel);
            }
        }
        if (barrelsToExplode.Count > 0) {
            Invoke("CascadeBarrels", 0.2f);
        }
    }

    void CascadeBarrels() {
        foreach (Barrel barrel in barrelsToExplode) {
            barrel.Explode();
        }
    }
}
