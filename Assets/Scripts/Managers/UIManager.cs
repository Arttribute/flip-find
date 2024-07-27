using System.Collections;
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
    [SerializeField] private MenuAnim menuAnim; // Reference to the MenuAnim script
    [SerializeField] private AudioClip buttonClickSound; // Reference to the button click sound AudioClip

    private AudioSource audioSource; // AudioSource component to play the sounds
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
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ResetScore(); // Initialize the score at the start

        // Add listeners to the pause and resume buttons
        pauseButton.onClick.AddListener(() => OnButtonClick(TogglePauseMenu));
        resumeButton.onClick.AddListener(() => OnButtonClick(TogglePauseMenu));

        // Initially hide the pause menu panel
        pauseMenuPanel.SetActive(true);
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

    private void TogglePauseMenu()
    {
        menuAnim.ToggleMenu(); // Show/hide the menu with animation
        StartCoroutine(PauseGameCoroutine());
    }

    private IEnumerator PauseGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(menuAnim.GetAnimationDuration()); // Wait for the animation to complete
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f; // Resume the game
        }
        else
        {
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
