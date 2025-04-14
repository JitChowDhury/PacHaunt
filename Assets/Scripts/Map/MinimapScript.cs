using Unity.VisualScripting;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    [SerializeField] Transform player; // Reference to the player
    [SerializeField] GameObject Hint;  // UI hint that can be toggled

    private void LateUpdate()
    {
        // Keep the minimap camera positioned above the player
        Vector3 newPos = player.position;
        newPos.y = transform.position.y; // Maintain the original Y position
        transform.position = newPos;

        // Rotate the minimap to match the player's orientation
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0);
    }

    private void Update()
    {
        // Toggle hint visibility when pressing 'C'
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isHintActive = !Hint.activeSelf;
            Hint.SetActive(isHintActive);
        }
    }
}
