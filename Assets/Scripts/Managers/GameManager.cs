using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening; // Make sure to include the DOTween namespace

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Sprite[] CurrentCardBatch => currentCardBatch;
    public bool IsCheckingForMatch => isCheckingForMatch; // Public property to access isCheckingForMatch

    public GameObject cardPrefab;
    public Transform gridTransform;
    public Transform fullImageTransform; // Reference to the UI element to show full image
    public Image fullImageDisplay; // Image component to display the full image
    public AudioClip backgroundMusic; // Reference to the background music AudioClip
    public AudioClip flipSound; // Reference to the card flip sound AudioClip
    public CardBatch[] cardBatches;

    [SerializeField] private TMP_Text _feedBackMessage;

    private AudioSource audioSource; // AudioSource component to play the background music and sounds
    private Card firstFlippedCard;
    private Card secondFlippedCard;
    private Sprite[] currentCardBatch;
    private Sprite defaultCardBack; // Store the default card back
    private bool isCheckingForMatch = false; // Flag to prevent additional flips while checking for match

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        defaultCardBack = cardPrefab.GetComponent<Card>().cardBack; // Set the default card back
        LoadCardBatch();
        GenerateCards();
        PlayBackgroundMusic();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GenerateCards()
    {
        List<Sprite> images = new List<Sprite>(currentCardBatch);
        images.AddRange(currentCardBatch); // Duplicate images for pairs
        images = ShuffleList(images);

        foreach (Sprite image in images)
        {
            GameObject cardObject = Instantiate(cardPrefab, gridTransform);
            Card card = cardObject.GetComponent<Card>();
            card.SetCardImage(image);
            Sprite currentCardBack = CollectablesManager.instance.GetCurrentCardBack();
            card.SetCardBack(currentCardBack != null ? currentCardBack : defaultCardBack); // Set card back
            CardAnim cardAnim = cardObject.GetComponent<CardAnim>();
            cardAnim.Init(card); // Initialize CardAnim with the Card reference
        }
    }

    public void LoadCardBatch()
    {
        int level = CollectablesManager.instance.CurrentLevel;
        if (level < cardBatches.Length)
        {
            currentCardBatch = cardBatches[level].cardFaces;
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
        if (isCheckingForMatch)
            return; // Prevent flipping if a match check is in progress

        // Play flip sound
        PlayFlipSound();

        if (firstFlippedCard == null)
        {
            firstFlippedCard = card;
        }
        else if (secondFlippedCard == null)
        {
            secondFlippedCard = card;
            isCheckingForMatch = true; // Set flag to indicate match check is in progress
            StartCoroutine(CheckForMatch());
        }
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

            // Show feedback message based on the score
            ShowFeedbackMessage(UIManager.Instance.CurrentScore);

            CheckLevelCompletion();
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
        isCheckingForMatch = false; // Clear flag after match check is complete
    }

    private void ShowFeedbackMessage(int score)
    {
        // Determine feedback message based on score
        string message = "";
        if (score == 30)
        {
            message = "Great job!";
        }
        else if (score == 60)
        {
            message = "You're getting good at this!";
        }
        else if (score == 90)
        {
            message = "Fantastic! Keep it up!";
        }
        else if (score == 120)
        {
            message = "Amazing skills!";
        }
        else if (score == 150)
        {
            message = "Incredible! You're unstoppable!";
        }
         else if (score == 180)
        {
            message = "Wow keep it up!";
        }
         else if (score == 220)
        {
            message = "That was great!";
        }
         else if (score == 280)
        {
            message = "You're a natural!";
        }
         else if (score == 350)
        {
            message = "You nailed it!";
        }

        if (!string.IsNullOrEmpty(message))
        {
            _feedBackMessage.text = message; // Set the message
            _feedBackMessage.alpha = 0; // Set the text to transparent initially

            // Use DOTween to animate the text appearance
            _feedBackMessage.DOFade(1, 0.5f).OnComplete(() =>
            {
                // Fade out the message after a delay
                _feedBackMessage.DOFade(0, 1f).SetDelay(3.0f);
            });
        }
    }

    private void CheckLevelCompletion()
    {
        if (UIManager.Instance.CardFaceScore >= 60)
        {
            CollectablesManager.instance.OnLevelUp();
            UIManager.Instance.ResetScore(); // Optionally reset the score for the next level
            RestartGame(); // Restart the game when level is completed
        }
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

    private void RestartGame()
    {
        // Clear existing cards
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }
        // Load a new card batch and generate new cards
        LoadCardBatch();
        GenerateCards();
    }
}
