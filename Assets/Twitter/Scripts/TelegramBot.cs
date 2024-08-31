using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.IO;
using W3Labs;

public class TelegramBot : MonoBehaviour
{
    [SerializeField] Button button , _downloadButton;

    [SerializeField] AIInFluencerHandler AIInfluence;
    private string botToken = "7034801212:AAEoF6U9A1IU7xaFGgNqPXD9NsVcfNnLq9E"; // Replace with your bot token
    private string channelUsername = "@AIInfluencerTest"; // Replace with your channel username (must start with @)

    private void Start() {
        // SendMessageToChannel(@"C:\Users\vaibh\Desktop\Yo.mp4");
        Debug.Log(AIInfluence._currentVideoPath);
        string filePath = Path.Combine(AIInfluence._currentVideoPath);
        button.onClick.AddListener(()=>{
                SendMessageToChannel(filePath);
            });
        _downloadButton.onClick.AddListener(()=>{
            DownloadVideo();
        });
    }
    void DownloadVideo(){
        string savePath = Path.Combine(Application.persistentDataPath, AIInfluence._currentVideoPath);
         byte[] videoData = GetVideoData(); // Replace this with your actual video data retrieval method

        // Write the video data to the file
        try
        {
            File.WriteAllBytes(savePath, videoData);
            Debug.Log("Video saved successfully at: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save video: " + e.Message);
        }
    }
    byte[] GetVideoData()
    {
        // For demonstration purposes, create a dummy byte array
        // Replace this with actual video file reading or download logic
        byte[] videoData = new byte[1000]; // Replace with actual data
        return videoData;
    }
    public void SendMessageToChannel(string videoPath)
    {
        // StartCoroutine(SendMessageCoroutine(videoPath));
        StartCoroutine(SendVideoCoroutine(videoPath));
    }

    private IEnumerator SendMessageCoroutine(string message)
    {
        string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={channelUsername}&text={UnityWebRequest.EscapeURL(message)}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Message sent successfully.");
            }
            else
            {
                Debug.LogError($"Error sending message: {request.error}");
            }
        }
        
    }
    private IEnumerator SendVideoCoroutine(string videoPath)
    {
        byte[] videoData = System.IO.File.ReadAllBytes(videoPath);

        WWWForm form = new WWWForm();
        form.AddField("chat_id", channelUsername);
        form.AddBinaryData("video", videoData, System.IO.Path.GetFileName(videoPath), "video/mp4");

        string url = $"https://api.telegram.org/bot{botToken}/sendVideo";

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Video sent successfully.");
            }
            else
            {
                Debug.LogError($"Error sending video: {request.error}");
            }
        }
    }
}
