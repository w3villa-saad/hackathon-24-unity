using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Authentication;

namespace W3Labs.Authentication
{
    public class AuthenticationUIHandler : MonoBehaviour
    {
        [SerializeField] private Button loginBTN;
        [SerializeField] private TextMeshProUGUI userIDText;
        [SerializeField] private Transform loginPanel;
        [SerializeField] private Transform userPanel;
        [SerializeField] private AuthManager authenticationManager;

        void OnEnable()
        {
            loginBTN.onClick.AddListener(OnClickLoginButton);
            authenticationManager.OnSginIned += OnHandleSignedIn;
            authenticationManager.OnSignInAnonymously += OnHandleSignedIn;
        }
        void OnDisable()
        {
            loginBTN.onClick.RemoveListener(OnClickLoginButton);
            authenticationManager.OnSginIned -= OnHandleSignedIn;
            authenticationManager.OnSignInAnonymously -= OnHandleSignedIn;
        }
        async void OnClickLoginButton()
        {
            authenticationManager.LoginStart();

        }
        void OnHandleSignedIn(PlayerInfo playerInfo, string playerName)
        {
            loginPanel.gameObject.SetActive(false);
            userPanel.gameObject.SetActive(true);
            userIDText.text = $"ID::{playerInfo.Id}";
            Debug.Log($" [Authentication]Player Name :: {playerName}");
            Debug.Log($" [Authentication]Player Name :: {playerInfo.Username}");
            Debug.Log($" [Authentication]Player Name :: {playerInfo.Identities}");



        }
        void OnHandleSignedIn(string playerId, string playerName)
        {
            loginPanel.gameObject.SetActive(false);
            userPanel.gameObject.SetActive(true);
            userIDText.text = $"ID::{playerId}";
            Debug.Log($" [Authentication]Player Name :: {playerName}");
            Debug.Log($" [Authentication]Player Name :: {playerId}");
        }

    }

    public enum PlayerData
    {
        PlayerToken,
        PlayerID
    }
}
