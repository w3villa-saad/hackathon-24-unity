using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using W3Labs.ViralRunner.Network;

namespace W3Labs
{
    public class AIInFluencerHandler : MonoBehaviour
    {
        [Header("TextGenerate")]
        [SerializeField] private TMP_InputField _promsText;
        [SerializeField] private Button _genrateTextBTN;
        [SerializeField] private GameObject _aiTextPanel;
        private TextPromsPOJO _texpromsPOJO;

        [Header("VideoGenerate")]
        [SerializeField] private TextMeshProUGUI _generatedtext;
        [SerializeField] private Button _createVideoBTN;
        //  private vi _texpromsPOJO;

        [Header("VideoCreated")]
        [SerializeField] private GameObject _videoPanel;
        [SerializeField] private VideoPlayer _videoPlayer;


        // [Header()]





        void Start()
        {
            _genrateTextBTN.onClick.AddListener(GenerateTextfromProms);
            _createVideoBTN.onClick.AddListener(GenerateVideoFromText);

        }
        void GenerateTextfromProms()
        {
            string promstextString = _promsText.text;
            Debug.Log(promstextString);
            APIHandlerW3labs.Instance.GenerateTextFromProms(promstextString, (status, data) =>
                   {
                       if (status)
                       {
                           _generatedtext.text = data.script_output;
                           _texpromsPOJO = data;
                           _aiTextPanel.SetActive(true);
                       }
                       else
                           Debug.Log("Value Not Set");

                   });

        }
        void GenerateVideoFromText()
        {
            string promstextString = _promsText.text;
            Debug.Log(promstextString);
            string id = _texpromsPOJO.id;
            _aiTextPanel.SetActive(true);
            APIHandlerW3labs.Instance.GenerateVideoFromText(_generatedtext.text, id, (status) =>
                   {
                       if (status)
                       {
                           _aiTextPanel.SetActive(false);
                           _videoPanel.SetActive(true);
                           StartCoroutine(GettingVideoLink());
                       }
                       else
                           Debug.Log("Value Not Set");

                   });

        }
        string _currentVideoPath = "";
        bool _isvideoLinkAvaible = false;
        IEnumerator GettingVideoLink()
        {
            // yield return new WaitWhile(() => isvideoLinkAvaible == true);
            while (_isvideoLinkAvaible == false)
            {
                APIHandlerW3labs.Instance.GetVideolink(_texpromsPOJO.id, (status, data) =>
                                   {
                                       if (status)
                                       {
                                           //    isvideoLinkAvaible = true;
                                           //    _videoPlayer.url = data.link;
                                           //    _videoPlayer.Play();
                                           _isvideoLinkAvaible = false;
                                           _currentVideoPath = Path.Combine(Application.persistentDataPath, _texpromsPOJO.id);
                                           StartCoroutine(DownloadAndPlayVideo(data.link, _currentVideoPath));

                                       }
                                       else
                                       {
                                           Debug.Log("Value Not Set");

                                       }

                                   });
                yield return new WaitForSeconds(5f);

            }



        }

        IEnumerator DownloadAndPlayVideo(string url, string path)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                // Send request and wait for a response
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error downloading video: {www.error}");
                }
                else
                {
                    // Save the video to a file
                    byte[] videoData = www.downloadHandler.data;
                    File.WriteAllBytes(path, videoData);
                    Debug.Log("Video downloaded successfully!");

                    // Play the video
                    PlayVideo(path);
                }
            }
        }

        void PlayVideo(string filePath)
        {
            // Set the VideoPlayer's source to the local file
            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = filePath;

            // Prepare and play the video
            _videoPlayer.Prepare();
            _videoPlayer.prepareCompleted += PreparedCompleted;
        }

        public void PreparedCompleted(VideoPlayer videoPlayer)
        {
            videoPlayer.Play();
        }

    }
}
