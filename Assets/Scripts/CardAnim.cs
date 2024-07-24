using UnityEngine;
using DG.Tweening;

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
        transform.DOScaleX(0, 0.2f).OnComplete(() =>
        {
            card.ShowCardFront();
            transform.DOScaleX(1, 0.2f).OnComplete(() =>
            {
                isAnimating = false;
                GameManager.Instance.OnCardFlipped(card);
            });
        });
    }

    public void FlipBack()
    {
        if (isAnimating || card.IsMatched) return;

        isAnimating = true;
        transform.DOScaleX(0, 0.2f).OnComplete(() =>
        {
            card.ShowCardBack();
            transform.DOScaleX(1, 0.2f).OnComplete(() =>
            {
                isAnimating = false;
            });
        });
    }
}
