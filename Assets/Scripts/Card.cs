
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    public Sprite cardFront;
    public Sprite cardBack;
    public Sprite fullImage;
    private Image image;
    private bool isFlipped = false;
    public bool isMatched = false;

    void Start()
    {
        image = GetComponent<Image>();
        ShowCardBack();

        // Add Button Click Listener
        GetComponent<Button>().onClick.AddListener(OnCardClicked);

    }

    public void SetCardImages(Sprite frontImage, Sprite fullImage)
    {
        cardFront = frontImage;
        this.fullImage = fullImage;
    }

    public void ShowCardFront()
    {
        image.sprite = cardFront;
        isFlipped = true;
    }

    public void ShowCardBack()
    {
        image.sprite = cardBack;
        isFlipped = false;
    }

    public void OnCardClicked()
    {

        if (!isFlipped)
        {
            ShowCardFront();
            GameManager.instance.OnCardFlipped(this);
            Debug.Log("Card flipped");

        }
    }


}
