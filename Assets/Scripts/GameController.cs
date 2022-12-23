using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Snake _snake;
    private LevelGrid _levelGrid;

    void Start()
    {
        /* GameObject snakeGameObject = new GameObject();
       SpriteRenderer snakeSpriteRenderer = snakeGameObject.AddComponent<SpriteRenderer>();
       snakeSpriteRenderer.sprite = GameAssets.instance.snakeHeadSprite; */
        _levelGrid = new LevelGrid(20, 20);

        _snake.Setup(_levelGrid);
        _levelGrid.Setup(_snake);
    }
}
