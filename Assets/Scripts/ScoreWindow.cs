using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text _scoreText;

    private void Awake()
    {
        _scoreText = transform.Find("ScoreText").GetComponent<Text>();
    }
    
    private void Update() {
        _scoreText.text = GameController.GetScore().ToString();
    }
}
