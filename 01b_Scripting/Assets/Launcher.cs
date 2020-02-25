using UnityEngine;

public class Launcher : MonoBehaviour {
    public Rigidbody projectilePrefab;
    [Range(0, 100)] public float launchVelocity = 10.0f;
    public Vector3 offset;
    public float blastRadius = 1.0f;

    [ContextMenu("Fire")]
    public void Fire() {
        var projectile = Instantiate(projectilePrefab);
        projectile.position = transform.position + offset;
        projectile.velocity = transform.forward * launchVelocity;
    }
}
