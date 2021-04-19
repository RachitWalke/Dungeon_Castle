using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int scoreCount;
    // Start is called before the first frame update
    void Start()
    {
        scoreCount = 0;
        scoreText.text = scoreCount.ToString();

        GameManager.instance.onUpdateScore += UpdateScore;
    }

    private void UpdateScore(int num)
    {
        scoreCount += num;
        scoreText.text = scoreCount.ToString();
    }

}
