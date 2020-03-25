using UnityEngine;

public class InteracatableDrawer : InteractableObject {
    [SerializeField] private string animatorParam;

    [SerializeField] private Animator animator;

    private bool isOpen = false;

    public override void Activate() {
        ToggleDrawer();
    }

    private void ToggleDrawer() {
        isOpen = !isOpen;

        animator.SetBool(animatorParam, isOpen);
    }
}
