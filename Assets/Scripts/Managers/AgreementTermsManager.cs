using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AgreementTermsManager : MonoBehaviour
{
    public GameObject eulaPanel; // Reference to the UI panel containing the EULA
    public Button acceptButton; // Reference to the Accept button

    private string privacyPolicyURL = "https://docs.google.com/document/d/1_2Tv-ANSQsxYodttS2jy8qqx_3Pf6bhVPtXPAkzFWIs/edit?tab=t.0";
    private bool isPrivacyPolicyOpened = false;

    void Start()
    {
        ShowEULA();
        acceptButton.interactable = false;
    }

    public void openPrivacyPolicy()
    {
        isPrivacyPolicyOpened = true;
        Application.OpenURL(privacyPolicyURL);
        EnableAcceptButton();

    }

    private void EnableAcceptButton()
    {
        if (isPrivacyPolicyOpened)
        {
            acceptButton.interactable = true;
        }
    }

    private void ShowEULA()
    {
        eulaPanel.SetActive(true); // Show the EULA panel
        acceptButton.onClick.AddListener(OnAccept);
    }

    private void OnAccept()
    {
        eulaPanel.SetActive(false); // Hide the EULA panel
        SceneManager.LoadScene("Tutorial"); // Load the tutorial scene
    }

    private void OnDecline()
    {
        // Handle decline, for example by exiting the game
        Application.Quit(); // Exit the game
    }
}
