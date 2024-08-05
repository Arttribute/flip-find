using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public int CurrentScore => _currentScore; // Property to access current score
    public int CardFaceScore => _cardFaceScore;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private GameObject pauseMenuPanel; // Reference to the pause menu panel
    [SerializeField] private Button pauseButton; // Reference to the pause button
    [SerializeField] private Button resumeButton; // Reference to the resume button
    [SerializeField] private MenuAnim menuAnim; // Reference to the MenuAnim script
    [SerializeField] private AudioClip buttonClickSound; // Reference to the button click sound AudioClip

    private AudioSource audioSource; // AudioSource component to play the sounds
    private GameData gameData;
    private int _currentScore;
    private int highScore;
    private int _cardFaceScore;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        gameData = new GameData();

        _currentScore = 0;
        ResetScore(); // Initialize the score at the start

        // Add listeners to the pause and resume buttons
        pauseButton.onClick.AddListener(() => OnButtonClick(TogglePauseMenu));
        resumeButton.onClick.AddListener(() => OnButtonClick(TogglePauseMenu));

        // Initially hide the pause menu panel
        pauseMenuPanel.SetActive(false);
    }

    public void SetScore(int score)
    {
        _currentScore = score;
        UpdateCurrentScoreUI();
    }

    public void DisplayHighScore()
    {
        highScore = gameData.GetHighScore();
        _highScoreText.text = highScore.ToString();

    }

    public void AddScore(int points)
    {
        _currentScore += points;
        _cardFaceScore += points;
        gameData.SaveGame(CollectablesManager.instance.CurrentLevel, _currentScore);
        UpdateCurrentScoreUI();
    }

    public void ResetScore()
    {
        _cardFaceScore = 0;
        UpdateCurrentScoreUI();
        DisplayHighScore();
    }

    private void UpdateCurrentScoreUI()
    {
        highScore = gameData.GetHighScore();
        _currentScoreText.text = "SCORE: " + _currentScore.ToString();
        _highScoreText.text = highScore.ToString();
    }

    private void TogglePauseMenu()
    {
        StartCoroutine(PauseGameCoroutine());
    }

    private IEnumerator PauseGameCoroutine()
    {
        if (Time.timeScale == 0f)
        {
            menuAnim.HideMenu(); // Hide the menu with animation
            yield return new WaitForSecondsRealtime(menuAnim.GetHideAnimationDuration()); // Wait for the hide animation to complete
            Time.timeScale = 1f; // Resume the game
        }
        else
        {
            pauseMenuPanel.SetActive(true);
            menuAnim.ShowMenu(); // Show the menu with animation
            yield return new WaitForSecondsRealtime(menuAnim.GetShowAnimationDuration()); // Wait for the show animation to complete
            Time.timeScale = 0f; // Pause the game
        }
    }

    private void OnButtonClick(System.Action action)
    {
        PlayButtonClickSound();
        action.Invoke();
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
