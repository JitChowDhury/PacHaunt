using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    [SerializeField] GameObject mapPanel; // UI panel for the map popup
    [SerializeField] private LookMouse lookMouse; // Reference to mouse look script

    private static bool hasSeenMapPopup = false; // Tracks if the popup has already been shown

    private void Start()
    {
        // If the popup was already shown in this session, remove the trigger
        if (hasSeenMapPopup)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Show the map popup only if it hasn't been seen before
        if (!hasSeenMapPopup)
        {
            mapPanel.SetActive(true); // Show the map UI
            lookMouse.LockCursor(false); // Unlock cursor for interaction
            hasSeenMapPopup = true; // Mark popup as seen
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Invoke("ClosePopup", 2.5f); // Auto-close popup after delay
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ClosePopup(); // Allow manual closing with 'M' key
        }
    }

    void ClosePopup()
    {
        mapPanel.SetActive(false); // Hide the map UI
        Destroy(gameObject); // Remove the trigger so it doesn't reappear
        lookMouse.LockCursor(true); // Lock cursor back for gameplay
    }
}
