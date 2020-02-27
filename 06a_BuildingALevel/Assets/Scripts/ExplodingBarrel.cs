using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour {
    [SerializeField] float radius = 3;
    [SerializeField] GameObject explosionEffect = null;
    [SerializeField] Transform explosionPoint = null;
    [SerializeField] int damage = 100;
    [SerializeField] bool isDestroyed = false;

    private List<ExplodingBarrel> barrelsToExplode = null;

    private void Start() {
        barrelsToExplode = new List<ExplodingBarrel>();
    }

    public void Explode() {
        Instantiate(explosionEffect, explosionPoint.position, Quaternion.identity);
        Destroy(transform.gameObject, 0.25f);

        // see if any players are nearby to damage
        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, playerMask);
        if (hits.Length > 0) {
            Health health = hits[0].GetComponent<Health>();
            health.TakeDamage(damage);
        }

        // see if any enemies are nearby to damage
        LayerMask enemyMask = LayerMask.GetMask("Enemies");
        hits = Physics.OverlapSphere(transform.position, radius, enemyMask);
        if (hits.Length > 0) {
            Health health = hits[0].GetComponent<Health>();
            health.TakeDamage(damage);
        }

        // prevent an infinite cascade, remember that this barrel is already destroyed
        isDestroyed = true;

        // see if any other barrels are nearby to cascade
        LayerMask barrelMask = LayerMask.GetMask("Barrels");
        hits = Physics.OverlapSphere(transform.position, radius, barrelMask);
        barrelsToExplode.Clear();
        for (int i = 0; i < hits.Length; i++) {
            ExplodingBarrel barrel = hits[i].GetComponent<ExplodingBarrel>();
            if (!barrel.isDestroyed) {
                barrelsToExplode.Add(barrel);
            }
        }
        if (barrelsToExplode.Count > 0) {
            CascadeBarrels();
        }
    }

    void CascadeBarrels() {
        foreach (ExplodingBarrel barrel in barrelsToExplode) {
            barrel.Explode();
        }
    }
}
