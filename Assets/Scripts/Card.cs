using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Sprite cardFront; // Image of the card's front
    public Sprite cardBack; // Image of the card's back
    private Image image;
    private bool isFlipped = false;
    [SerializeField] private bool isMatched = false;

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
        if (!isFlipped)
        {
            ShowCardFront();
            GameManager.Instance.OnCardFlipped(this);
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
