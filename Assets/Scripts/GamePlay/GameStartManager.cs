using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public static bool hasSeenTutorial = false; // Static variable to track tutorial state

    void Start()
    {
        if (!hasSeenTutorial) // If tutorial has not been seen
        {
            ShowTutorial();
            hasSeenTutorial = true; // Set to true so it doesn't show again
        }
    }

    void ShowTutorial()
    {
        
    }
}
