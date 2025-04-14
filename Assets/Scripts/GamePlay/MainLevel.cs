using UnityEngine;
using TMPro;

public class MainLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText; // Assign in Inspector

    private void Start()
    {
        if (GameTimer.instance != null)
        {
            GameTimer.instance.SetTimerText(timerText);
            GameTimer.instance.StartTimer();
        }
    }
}
