using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectablesManager : MonoBehaviour
{
    public static CollectablesManager instance;
    public Image cardImage; // UI Image to display the new card face
    public GameObject unlockMessagePanel; // UI Panel for the unlock message
    public TextMeshProUGUI unlockMessageText; // Text component for the unlock message
    public Sprite[] cardFaces; // Array of card faces
    public TextMeshProUGUI scoreMessage; // Text component for the score message

    private int currentLevel = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
    }

    public void OnLevelUp()
    {
        currentLevel++;
        if (currentLevel < cardFaces.Length)
        {
            LoadCardFace();
            ShowUnlockMessage();
        }
    }

    private void LoadCardFace()
    {
        cardImage.sprite = cardFaces[currentLevel];
    }

    private void ShowUnlockMessage()
    {
        unlockMessageText.text = "New card face unlocked!";
        unlockMessagePanel.SetActive(true);
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
        Invoke("HideScoreMessage", 1.0f); // Hide the message after 1 second
    }

    private void HideScoreMessage()
    {
        scoreMessage.gameObject.SetActive(false);
    }
}
