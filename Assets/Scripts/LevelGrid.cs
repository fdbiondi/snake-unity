using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class LevelGrid
{
    public int width;
    public int height;
    private GameObject _foodGameObject;
    private Vector2Int _foodGridPosition;
    private Snake _snake;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake)
    {
        _snake = snake;

        SpawnFood();
    }

    private void SpawnFood()
    {
        do
        {
            _foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (_snake.GetFullSnakePositionGridList().IndexOf(_foodGridPosition) != -1);

        _foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        _foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        _foodGameObject.transform.position = new Vector3(_foodGridPosition.x, _foodGridPosition.y);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == _foodGridPosition)
        {
            Object.Destroy(_foodGameObject);
            SpawnFood();

            GameController.AddScore();

            return true;
        }

        return false;
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0) {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width - 1) {
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0) {
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1) {
            gridPosition.y = 0;
        }

        return gridPosition;
    }
}
