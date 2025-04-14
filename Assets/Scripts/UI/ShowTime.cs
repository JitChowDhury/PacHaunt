using UnityEngine;
using TMPro;

public class ShowTime : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText; 

    private void Start()
    {
        if (GameTimer.instance != null)
        {
            float time = GameTimer.instance.GetElapsedTime(); // Get elapsed time
            timeText.text = "Time: " + FormatTime(time); // Display formatted time
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds); // Format as MM:SS
    }
}
