using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject cardPrefab;
    public Transform gridTransform;
    public Transform fullImageTransform;
    public Image fullImageDisplay;
    public Text scoreText;
    public Sprite[] cardImages;
    public Sprite[] fullImages;
    private List<Card> cards = new List<Card>();
    private Card firstFlippedCard;
    private Card secondFlippedCard;
    private int score = 0;

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
        GenerateCards();
    }

    void GenerateCards()
    {
        List<Sprite> splitImages = new List<Sprite>(cardImages);
        List<Sprite> correspondingFullImages = new List<Sprite>(fullImages);
        List<int> usedIndexes = new List<int>();

        //List<Sprite> images = new List<Sprite>();
        //images.AddRange(cardImages); // Duplicate images for pairs
        //images.AddRange(cardImages);

        // Shuffle images
        //images = ShuffleList(images);

        // for (int i = 0; i < images.Count; i++)
        // {
        //     GameObject cardObject = Instantiate(cardPrefab, gridTransform);
        //     Card card = cardObject.GetComponent<Card>();
        //     card.SetCardImage(images[i]);
        //     cards.Add(card);
        // }

        foreach (Sprite image in cardImages)
        {
            int index = Random.Range(0, correspondingFullImages.Count);
            while (usedIndexes.Contains(index))
            {
                index = Random.Range(0, correspondingFullImages.Count);
            }
            usedIndexes.Add(index);

            Sprite fullImage = correspondingFullImages[index];
            CreateCard(image, fullImage);
            CreateCard(image, fullImage);
        }
    }

    void CreateCard(Sprite frontImage, Sprite fullImage)
    {
        GameObject cardObject = Instantiate(cardPrefab, gridTransform);
        Card card = cardObject.GetComponent<Card>();
        card.SetCardImages(frontImage, fullImage);
    }

    public void OnCardFlipped(Card flippedCard)
    {
        if (firstFlippedCard == null)
        {
            firstFlippedCard = flippedCard;
        }
        else if (secondFlippedCard == null)
        {
            secondFlippedCard = flippedCard;
            StartCoroutine(CheckForMatch());
        }
    }

    IEnumerator CheckForMatch()
    {
        yield return new WaitForSeconds(1);

        if (firstFlippedCard.fullImage == secondFlippedCard.fullImage)
        {
            // Match found, show the full image
            fullImageDisplay.sprite = firstFlippedCard.fullImage;
            score += 25;
            scoreText.text = "Score: " + score;
            /*UIManager.Instance.AddScore(10); // Add points for a correct match
             CheckLevelCompletion();*/
        }
        else
        {
            firstFlippedCard.ShowCardBack();
            secondFlippedCard.ShowCardBack();
        }

        firstFlippedCard = null;
        secondFlippedCard = null;
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
    /*void CheckLevelCompletion()
    {
        bool isMatched = true;

        foreach (Card card in cards)
        {
            if (!card.isMatched)
            {
                isMatched = false;
                break;
            }
        }

        if (isMatched)
        {
            CollectablesManager.instance.OnLevelUp();
        }
    }*/
}
