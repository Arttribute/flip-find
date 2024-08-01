using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CollectablesManager : MonoBehaviour
{
    public static CollectablesManager instance;
    public int CurrentLevel => currentLevel;
    public Image cardImage; // UI Image to display the new card face
    public GameObject unlockMessagePanel; // UI Panel for the unlock message
    public TextMeshProUGUI unlockMessageText; // Text component for the unlock message
    public Sprite[] cardFaces; // Array of card faces
    public TextMeshProUGUI scoreMessage; // Text component for the score message
    public AudioClip unlockSound; // Reference to the unlock sound AudioClip
    public AudioClip scoreSound; // Reference to the score sound AudioClip

    private AudioSource audioSource; // AudioSource component to play the sounds
    private int currentLevel = 0;
    private Sprite currentCardBack = null; // To store the current card back

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadCardFace();
        HideScoreMessage();
        // currentCardBack = cardFaces[currentLevel]; // Do not set the card back initially
    }

    public void OnLevelUp()
    {
        currentLevel++;
        if (currentLevel < cardFaces.Length)
        {
            LoadCardFace();
            currentCardBack = cardFaces[currentLevel]; // Update the card back
            ShowUnlockMessage();
        }
    }

    private void LoadCardFace()
    {
        cardImage.sprite = cardFaces[currentLevel];

    }

    public Sprite GetCurrentCardBack()
    {
        return currentCardBack;
    }

    private void ShowUnlockMessage()
    {
        unlockMessageText.text = "New card face unlocked!";
        unlockMessagePanel.SetActive(true);
        PlayUnlockSound();
        AnimateCard();
        Invoke("HideUnlockMessage", 3.0f); // Hide the message after 3 seconds
    }

    private void HideUnlockMessage()
    {
        unlockMessagePanel.SetActive(false);
    }

    public void ShowScoreMessage()
    {
        scoreMessage.text = "+10";
        scoreMessage.gameObject.SetActive(true);

        // Animate the score message from left to right
        Vector3 startPosition = new Vector3(-Screen.width, scoreMessage.rectTransform.anchoredPosition.y, 0);
        Vector3 endPosition = new Vector3(0, scoreMessage.rectTransform.anchoredPosition.y, 0);

        scoreMessage.rectTransform.anchoredPosition = startPosition;
        scoreMessage.rectTransform.DOAnchorPos(endPosition, 1.0f).SetEase(Ease.OutCubic);

        PlayScoreSound();
        Invoke("HideScoreMessage", 1.0f); // Hide the message after 1 second
    }

    private void HideScoreMessage()
    {
        scoreMessage.gameObject.SetActive(false);
    }

    private void AnimateCard()
    {
        // Animate the card image to move from left to center
        Vector3 startPosition = new Vector3(-Screen.width / 2, cardImage.rectTransform.anchoredPosition.y, 0);
        Vector3 endPosition = new Vector3(0, cardImage.rectTransform.anchoredPosition.y, 0);

        cardImage.rectTransform.anchoredPosition = startPosition;
        cardImage.rectTransform.DOAnchorPos(endPosition, 1.0f).SetEase(Ease.Linear);
    }

    private void PlayUnlockSound()
    {
        if (unlockSound != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }
    }

    private void PlayScoreSound()
    {
        if (scoreSound != null)
        {
            audioSource.PlayOneShot(scoreSound);
        }
    }
}
