using UnityEngine;

public class CollectibleWeapon : Collectible {
    [SerializeField] PlayerWeapon playerWeapon;
    [SerializeField] int ammoGain = 12;
    [SerializeField] bool includesWeapon = false;

    public override void Collect() {
        if (includesWeapon) {
            playerWeapon.ObtainedWeapon();
        }

        playerWeapon.CollectAmmo(ammoGain);
    }
}
