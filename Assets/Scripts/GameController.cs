using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

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

    void Start()
    {
        _levelGrid = new LevelGrid(20, 20);

        _snake.Setup(_levelGrid);
        _levelGrid.Setup(_snake);

        CMDebug.ButtonUI(Vector2.zero, "Reload Scene", () => {
            Loader.Load(Loader.Scene.GameScene);
        });
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
}
