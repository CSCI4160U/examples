using UnityEngine;

public class Collectible : InteractableObject {
    public virtual void Collect() {
        Debug.Log("Item collected: " + gameObject.name);
    }

    private void HideMesh() {
        gameObject.SetActive(false);
    }

    public override void Activate() {
        Collect();

        HideMesh();
    }
}
