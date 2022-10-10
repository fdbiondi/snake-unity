using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int _gridPosition;
    private Vector2Int _gridMoveDirection;
    private float _gridMoveTimer;
    private float _gridMoveTimerMax;

    private void Awake()
    {
        _gridPosition = new Vector2Int(10, 10);
        _gridMoveDirection = new Vector2Int(1, 0);
        _gridMoveTimerMax = 1f;
        _gridMoveTimer = _gridMoveTimerMax;
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
            _gridMoveDirection.y = 1;
            _gridMoveDirection.x = 0;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && _gridMoveDirection.y != 1)
        {
            _gridMoveDirection.y = -1;
            _gridMoveDirection.x = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _gridMoveDirection.x != 1)
        {
            _gridMoveDirection.y = 0;
            _gridMoveDirection.x = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && _gridMoveDirection.x != -1)
        {
            _gridMoveDirection.y = 0;
            _gridMoveDirection.x = 1;
        }
    }

    private void HandleGridMovement()
    {
        _gridMoveTimer += Time.deltaTime;

        if (_gridMoveTimer >= _gridMoveTimerMax)
        {
            _gridPosition += _gridMoveDirection;
            _gridMoveTimer -= _gridMoveTimerMax;

            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(_gridMoveDirection) - 90);
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
}
