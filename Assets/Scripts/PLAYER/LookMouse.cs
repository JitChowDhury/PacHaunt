using UnityEngine;

public class LookMouse : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f; // Sensitivity of the mouse movement
    [SerializeField] private Transform Body; // Reference to the player's body for horizontal rotation

    private float xRotation = 0f; // Vertical rotation value
    private bool isCursorLocked = true; // Tracks whether the cursor is locked

    void Start()
    {
        LockCursor(true); // Lock the cursor when the game starts
    }

    void Update()
    {
        if (isCursorLocked)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Adjust vertical rotation (clamped to avoid flipping)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            // Apply vertical rotation to the camera
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Rotate the player body horizontally
            Body.Rotate(Vector3.up * mouseX);
        }
    }

    public void LockCursor(bool shouldLock)
    {
        // Lock or unlock the cursor based on input
        isCursorLocked = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
