using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening; // Add this for DoTween

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public GameObject cardPrefab;
    public Transform gridTransform;
    public Transform fullImageTransform; // Reference to the UI element to show full image
    public Image fullImageDisplay; // Image component to display the full image
    public Sprite[] cardImages;

    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject tutorialPointer;
    [SerializeField] private GameObject tutorialMessage;

    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu
    [SerializeField] private Button pauseButton; // Reference to the pause button
    [SerializeField] private Button backButton; // Reference to the back button

    private Timer timer;
    private Card firstFlippedCard;
    private Card secondFlippedCard;

    [SerializeField] private bool isTutorialCompleted;

    private Card card;

    [SerializeField] private AudioClip backgroundMusic; // Reference to the background music AudioClip
    [SerializeField] private AudioClip flipSound; // Reference to the card flip sound AudioClip
    private AudioSource audioSource; // AudioSource component to play the background music

    private bool isPaused = false; // To track the pause state

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void Start()
    {
        tutorialPointer.SetActive(true);
        tutorialMessage.SetActive(true);
        tutorialText.text = "Flip the indicated card";
        GenerateCards();
        timer.ResetTimer();
        PlayBackgroundMusic();

        // Initially hide the pause menu
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (!isPaused)
        {
            OnCardFlip();
        }
    }

    public void GenerateCards()
    {
        List<Sprite> images = new List<Sprite>(cardImages);
        images.AddRange(cardImages); // Duplicate images for pairs
        images = ShuffleList(images);

        foreach (Sprite image in images)
        {
            GameObject cardObject = Instantiate(cardPrefab, gridTransform);
            Card card = cardObject.GetComponent<Card>();
            card.SetCardImage(image);
            CardAnim cardAnim = cardObject.GetComponent<CardAnim>();
            cardAnim.Init(card); // Initialize CardAnim with the Card reference
        }
    }

    List<Sprite> ShuffleList(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    public void OnCardFlipped(Card card)
    {
        if (firstFlippedCard == null)
        {
            firstFlippedCard = card;
        }
        else if (secondFlippedCard == null)
        {
            secondFlippedCard = card;
            StartCoroutine(CheckForMatch());
        }
        PlayFlipSound();
    }

    private IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(1); // Wait for a second to let the player see the flipped cards

        if (firstFlippedCard.cardFront == secondFlippedCard.cardFront)
        {
            // Match found
            fullImageDisplay.sprite = firstFlippedCard.cardFront;
            firstFlippedCard.SetMatched();
            secondFlippedCard.SetMatched();

            // Show score message
            CollectablesManager.instance.ShowScoreMessage();

            // Update score
            UIManager.Instance.AddScore(10); // Add points for a correct match

            CheckTutorialCompletion();
        }
        else
        {
            // No match, flip cards back
            firstFlippedCard.GetComponent<CardAnim>().FlipBack();
            secondFlippedCard.GetComponent<CardAnim>().FlipBack();
        }

        // Reset flipped cards
        firstFlippedCard = null;
        secondFlippedCard = null;
    }

    private void OnCardFlip()
    {
        Card[] allCards = FindObjectsOfType<Card>();

        foreach (Card card in allCards)
        {
            if (card.IsFlipped)
            {
                tutorialPointer.SetActive(false);
                tutorialText.text = "Great! Now flip another card";

                if (card.IsMatched)
                {
                    tutorialText.text = "Tutorial Complete! Flip all cards";
                    break;
                }
            }
        }
    }

    private void CheckTutorialCompletion()
    {
        if (UIManager.Instance.CurrentScore >= 60)
        {
            OnTutorialComplete();
        }

        if (AllCardsMatched())
        {
            RestartGame();
        }
    }

    private bool AllCardsMatched()
    {
        foreach (Transform child in gridTransform)
        {
            Card card = child.GetComponent<Card>();
            if (!card.IsMatched)
            {
                return false;
            }
        }
        return true;
    }

    public void StartTutorial()
    {
        isTutorialCompleted = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    private void OnTutorialComplete()
    {
        isTutorialCompleted = true;
        //PlayFabManager.Instance.MarkTutorialAsCompleted();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void PlayFlipSound()
    {
        if (flipSound != null)
        {
            audioSource.PlayOneShot(flipSound);
        }
    }

    private void OnPauseButtonClicked()
    {
        if (!isPaused)
        {
            isPaused = true;
            StartCoroutine(PauseGameCoroutine());
        }
    }

    private void OnBackButtonClicked()
    {
        if (isPaused)
        {
            StartCoroutine(ResumeGameCoroutine());
        }
    }

    private IEnumerator PauseGameCoroutine()
    {
        // Animate the menu from left to right
        pauseMenu.SetActive(true);
        pauseMenu.transform.DOMoveX(Screen.width / 2, 0.5f).SetEase(Ease.OutQuad);
        yield return new WaitForSecondsRealtime(0.5f); // Wait for the animation to complete
        Time.timeScale = 0f; // Pause the game
    }

    private IEnumerator ResumeGameCoroutine()
    {
        // Animate the menu from right to left
        pauseMenu.transform.DOMoveX(-Screen.width / 2, 0.5f).SetEase(Ease.OutQuad);
        yield return new WaitForSecondsRealtime(0.5f); // Wait for the animation to complete
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    private void RestartGame()
    {
        // Clear existing cards
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }
        // Generate new cards
        GenerateCards();
        // Reset timer
        timer.ResetTimer();
    }
}
