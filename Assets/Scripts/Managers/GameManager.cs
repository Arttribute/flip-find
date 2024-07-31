using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsCheckingForMatch => isCheckingForMatch; // Public property to access isCheckingForMatch

    public GameObject cardPrefab;
    public Transform gridTransform;
    public Transform fullImageTransform; // Reference to the UI element to show full image
    public Image fullImageDisplay; // Image component to display the full image
    public Sprite[] cardImages;
    public AudioClip backgroundMusic; // Reference to the background music AudioClip
    public AudioClip flipSound; // Reference to the card flip sound AudioClip

    private AudioSource audioSource; // AudioSource component to play the background music and sounds
    private Timer timer;
    private Card firstFlippedCard;
    private Card secondFlippedCard;
    private bool isCheckingForMatch = false; // Flag to prevent additional flips while checking for match

    private Sprite defaultCardBack; // Store the default card back

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
        GenerateCards();
        PlayBackgroundMusic();
    }

    public void Quit()
    {
        Application.Quit();
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
            Sprite currentCardBack = CollectablesManager.instance.GetCurrentCardBack();
            card.SetCardBack(currentCardBack != null ? currentCardBack : defaultCardBack); // Set card back
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
        // Generate new cards
        GenerateCards();
    }
}
