using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    private static int Score;

    [SerializeField]
    private Snake _snake;
    private LevelGrid _levelGrid;

    private void Awake()
    {
        instance = this;
        InitializeStatic();
    }

    private void Start()
    {
        _levelGrid = new LevelGrid(20, 20);

        _snake.Setup(_levelGrid);
        _levelGrid.Setup(_snake);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameController.IsGamePaused()) {
                GameController.ResumeGame();
            } else {
                GameController.PauseGame();
            }
        }    
    }

    private static void InitializeStatic()
    {
        Score = 0;
    }

    public static int GetScore()
    {
        return Score;
    }
    
    public static void AddScore()
    {
        Score += 100;
    }
    
    public static void SnakeDied()
    {
        GameOverWindow.ShowStatic();
    }
    
    public static void ResumeGame()
    {
        PauseWindow.HideStatic();

        Time.timeScale = 1f;
    }
    
    public static void PauseGame()
    {
        PauseWindow.ShowStatic();

        Time.timeScale = 0f;
    }

    public static bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }
}
