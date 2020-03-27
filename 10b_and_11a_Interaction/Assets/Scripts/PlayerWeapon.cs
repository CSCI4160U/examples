using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
    [SerializeField] bool weaponExists = false;
    [SerializeField] int ammo = 0;

    public void ObtainedWeapon() {
        this.weaponExists = true;
    }

    public void CollectAmmo(int amount) {
        this.ammo += amount;
    }
}
