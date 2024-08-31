using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class Instagram: MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_InputField field;

    private void Start() {
        button.onClick.AddListener(()=>{ShareToInstagram("@"+field.text);});
    }
    public void ShareToInstagram(string videoPath)
    {
        Debug.Log(videoPath);
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "video/mp4");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), videoPath);

        AndroidJavaClass packageManagerClass = new AndroidJavaClass("android.content.pm.PackageManager");
        AndroidJavaObject packageManagerObject = new AndroidJavaObject("android.content.pm.PackageManager");
        AndroidJavaObject resolveInfo = packageManagerObject.Call<AndroidJavaObject>("resolveActivity", intentObject, packageManagerClass.GetStatic<int>("MATCH_DEFAULT"));
        Debug.Log("Kuch hua hai??");
        if (resolveInfo != null)
        {
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("startActivity", intentObject);
        }
    }
}