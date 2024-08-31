
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace W3Labs.Utils
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Current;
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            Current = GetComponent<GameEvents>();

        }



        // public Action<string> OnShowMessageDialog = delegate (string message) { };
        // public void ShowMessageDialog(string message, bool isModal = false) => OnShowMessageDialog(message);

        // public Action<string, string, Action, string, bool, bool> OnShowMessageDailog1 = delegate (string message, string title, Action action, string actionText, bool isModal, bool isDarkBackGround) { };
        // public void ShowMessageDailog(string message, string title, Action action, string actionText, bool isModal = false, bool isDarkBackGround = false) => OnShowMessageDailog1(message, title, action, actionText, isModal, isDarkBackGround);


        public Action<string, string, Action, Action> OnShowMessageDailog = delegate (string message, string title, Action OnYes, Action OnNo) { };
        public void ShowMessageDailog(string message, string title, Action onYes, Action onNo) => OnShowMessageDailog(message, title, onYes, onNo);


        // Sounds Events  

        // public Action<string, string, Action, Action, bool, string, string, bool> OnShowMessageDailog3 = delegate (string message, string title, Action OnYes, Action OnNo, bool isModal, string _yesText, string _notext, bool isDarkBackGround) { };
        // public void ShowMessageDailog(string message, string title, Action onYes, Action onNo, string yesText, string noText, bool isModal = false, bool isDarkBackGround = false) => OnShowMessageDailog3(message, title, onYes, onNo, isModal, yesText, noText, isDarkBackGround);


    }


}