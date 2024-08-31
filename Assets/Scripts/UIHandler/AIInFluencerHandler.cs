using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
            string id = "";
            _aiTextPanel.SetActive(true);
            APIHandlerW3labs.Instance.GenerateVideoFromText(_generatedtext.text, id, (status, data) =>
                   {
                       if (status)
                       {
                           _aiTextPanel.SetActive(false);
                           _videoPanel.SetActive(true);
                       }
                       else
                           Debug.Log("Value Not Set");

                   });

        }
    }
}
