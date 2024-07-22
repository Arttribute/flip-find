using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
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

#if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
#if UNITY_IOS
        var requestIOS = new LoginWithIOSDeviceIDRequest {DeviceId = ReturnMobileID(), CreateAccount = true};
        PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Player is logged in!");

    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }
}