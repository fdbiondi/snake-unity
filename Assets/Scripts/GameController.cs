using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /* int number = 0;
        FunctionPeriodic.Create(
            () =>
            {
                CMDebug.TextPopupMouse("Ding! " + number);
                number++;
            },
            .3f
        ); */
        GameObject snakeGameObject = new GameObject();
        SpriteRenderer snakeSpriteRenderer = snakeGameObject.AddComponent<SpriteRenderer>();

        snakeSpriteRenderer.sprite = GameAssets.instance.snakeHeadSprite;
    }

    // Update is called once per frame
    void Update() { }
}
