using UnityEngine;

public class CollectibleHealth : Collectible {
    [SerializeField] Health playerHealth;
    [SerializeField] float healthGain = 20f;

    public override void Collect() {
        playerHealth.Heal(healthGain);        
    }
}
