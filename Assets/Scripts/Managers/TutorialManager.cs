using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    private Timer timer;
    private Card firstFlippedCard;
    private Card secondFlippedCard;

    [SerializeField] private bool isTutorialCompleted;

    private Card card;


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

    }

    void Start()
    {


        tutorialPointer.SetActive(true);
        tutorialMessage.SetActive(true);
        tutorialText.text = "Flip the indicated card";
        GenerateCards();
        timer.ResetTimer();
    }

    void Update()
    {
        OnCardFlip();
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

            //CheckLevelCompletion();
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

    public void StartTutorial()
    {
        isTutorialCompleted = false;
    }

    private void OnTutorialComplete()
    {
        isTutorialCompleted = true;
        PlayFabManager.Instance.MarkTutorialAsCompleted();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
