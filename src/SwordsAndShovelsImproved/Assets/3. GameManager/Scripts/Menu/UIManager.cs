using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;

    [SerializeField] private Camera _dummyCamera;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _mainMenu.OnFadeComplete.AddListener(HandleMainMenuFadeComplete);

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.StartGame();
            }
        }
    }

    private void HandleMainMenuFadeComplete(bool fadeIn)
    {
        // pass it on
        OnMainMenuFadeComplete.Invoke(fadeIn);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        switch (currentState)
        {
            case GameManager.GameState.PAUSED:
                _pauseMenu.gameObject.SetActive(true);
                break;

            default:
                _pauseMenu.gameObject.SetActive(false);
                break;
        }
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }
}
