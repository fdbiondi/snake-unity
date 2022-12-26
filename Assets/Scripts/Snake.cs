using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Snake : MonoBehaviour
{
    private enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    private enum State {
        Alive,
        Dead
    }

    private State _state;

    private Vector2Int _gridPosition;
    private Direction _gridMoveDirection;
    private float _gridMoveTimer;
    private float _gridMoveTimerMax;
    private LevelGrid _levelGrid;
    private int _snakeBodySize;
    private List<SnakeMovePosition> _snakeMovePositionList;
    private List<SnakeBodyPart> _snakeBodyPartList;

    public void Setup(LevelGrid levelGrid)
    {
        _levelGrid = levelGrid;
    }

    private void Awake()
    {
        _gridPosition = new Vector2Int(10, 10);
        _gridMoveTimerMax = .2f;
        _gridMoveTimer = _gridMoveTimerMax;
        _gridMoveDirection = Direction.Right;

        _snakeMovePositionList = new List<SnakeMovePosition>();
        _snakeBodySize = 0;

        _snakeBodyPartList = new List<SnakeBodyPart>();

        _state = State.Alive;
    }

    private void Update()
    {
        switch (_state) {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && _gridMoveDirection != Direction.Down)
        {
            _gridMoveDirection = Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && _gridMoveDirection != Direction.Up)
        {
            _gridMoveDirection = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _gridMoveDirection != Direction.Right)
        {
            _gridMoveDirection = Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && _gridMoveDirection != Direction.Left)
        {
            _gridMoveDirection = Direction.Right;
        }
    }

    private void HandleGridMovement()
    {
        _gridMoveTimer += Time.deltaTime;

        if (_gridMoveTimer >= _gridMoveTimerMax)
        {
            _gridMoveTimer -= _gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;

            if (_snakeMovePositionList.Count > 0) {
                previousSnakeMovePosition = _snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, _gridPosition, _gridMoveDirection);
            _snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;

            switch (_gridMoveDirection) {
                case Direction.Right:
                    gridMoveDirectionVector = new Vector2Int(+1, 0);
                    break;
                case Direction.Left:
                    gridMoveDirectionVector = new Vector2Int(-1, 0);
                    break;
                case Direction.Up:
                    gridMoveDirectionVector = new Vector2Int(0, +1);
                    break;
                case Direction.Down:
                default:
                    gridMoveDirectionVector = new Vector2Int(0, -1);
                    break;
            }

            _gridPosition += gridMoveDirectionVector;
            _gridPosition = _levelGrid.ValidateGridPosition(_gridPosition);

            bool snakeAteFood = _levelGrid.TrySnakeEatFood(_gridPosition);

            if (snakeAteFood)
            {
                _snakeBodySize++;
                CreateSnakeBodyPart();
            }

            if (_snakeMovePositionList.Count >= _snakeBodySize + 1)
            {
                _snakeMovePositionList.RemoveAt(_snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in _snakeBodyPartList) {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();

                if (_gridPosition == snakeBodyPartGridPosition) {
                    // CMDebug.TextPopup("Game Over!", transform.position);

                    GameController.SnakeDied();
                    _state = State.Dead;
                }
            }

            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);
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

    private void CreateSnakeBodyPart()
    {
        _snakeBodyPartList.Add(new SnakeBodyPart(_snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < _snakeBodyPartList.Count; i++)
        {
            _snakeBodyPartList[i].SetSnakeMovePosition(_snakeMovePositionList[i]);
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

        foreach (SnakeMovePosition snakeMovePosition in _snakeMovePositionList) {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }

        return gridPositionList;
    }

    /*
     * Handles a Single Snake Body Part
     * */
    private class SnakeBodyPart
    {
        private SnakeMovePosition _snakeMovePosition;
        private Transform _transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));

            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder= -bodyIndex;
            _transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            Vector2Int gridPosition = snakeMovePosition.GetGridPosition();

            _snakeMovePosition = snakeMovePosition;
            _transform.position = new Vector3(gridPosition.x, gridPosition.y);

            float angle = 0f;

            switch (snakeMovePosition.GetDirection()) {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        case Direction.Left:
                            angle = 45;
                            _transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Right:
                            angle = -45;
                            _transform.position += new Vector3(-.2f, .2f);
                            break;
                        default:
                            angle = 0;
                            break;
                    }
                break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        case Direction.Left:
                            angle = 180 - 45;
                            _transform.position += new Vector3(.2f, -.2f);
                            break;
                        case Direction.Right:
                            angle = 180 + 45;
                            _transform.position += new Vector3(-.2f, -.2f);
                            break;
                        default:
                            angle = 180;
                            break;
                    }
                break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        case Direction.Down:
                            angle = 180 - 45;
                            _transform.position += new Vector3(-.2f, .2f);
                            break;
                        case Direction.Up:
                            angle = 45;
                            _transform.position += new Vector3(-.2f, -.2f);
                            break;
                        default:
                            angle = -90;
                            break;
                    }
                break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection()) {
                        case Direction.Down:
                            angle = 180 + 45;
                            _transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Up:
                            angle = -45;
                            _transform.position += new Vector3(.2f, -.2f);
                            break;
                        default:
                            angle = 90;
                            break;
                    }
                break;
            }

            _transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return _snakeMovePosition.GetGridPosition();
        }
    }

    /*
     * Handles one move position from the snake
     * */
    private class SnakeMovePosition
    {
        private SnakeMovePosition _previousSnakeMovePosition;
        private Vector2Int _gridPosition;

        private Direction _direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)  {
            _previousSnakeMovePosition = previousSnakeMovePosition;
            _gridPosition = gridPosition;
            _direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return _gridPosition;
        }

        public Direction GetDirection()
        {
            return _direction;
        }

        public Direction GetPreviousDirection()
        {
            if (_previousSnakeMovePosition == null) {
                return Direction.Right;
            }

            return _previousSnakeMovePosition.GetDirection();
        }
    }
}
