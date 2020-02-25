using UnityEngine;

public class Ragdoller : MonoBehaviour {
    [SerializeField] Rigidbody mainBody = null;
    [SerializeField] Transform navigatingObject = null;

    public void Ragdoll(Transform newTransform) {
        // move and rotate the ragdoll the same as the current enemy, and show it in its place
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }

    public void ApplyForce(Vector3 forceDirection, float forceAmount) {
        if (mainBody != null && forceAmount > 0) {
            mainBody.AddForce(forceDirection * forceAmount);
        }
    }

    public void ApplyExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius) {
        if (mainBody != null && explosionForce > 0) {
            mainBody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        }
    }
}
