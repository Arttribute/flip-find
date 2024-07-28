using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "FF1F3";
        }
        //var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

        Login();

    }

    private void Login()
    {
#if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
#if UNITY_IOS
        var requestIOS = new LoginWithIOSDeviceIDRequest {DeviceId = ReturnMobileID(), CreateAccount = true};
        PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Login successful!");
        LoadingScreen.instance.HideLoadingSpinner();
        // CheckTutorialStatus();
        if (result.NewlyCreated)
        {
            TutorialManager.instance.StartTutorial();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }

    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void CheckTutorialStatus()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    private void OnDataReceived(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("TutorialCompleted"))
        {
            if (result.Data["TutorialCompleted"].Value == "true")
            {
                LoadMainScene();
            }
            else
            {
                LoadTutorial();
            }
        }
        else
        {
            LoadTutorial();
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport());
    }

    private void LoadTutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");

    }

    private void LoadMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void MarkTutorialAsCompleted()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "TutorialCompleted", "true" },
            }
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    private void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Tutorial completion status saved.");
    }



    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }
}