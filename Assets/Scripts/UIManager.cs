using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private GameObject pauseMenuPanel; // Reference to the pause menu panel
    [SerializeField] private Button pauseButton; // Reference to the pause button
    [SerializeField] private Button resumeButton; // Reference to the resume button

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

        // Add listeners to the pause and resume buttons
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);

        // Initially hide the pause menu panel
        pauseMenuPanel.SetActive(false);
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

    private void PauseGame()
    {
        pauseMenuPanel.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Pause the game
    }

    private void ResumeGame()
    {
        pauseMenuPanel.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game
    }
}
