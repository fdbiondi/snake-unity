using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Snake : MonoBehaviour
{
    private Vector2Int _gridPosition;
    private Vector2Int _gridMoveDirection;
    private float _gridMoveTimer;
    private float _gridMoveTimerMax;
    private LevelGrid _levelGrid;
    private int _snakeBodySize;
    private List<Vector2Int> _snakeMovePositionList;

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
        _snakeBodySize = 1;
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

            if (_snakeMovePositionList.Count >= _snakeBodySize + 1)
            {
                _snakeMovePositionList.RemoveAt(_snakeBodySize);
            }

            for (int i = 0; i < _snakeMovePositionList.Count; i++)
            {
                AddTale(_snakeMovePositionList[i]);
            }

            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(_gridMoveDirection) - 90);

            _levelGrid.SnakeMoved(_gridPosition);
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

    public Vector2Int GetGridPosition()
    {
        return _gridPosition;
    }

    private void AddTale(Vector2Int snakeMovePosition)
    {
        GameObject snakeTaleGameObj = CreateTale(snakeMovePosition);

        FunctionTimer.Create(
            () =>
            {
                Object.Destroy(snakeTaleGameObj);
            },
            _gridMoveTimerMax
        );
    }

    private GameObject CreateTale(Vector2Int snakeMovePosition)
    {
        Vector3 localPosition = new Vector3(snakeMovePosition.x, snakeMovePosition.y);
        Vector3 localScale = Vector3.one * .5f;
        Sprite sprite = Assets.i.s_White;
        int sortingOrder = (int)(5000 - localPosition.y);

        GameObject snakeTaleGameObj = new GameObject("SnakeTale", typeof(SpriteRenderer));
        Transform transform = snakeTaleGameObj.transform;
        transform.SetParent(null, false);
        transform.localPosition = localPosition;
        transform.localScale = localScale;

        SpriteRenderer spriteRenderer = snakeTaleGameObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.color = Color.white;

        transform = snakeTaleGameObj.transform;
        spriteRenderer = snakeTaleGameObj.GetComponent<SpriteRenderer>();

        return snakeTaleGameObj;
    }
}
