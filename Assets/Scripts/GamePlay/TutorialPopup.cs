using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;
    [SerializeField] MovePlayer movePlayer;
    [SerializeField] LookMouse lookMouse;
    [SerializeField] GameObject canvas;

    private static bool hasSeenTutorial = false;

    void Start()
    {
        if (hasSeenTutorial)
        {
            popupPanel.SetActive(false);
            movePlayer.enabled = true;
            lookMouse.enabled = true;
            canvas.SetActive(true);
            lookMouse.LockCursor(true);
            return;
        }


        popupPanel.SetActive(true);
        movePlayer.enabled = false;
        lookMouse.enabled = false;
        canvas.SetActive(false);
        lookMouse.LockCursor(false);
    }

    public void CloseButton()
    {
        movePlayer.enabled = true;
        lookMouse.enabled = true;
        canvas.SetActive(true);
        popupPanel.SetActive(false);
        lookMouse.LockCursor(true);
        hasSeenTutorial = true;
    }
}
