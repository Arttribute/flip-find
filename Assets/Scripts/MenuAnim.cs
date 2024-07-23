using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuAnim : MonoBehaviour
{
    [SerializeField] private RectTransform menuTransform;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Vector2 hiddenPosition = new Vector2(-Screen.width, 0); // Initially off-screen to the left
    [SerializeField] private Vector2 shownPosition = new Vector2(0, 0); // Centered position

    private bool isMenuShown = false;

    void Start()
    {
        // Set the initial position of the menu to be hidden
        menuTransform.anchoredPosition = hiddenPosition;
    }

    public void ToggleMenu()
    {
        if (isMenuShown)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
        isMenuShown = !isMenuShown;
    }

    public void ShowMenu()
    {
        menuTransform.DOAnchorPos(shownPosition, animationDuration).SetEase(Ease.OutCubic);
    }

    public void HideMenu()
    {
        menuTransform.DOAnchorPos(hiddenPosition, animationDuration).SetEase(Ease.InCubic);
    }
}
