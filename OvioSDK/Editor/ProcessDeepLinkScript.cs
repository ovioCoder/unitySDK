using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProcessDeepLinkScript : MonoBehaviour
{
    public string deeplinkURL;

    void Update()
    {
       
    }

    void Awake()
    {
        if (!string.IsNullOrEmpty(Application.absoluteURL))
        {
            Debug.Log("Launched with deep link. " + Application.absoluteURL);
            HandleLaunchURL(new Uri(Application.absoluteURL));
        }

        Application.deepLinkActivated += OnDeepLinkActivated;
    }

    void OnDeepLinkActivated(string link)
    {
        Debug.Log("Resumed with deep link. " + link);
        HandleLaunchURL(new Uri(link));
    }

    async void HandleLaunchURL(Uri link)
    {
        // Deep link logic here
        Debug.Log($"{nameof(HandleLaunchURL)}");
        CallbackOvioApplication();
    }

    private void CallbackOvioApplication()
    {
        string url = $"ovio://{Application.identifier}?id={ReadScript.UserId}";

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriData = uriClass.CallStatic<AndroidJavaObject>("parse", url);

        AndroidJavaObject intent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", ReadScript.OvioApplicationIdentifier);

        intent.Call<AndroidJavaObject>("setAction", "android.intent.action.VIEW");
        intent.Call<AndroidJavaObject>("setData", uriData);

        currentActivity.Call("startActivity", intent);
    }
}
