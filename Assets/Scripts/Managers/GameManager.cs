using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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
        GenerateCards();
        //timer.ResetTimer();
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
            card.SetCardBack(CollectablesManager.instance.GetCurrentCardBack()); // Set the current card back
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
        // Play flip sound
        PlayFlipSound();

        if (firstFlippedCard == null)
        {
            firstFlippedCard = card;
        }
        else if (secondFlippedCard == null)
        {
            secondFlippedCard = card;
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
    }

    private void CheckLevelCompletion()
    {
        if (UIManager.Instance.CurrentScore >= 60)
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
        // Reset timer
        timer.ResetTimer();
    }
}
