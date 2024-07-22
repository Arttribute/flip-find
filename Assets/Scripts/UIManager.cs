using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _currentScoreText;

    private int _currentScore;

    public int CurrentScore => _currentScore; // Property to access current score

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetScore(); // Initialize the score at the start
    }

    public void AddScore(int points)
    {
        _currentScore += points;
        UpdateCurrentScoreUI();
    }

    public void ResetScore()
    {
        _currentScore = 0;
        UpdateCurrentScoreUI();
    }

    private void UpdateCurrentScoreUI()
    {
        _currentScoreText.text = "SCORE: " + _currentScore.ToString();
    }
}
