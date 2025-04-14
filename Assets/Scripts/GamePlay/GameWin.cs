using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make the cursor visible

            if (GameTimer.instance != null)
            {
                GameTimer.instance.StopTimer(); // Stop the timer
            }

            SceneManager.LoadScene("Win"); // Load the win screen/scene
        }
    }
}
