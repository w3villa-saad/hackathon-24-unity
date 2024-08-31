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


        void Start()
        {
            _genrateTextBTN.onClick.AddListener(GenerateTextfromProms);

        }
        void GenerateTextfromProms()
        {
            string promstextString = _promsText.text;
            Debug.Log(promstextString);
            _aiTextPanel.SetActive(true);
            APIHandlerW3labs.Instance.GenerateTextFromProms(promstextString, (status, data) =>
                   {
                       if (status)
                       {
                           _aiTextPanel.SetActive(true);
                       }
                       else
                           Debug.Log("Value Not Set");

                   });

        }
    }
}
