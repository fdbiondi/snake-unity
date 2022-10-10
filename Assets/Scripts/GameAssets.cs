using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    [SerializeField]
    private Sprite _snakeHeadSprite;

    public static GameAssets instance;

    public Sprite snakeHeadSprite
    {
        get { return _snakeHeadSprite; }
    }

    private void Awake()
    {
        instance = this;
    }

    void Update() { }
}
