
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    public Sprite cardFront;
    public Sprite cardBack;
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

    public void SetCardImage(Sprite frontImage)
    {
        cardFront = frontImage;
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
