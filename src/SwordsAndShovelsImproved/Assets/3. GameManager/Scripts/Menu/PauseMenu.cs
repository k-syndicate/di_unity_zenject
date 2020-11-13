using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button ResumeButton;
    public Button QuitButton;
    public Button RestartButton;

    private void Awake()
    {
        ResumeButton.onClick.AddListener(HandleResumeClick);
        QuitButton.onClick.AddListener(HandleQuitClick);
        RestartButton.onClick.AddListener(HandleRestartClick);
    }

    void HandleResumeClick()
    {
        GameManager.Instance.TogglePause();
    }

    void HandleQuitClick()
    {
        GameManager.Instance.QuitGame();
    }

    void HandleRestartClick()
    {
        GameManager.Instance.RestartGame();
    }
}
