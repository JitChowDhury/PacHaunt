using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance; // Singleton instance

    private float elapsedTime = 0f; // Timer value
    private bool isRunning = false; // Check if timer is active
    private TMP_Text timerText; // UI text reference

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = "Time: " + FormatTime(elapsedTime);
            }
        }
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // This method is called when the level starts
    public void SetTimerText(TMP_Text text)
    {
        timerText = text;
    }

    // Reset the timer when restarting the game
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = false;
    }
}
