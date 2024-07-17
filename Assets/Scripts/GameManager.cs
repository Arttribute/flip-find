using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject cardPrefab;
    public Transform gridTransform;
    public Sprite[] cardImages;
    private List<Card> cards = new List<Card>();
    private Card firstFlippedCard;
    private Card secondFlippedCard;

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
        List<Sprite> images = new List<Sprite>();
        images.AddRange(cardImages); // Duplicate images for pairs
        images.AddRange(cardImages);

        // Shuffle images
        images = ShuffleList(images);

        for (int i = 0; i < images.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, gridTransform);
            Card card = cardObject.GetComponent<Card>();
            card.SetCardImage(images[i]);
            cards.Add(card);
        }
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

        if (firstFlippedCard.cardFront == secondFlippedCard.cardFront)
        {
            firstFlippedCard.isMatched = true;
            secondFlippedCard.isMatched = true;
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
