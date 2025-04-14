using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text finalTimeText;



    private void Start()
    {
        if (GameTimer.instance == null)
        {
            Debug.Log("NO INSTANCE");
        }

        if (GameTimer.instance != null)
        {
            float finalTime = GameTimer.instance.GetElapsedTime();
            finalTimeText.text = "Final Time: " + FormatTime(finalTime);
        }

    }
    // Loads the main level scene
    public void LoadMainLevel()
    {

        SceneManager.LoadScene("MainLevel");

    }
    // Loads the main menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
