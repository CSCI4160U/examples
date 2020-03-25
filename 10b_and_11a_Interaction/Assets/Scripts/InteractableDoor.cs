using System.Collections;
using UnityEngine;

public class InteractableDoor : InteractableObject {
    [SerializeField] private bool autoOpen = false;
    [SerializeField] private bool autoClose = false;
    [SerializeField] private float autoCloseDelay = 8f;

    [SerializeField] private bool isUnlocked = true;
    [SerializeField] private string lockedText = "Find a way to unlock this door";
    [SerializeField] private string openText = "Close this door";

    [SerializeField] private Animator animator = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private float audioDelay = 0.2f;

    private bool isOpen = false;


    private void OpenDoor() {
        isOpen = true;

        animator.SetBool("Open", true);

        audioSource.PlayDelayed(audioDelay);

        // Close the door (if auto-close)
        if (autoClose) {
            StartCoroutine(CloseAfterDelay(autoCloseDelay));
        }
    }

    public IEnumerator CloseAfterDelay(float delay) {
        float timePassed = 0f;

        do {
            timePassed += Time.deltaTime;
            yield return null;
        } while (timePassed < delay);

        CloseDoor();

        yield return null;
    }

    private void CloseDoor() {
        isOpen = false;

        animator.SetBool("Open", false);
    }

    public void Unlock() {
        isUnlocked = true;
    }

    public override void Activate() {
        if (!isOpen) {
            if (isUnlocked) {
                OpenDoor();
            }
        } else {
            CloseDoor();
        }
    }

    public override string GetInteractionText() {
        if (isOpen) {
            return openText;
        } else if (isUnlocked) {
            return activateText;
        } else {
            return lockedText;
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter()");
        if (autoOpen && !isOpen) {
            if (other.gameObject.CompareTag("Player")) {
                OpenDoor();
            }
        }
    }
}
