using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CardAnim : MonoBehaviour
{
    private Card card;
    private bool isAnimating = false;

    public void Init(Card card)
    {
        this.card = card;
    }

    public void Flip()
    {
        if (isAnimating || card.IsMatched) return;

        isAnimating = true;
        transform.DOScaleX(0, 0.1f).OnComplete(() =>
        {
            card.ShowCardFront();
            transform.DOScaleX(1, 0.1f).OnComplete(() =>
            {
                isAnimating = false;
                if (SceneManager.GetActiveScene().name == "MainScene")
                {
                    GameManager.Instance.OnCardFlipped(card);
                }
                else
                {
                    TutorialManager.instance.OnCardFlipped(card);
                }


            });
        });
    }

    public void FlipBack()
    {
        if (isAnimating || card.IsMatched) return;

        isAnimating = true;
        transform.DOScaleX(0, 0.1f).OnComplete(() =>
        {
            card.ShowCardBack();
            transform.DOScaleX(1, 0.1f).OnComplete(() =>
            {
                isAnimating = false;
            });
        });
    }
}
