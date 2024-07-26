using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject tutorialPointer;
    [SerializeField] private GameObject tutorialMessage;

    private Card card;


    void Awake()
    {
        card = GetComponent<Card>();
    }

    void Start()
    {

        tutorialPointer.SetActive(true);
        tutorialMessage.SetActive(true);
        tutorialText.text = "Flip the indicated card";
    }

    void Update()
    {
        OnCardFlip();
    }

    private void OnCardFlip()
    {
        Card[] allCards = FindObjectsOfType<Card>();

        foreach (Card card in allCards)
        {
            if (card.IsFlipped)
            {
                tutorialPointer.SetActive(false);
                tutorialText.text = "Great! Now flip another card";

                if (card.IsMatched)
                {
                    tutorialText.text = "Tutorial Complete! Flip all cards";
                    break;
                }

            }


        }



    }

    private void OnTutorialComplete()
    {
        //PlayFabManager.Instance.MarkTutorialAsCompleted();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
