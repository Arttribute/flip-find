using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _currentScoreText;

    private int _currentScore;
    private int _highScore;

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
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();
    }

    public void AddScore(int points)
    {
        _currentScore += points;
        UpdateCurrentScoreUI();

        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            PlayerPrefs.SetInt("HighScore", _highScore);
            UpdateHighScoreUI();
        }
    }

    public void ResetScore()
    {
        _currentScore = 0;
        UpdateCurrentScoreUI();
    }

    private void UpdateHighScoreUI()
    {
        _highScoreText.text = "BEST: " + _highScore.ToString();
    }

    private void UpdateCurrentScoreUI()
    {
        _currentScoreText.text = "High Score: " + _currentScore.ToString();
    }
}