using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

namespace W3Labs.Authentication
{
    public class AuthManager : MonoBehaviour
    {

        private PlayerInfo _currentPlayerInfo;

        public event Action<PlayerInfo, string> OnSignInSuccessfully;
        public event Action<PlayerInfo, string, string> OnSginIned;
        public event Action<string, string> OnSignInAnonymously;
        async void Awake()
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($" [Authentication]Request failed: {e}");
            }

            PlayerAccountService.Instance.SignedIn += SignedIn;

        }
        private async void SignedIn()
        {
            try
            {
                var accessToken = PlayerAccountService.Instance.AccessToken;
                Debug.Log($" [Authentication]Token:: {accessToken}");
                await SignInWithUnityAsync(accessToken);

            }
            catch (Exception ex)
            {
                Debug.Log($" [Authentication]Exception:: {ex}");

            }
        }

        async public void LoginStart()
        {
            if (PlayerAccountService.Instance.AccessToken == null)
                await PlayerAccountService.Instance.StartSignInAsync();
            else
            {
                await SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            }
        }

        async Task SignInWithUnityAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);

                Debug.Log(" [Authentication] SignIn is successful." + AuthenticationService.Instance.PlayerInfo.Id);
                _currentPlayerInfo = AuthenticationService.Instance.PlayerInfo;
                PlayerPrefs.SetString(PlayerData.PlayerToken.ToString(), accessToken);
                PlayerPrefs.SetString(PlayerData.PlayerID.ToString(), AuthenticationService.Instance.PlayerInfo.Id);
                var name = await AuthenticationService.Instance.GetPlayerNameAsync();
                OnSginIned?.Invoke(_currentPlayerInfo, name, accessToken);

            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogError($" [Authentication]Authentication failed: {ex}");
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogError($" [Authentication]Request failed: {ex}");
            }
        }


    }

}
