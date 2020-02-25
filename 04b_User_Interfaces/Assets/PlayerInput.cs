using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public float speed = 10f;

    void Update() {
        float horizontalMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.Translate(transform.right * horizontalMove);
    }
}
