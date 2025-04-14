using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false; // Tracks if the game is paused
    [SerializeField] private GameObject PauseMenuUI; // Reference to the pause menu UI
    [SerializeField] private LookMouse lookMouse; // Reference to the mouse look script

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check for Escape key press
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false); // Hide pause menu
        Time.timeScale = 1.0f; // Resume game time
        isPaused = false;
        lookMouse.LockCursor(true); // Lock cursor for normal gameplay
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true); // Show pause menu
        Time.timeScale = 0f; // Stop game time
        isPaused = true;
        lookMouse.LockCursor(false); // Unlock cursor for menu navigation
    }

    public void OpenSettings()
    {
        // Placeholder for opening settings menu
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f; // Reset game time before switching scenes
        SceneManager.LoadScene("MainMenu"); // Load main menu scene
    }
}
