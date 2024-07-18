using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject cardPrefab;
    public Transform gridTransform;
    public Transform fullImageTransform; // Reference to the UI element to show full image
    public Image fullImageDisplay; // Image component to display the full image

    public Text scoreText;
    public Sprite[] cardImages;
    private Card firstFlippedCard;
    private Card secondFlippedCard;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateCards();
    }

    void GenerateCards()
    {
        List<Sprite> images = new List<Sprite>(cardImages);
        images.AddRange(cardImages); // Duplicate images for pairs
        images = ShuffleList(images);

        foreach (Sprite image in images)
        {
            GameObject cardObject = Instantiate(cardPrefab, gridTransform);
            Card card = cardObject.GetComponent<Card>();
            card.SetCardImage(image);
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
            // Here you can add code to disable or remove the matched cards
            fullImageDisplay.sprite = firstFlippedCard.cardFront;

            firstFlippedCard.SetMatched();
            secondFlippedCard.SetMatched();

            //firstFlippedCard.gameObject.SetActive(false);
            //secondFlippedCard.gameObject.SetActive(false);
        }
        else
        {
            // No match, flip cards back
            firstFlippedCard.ShowCardBack();
            secondFlippedCard.ShowCardBack();
        }

        firstFlippedCard = null;
        secondFlippedCard = null;
    }
}

//void CheckLevelCompletion()
// {
//     bool isMatched = true;

//     foreach (Card card in cards)
//     {
//         if (!card.isMatched)
//         {
//             isMatched = false;
//             break;
//         }
//     }

//     if (isMatched)
//     {
//         CollectablesManager.instance.OnLevelUp();
//     }
// }