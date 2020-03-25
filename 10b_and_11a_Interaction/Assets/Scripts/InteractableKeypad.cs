using UnityEngine;

public class InteractableKeypad : InteractableObject {
    private AudioSource audioSource = null;

    [SerializeField] private float audioDelay = 0.2f;
    [SerializeField] private InteractableDoor doorToUnlock = null;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public override void Activate() {
        Unlock();
    }

    private void Unlock() {
        if (doorToUnlock != null) {
            doorToUnlock.Unlock();
        }

        audioSource.PlayDelayed(audioDelay);
    }
}
