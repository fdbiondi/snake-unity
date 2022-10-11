using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int _gridPosition;
    private Vector2Int _gridMoveDirection;
    private float _gridMoveTimer;
    private float _gridMoveTimerMax;
    private LevelGrid _levelGrid;
    private int _snakeBodySize;
    private List<Vector2Int> _snakeMovePositionList;
    private List<Transform> _snakeTransformList;

    public void Setup(LevelGrid levelGrid)
    {
        _levelGrid = levelGrid;
    }

    private void Awake()
    {
        _gridPosition = new Vector2Int(10, 10);
        _gridMoveDirection = new Vector2Int(1, 0);
        _gridMoveTimerMax = .3f;
        _gridMoveTimer = _gridMoveTimerMax;

        _snakeMovePositionList = new List<Vector2Int>();
        _snakeBodySize = 0;

        _snakeTransformList = new List<Transform>();
    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _gridMoveDirection.y != -1)
        {
            _gridMoveDirection.x = 0;
            _gridMoveDirection.y = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && _gridMoveDirection.y != 1)
        {
            _gridMoveDirection.x = 0;
            _gridMoveDirection.y = -1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _gridMoveDirection.x != 1)
        {
            _gridMoveDirection.x = -1;
            _gridMoveDirection.y = 0;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && _gridMoveDirection.x != -1)
        {
            _gridMoveDirection.x = 1;
            _gridMoveDirection.y = 0;
        }
    }

    private void HandleGridMovement()
    {
        _gridMoveTimer += Time.deltaTime;

        if (_gridMoveTimer >= _gridMoveTimerMax)
        {
            _gridMoveTimer -= _gridMoveTimerMax;
            _snakeMovePositionList.Insert(0, _gridPosition);
            _gridPosition += _gridMoveDirection;

            bool snakeAteFood = _levelGrid.TrySnakeEatFood(_gridPosition);
            if (snakeAteFood)
            {
                _snakeBodySize++;
                CreateSnakeBody();
            }

            if (_snakeMovePositionList.Count >= _snakeBodySize + 1)
            {
                _snakeMovePositionList.RemoveAt(_snakeMovePositionList.Count - 1);
            }

            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(_gridMoveDirection) - 90);

            MoveSnakeBody();
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (n < 0)
        {
            n += 360;
        }

        return n;
    }

    private void CreateSnakeBody()
    {
        GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));

        snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets
            .instance
            .snakeBodySprite;

        snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder =
            -_snakeTransformList.Count;

        _snakeTransformList.Add(snakeBodyGameObject.transform);
    }

    private void MoveSnakeBody()
    {
        for (int i = 0; i < _snakeTransformList.Count; i++)
        {
            _snakeTransformList[i].position = new Vector3(
                _snakeMovePositionList[i].x,
                _snakeMovePositionList[i].y
            );
        }
    }

    public Vector2Int GetGridPosition()
    {
        return _gridPosition;
    }

    // Return complete snake Head + Body
    public List<Vector2Int> GetFullSnakePositionGridList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { _gridPosition };

        gridPositionList.AddRange(_snakeMovePositionList);

        return gridPositionList;
    }
}
