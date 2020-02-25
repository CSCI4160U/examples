using UnityEngine;

public class FirstPersonInput : MonoBehaviour {
    [SerializeField] private Transform camera = null;
    [SerializeField] private Animator gunAnimator = null;
    [SerializeField] private GameObject bulletHolePrefab = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private GameObject muzzleFlashPrefab = null;
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private float damage = 50f;

    float range = 100f;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    private void Shoot() {
        RaycastHit hit;

        LayerMask enemyMask = LayerMask.GetMask("Enemies");
        LayerMask groundMask = LayerMask.GetMask("Ground");
        LayerMask barrelMask = LayerMask.GetMask("Barrels");

        Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        gunAnimator.SetTrigger("Fire");
        audioSource.PlayDelayed(0.1f);

        if (Physics.Raycast(camera.position, camera.forward, out hit, range, enemyMask)) {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage(damage);
            } else {
                Debug.Log("Object has no EnemyHealth script: " + hit.collider.name);
            }
        } else if (Physics.Raycast(camera.position, camera.forward, out hit, range, barrelMask)) {
            Barrel barrel = hit.collider.GetComponent<Barrel>();
            barrel.Explode();
        } else if (Physics.Raycast(camera.position, camera.forward, out hit, range, groundMask)) {
            Instantiate(bulletHolePrefab, 
                hit.point + (0.01f * hit.normal), 
                Quaternion.LookRotation(-1 * hit.normal, hit.transform.up));
        }
    }
}
