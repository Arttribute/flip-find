using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;

    [SerializeField] private GameObject loadingSpinner;

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

    private void Start()
    {
        ShowLoadingSpinner();

    }

    private void ShowLoadingSpinner()
    {
        if (loadingSpinner != null)
        {
            loadingSpinner.SetActive(true);
        }
    }

    public void HideLoadingSpinner()
    {
        if (loadingSpinner != null)
        {
            loadingSpinner.SetActive(false);
        }
    }
}