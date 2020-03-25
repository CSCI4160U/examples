using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Transform camera;
    [SerializeField] private float range = 2f;
    [SerializeField] private Text interactionText;

    private void Update() {
        RaycastHit hit;
        LayerMask interactableMask = LayerMask.GetMask("Interactable");
        InteractableObject interactableObject = null;

        if (Physics.Raycast(camera.position, camera.forward, out hit, range, interactableMask)) {
            interactableObject = hit.collider.gameObject.GetComponent<InteractableObject>();
        }

        if (interactableObject != null) {
            interactionText.text = interactableObject.GetInteractionText();

            if (Input.GetButtonDown("Fire2")) {
                interactableObject.Activate();
            }
        } else {
            interactionText.text = "";
        }
    }
}
