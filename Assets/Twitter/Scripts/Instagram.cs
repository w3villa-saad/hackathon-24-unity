using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class Instagram : MonoBehaviour
{
    [SerializeField] Button button;

    private void Start()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Yo.mp4");
        button.onClick.AddListener(() => { 
                ShareVideo(filePath); 
            });
    }
    public void ShareVideo(string videoPath)
    {
        Debug.Log(videoPath);
        if (File.Exists(videoPath))
        {
            new NativeShare().AddFile(videoPath).SetSubject("Share the video").SetText("Your own AI influencer").Share();
        }
        else
        {
            Debug.Log("Video file n found");
        }
    }

    // public void ShareVideo(string videoPath)
    // {
    //     AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
    //     AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

    //     intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
    //     intentObject.Call<AndroidJavaObject>("setType", "video/*");

    //     AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", videoPath);
    //     AndroidJavaObject uriObject = new AndroidJavaClass("androidx.core.content.FileProvider")
    //         .CallStatic<AndroidJavaObject>("getUriForFile", new AndroidJavaObject("com.unity3d.player.UnityPlayer")
    //         .GetStatic<AndroidJavaObject>("currentActivity"), Application.identifier + ".fileprovider", fileObject);

    //     intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

    //     AndroidJavaObject unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
    //         .GetStatic<AndroidJavaObject>("currentActivity");
    //     unityActivity.Call("grantUriPermission", "com.example.package.name", uriObject, 1);

    //     unityActivity.Call("startActivity", intentObject);
    // }

    // public void ShareToInstagram(string videoPath)
    // {
    //     try
    //     {
    //         Debug.Log(videoPath);

    //         AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
    //         AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
    //         intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
    //         intentObject.Call<AndroidJavaObject>("setType", "video/mp4");

    //         AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

    //         AndroidJavaClass fileProviderClass = new AndroidJavaClass("androidx.core.content.FileProvider");
    //         string authority = activity.Call<string>("getPackageName") + ".fileprovider";
    //         AndroidJavaObject uriObject = fileProviderClass.CallStatic<AndroidJavaObject>("getUriForFile", activity, authority, new AndroidJavaObject("java.io.File", videoPath));

    //         intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
    //         intentObject.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION"));

    //         AndroidJavaObject packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
    //         AndroidJavaObject resolveInfo = packageManager.Call<AndroidJavaObject>("resolveActivity", intentObject, 0);

    //         if (resolveInfo != null)
    //         {
    //             activity.Call("startActivity", intentObject);
    //         }
    //         else
    //         {
    //             Debug.Log("No app available to handle this intent.");
    //         }
    //     }
    //     catch (System.Exception ex)
    //     {
    //         Debug.Log("Error occurred: " + ex.Message);
    //     }
    // }
}
