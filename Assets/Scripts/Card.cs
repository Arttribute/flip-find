using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite cardFront; // Image of the card's front
    public Sprite cardBack; // Image of the card's back
    public bool IsFlipped => isFlipped;
    public bool IsMatched => isMatched;
    private Image image;
    private bool isFlipped = false;
    private bool isMatched = false;

    private void Start()
    {
        image = GetComponent<Image>();
        ShowCardBack();
        GetComponent<Button>().onClick.AddListener(OnCardClicked);
    }

    public void SetCardImage(Sprite frontImage)
    {
        cardFront = frontImage;
    }

    public void SetCardBack(Sprite backImage)
    {
        cardBack = backImage;
    }

    public void ShowCardFront()
    {
        if (!isMatched)
        {
            image.sprite = cardFront;
            isFlipped = true;
        }
    }

    public void ShowCardBack()
    {
        if (!isMatched)
        {
            image.sprite = cardBack;
            isFlipped = false;
        }
    }

    public void OnCardClicked()
    {
        if (!isFlipped && !isMatched && !GameManager.Instance.IsCheckingForMatch) // Check if match check is in progress
        {
            GetComponent<CardAnim>().Flip();
        }
    }

    public void SetMatched()
    {
        isMatched = true;
        Color newColor;
        ColorUtility.TryParseHtmlString("#1A0033", out newColor);
        image.sprite = null;
        image.color = newColor;
    }
}
