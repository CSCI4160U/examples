using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Transform camera;
    [SerializeField] private float range = 2f;
    [SerializeField] private Text interactionText;

    private void Update() {
        RaycastHit hit;

        Collectible collectible = null;
        InteractableObject interactableObject = null;

        LayerMask collectibleMask = LayerMask.GetMask("Collectible");
        LayerMask interactableMask = LayerMask.GetMask("Interactable");

        if (Physics.Raycast(camera.position, camera.forward, out hit, range, collectibleMask)) {
            collectible = hit.collider.GetComponent<Collectible>();
        }

        if (Physics.Raycast(camera.position, camera.forward, out hit, range, interactableMask)) {
            interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();
        }

        if (collectible != null) {
            interactionText.text = collectible.GetInteractionText();

            if (Input.GetButtonDown("Fire2")) {
                collectible.Activate();
            }
        } else if (interactableObject != null) {
            interactionText.text = interactableObject.GetInteractionText();

            if (Input.GetButtonDown("Fire2")) {
                interactableObject.Activate();
            }
        } else {
            interactionText.text = "";
        }
    }
}
