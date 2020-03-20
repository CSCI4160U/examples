using UnityEngine;

public class Ragdoller : MonoBehaviour {
    [SerializeField] Rigidbody mainBody = null;

    public void Ragdoll(Transform newTransform) {
        // move the ragdoll to where the non-ragdoll enemy is
        transform.position = newTransform.position;

        // rotate the ragdoll to how the non-ragdoll enemy is rotated
        transform.rotation = newTransform.rotation;
    }

    public void ApplyForce(Vector3 forceDirection, float forceAmount) {
        if (mainBody != null && forceAmount > 0) {
            mainBody.AddForce(forceDirection * forceAmount);
        }
    }
}
