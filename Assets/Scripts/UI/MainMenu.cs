using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Loads the next scene in the build order
    public void PlayGame()
    {
        if (GameTimer.instance != null)
        {
            GameTimer.instance.ResetTimer(); // Reset timer before starting a new game
            GameTimer.instance.StartTimer(); // Start the timer
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
    public void QuitGame()
    {
        Debug.Log("Quitting"); // Logs quit action
        Application.Quit();
    }

   
}
