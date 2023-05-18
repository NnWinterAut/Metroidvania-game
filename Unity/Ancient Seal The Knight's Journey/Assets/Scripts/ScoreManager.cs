using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int score;
    int currentScore;

    [SerializeField] Vector3 ScoreTextScale = new Vector3(1f, 1f, 1f);
    void Awake()
    {

    }
    public static ScoreManager Instance { get; private set; }

    public void ResetScore()
    {
        score = 0;
        ScoreDisplay.UpdateText(score);
    }

    public void AddScore(int scorepoint)
    {
        score += scorepoint;
        ScoreDisplay.UpdateText(score);
    }
}
