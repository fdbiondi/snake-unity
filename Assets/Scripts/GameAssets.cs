using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [SerializeField]
    private Sprite _snakeHeadSprite;

    [SerializeField]
    private Sprite _snakeBodySprite;

    [SerializeField]
    private Sprite _foodSprite;

    public static GameAssets instance;

    public Sprite snakeHeadSprite
    {
        get { return _snakeHeadSprite; }
    }

    public Sprite snakeBodySprite
    {
        get { return _snakeBodySprite; }
    }

    public Sprite foodSprite
    {
        get { return _foodSprite; }
    }

    private void Awake()
    {
        instance = this;
    }

    void Update() { }
}
