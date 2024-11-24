using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance;
    private GameData gameData;

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
        gameData = new GameData();

        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "FF1F3"; // Replace with your Title ID
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("No internet connection. Loading offline data");
            LoadOfflineData();
        }
        else
        {
            Login();
        }


    }

    private void Login()
    {
#if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = ReturnMobileID(),
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
#if UNITY_IOS
        var requestIOS = new LoginWithIOSDeviceIDRequest
        {
            DeviceId = ReturnMobileID(),
            CreateAccount = true
            };
        PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        LoadingScreen.instance.HideLoadingSpinner();

        if (result.NewlyCreated)
        {
            // If the user is new, load the EULA scene before the tutorial
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndUserLicense");
        }
        else
        {
            // If the user is not new, load the main game scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }

        //Optionally save server data locally for offline use
        SaveOnlineDataLocally();
    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.LogWarning("Falling back to offline data");
        LoadOfflineData();
    }

    private void LoadOfflineData()
    {
        gameData.LoadGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene"); // Load directly into the game
    }

    private void SaveOnlineDataLocally()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("HighScore"))
            {
                int serverHighScore = int.Parse(result.Data["HighScore"].Value);
                PlayerPrefs.SetInt("HighScore", serverHighScore);
                PlayerPrefs.Save();
                Debug.Log("Online data saved locally.");
            }
        },
        error =>
        {
            Debug.LogError("Failed to fetch user data: " + error.GenerateErrorReport());
        });
    }

    public static string ReturnMobileID()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }
}
