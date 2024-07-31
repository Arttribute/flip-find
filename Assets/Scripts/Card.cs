using UnityEngine;
using UnityEngine.SceneManagement;
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

    private CardAnim cardAnim; // Reference to CardAnim component

    private void Start()
    {
        image = GetComponent<Image>();
        cardAnim = GetComponent<CardAnim>(); // Get CardAnim component
        if (cardAnim == null)
        {
            Debug.LogError("CardAnim component is missing from this GameObject.");
        }

        ShowCardBack();
        GetComponent<Button>().onClick.AddListener(OnGameCardClicked);
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

    public void OnGameCardClicked()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (!isFlipped && !isMatched && !TutorialManager.instance.IsCheckingForMatch) // Check if match check is in progress
            {
                GetComponent<CardAnim>().Flip();
            }
        }
        else if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if (!isFlipped && !isMatched && !GameManager.Instance.IsCheckingForMatch) // Check if match check is in progress
            {
                GetComponent<CardAnim>().Flip();
            }
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
