using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
namespace W3Labs.Authentication
{
    public class AuthenticationManager : MonoBehaviour
    {
        private PlayerInfo _currentPlayerInfo;

        public event Action<PlayerInfo, string> OnSignInSuccessfully;
        public event Action<PlayerInfo, string> OnSginIned;
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
        // void Start()
        // {
        //     appleAuthenticationManger.OnSginInEdWithApple += OnHandleSignedWithApple;
        // }
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
        public void InitSignIn()
        {

            SignedIn();

        }


        public void OnClickSignInAnonymously()
        {
            SignInAnonymouslyAsync();
        }

        private async void SignInAnonymouslyAsync()
        {
            try
            {
                if (AuthenticationService.Instance.IsSignedIn)//|| AuthenticationService.Instance.SessionTokenExists)
                {
                    Debug.Log($" [Authentication]  Player is already signed in. Player ID: {AuthenticationService.Instance.PlayerId}");
                    // LoadPlayerData(AuthenticationService.Instance.PlayerId);
                }
                else
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    Debug.Log($" [Authentication] Player signed in anonymously. Player ID: {AuthenticationService.Instance.PlayerId}");
                    SavePlayerData(AuthenticationService.Instance.PlayerId);
                }

                Debug.Log($" [Authentication] Player ID: {AuthenticationService.Instance.PlayerId}");
                // _currentPlayer.PlayerToken = AuthenticationService.Instance.AccessToken;
                Debug.Log($" [Authentication] Player Token: {_currentPlayerInfo.Id}");
                _currentPlayerInfo = AuthenticationService.Instance.PlayerInfo;
                // _currentPlayer.PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                // currentPlayer.PlayerInfo = AuthenticationService.Instance.PlayerInfo;
                OnSignInSuccessfully?.Invoke(_currentPlayerInfo, AuthenticationService.Instance.AccessToken);
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError($" [Authentication] Failed to sign in anonymously: {ex}");
            }
            catch (RequestFailedException ex)
            {
                Debug.LogError($" [Authentication]Request failed: {ex}");
            }
        }


        private static void SavePlayerData(string playerId)
        {
            // Implement your logic to save player data using the playerId
            PlayerPrefs.SetString("PlayerID", playerId);
            // Save other player-specific data as needed
        }
        private static void LoadPlayerData(string playerId)
        {
            // Implement your logic to load player data using the playerId
            if (PlayerPrefs.HasKey("PlayerID"))
            {
                string savedPlayerId = PlayerPrefs.GetString("PlayerID");
                if (savedPlayerId == playerId)
                {
                    // Load the saved player data
                    Debug.Log($"[Authentication] ::Loaded data for Player ID: {savedPlayerId}");
                }
                else
                {
                    Debug.LogWarning($" [Authentication]::Player ID mismatch. Expected: {playerId}, Found: {savedPlayerId}");
                }
            }
            else
            {
                Debug.LogWarning($" [Authentication]::No saved data found for Player ID: {playerId}");
            }
        }
        #region  Unity Login 
        async Task SignInWithUnityAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
                Debug.Log(" [Authentication] SignIn is successful.");
                _currentPlayerInfo = AuthenticationService.Instance.PlayerInfo;
                var name = await AuthenticationService.Instance.GetPlayerNameAsync();
                OnSginIned?.Invoke(_currentPlayerInfo, name);
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
        async Task LinkWithUnityAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithUnityAsync(accessToken);
                Debug.Log("Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");
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
        async Task UnlinkUnityAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.UnlinkUnityAsync();
                Debug.Log("Unlink is successful.");
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
        #endregion

        void OnDestroy()
        {
            PlayerAccountService.Instance.SignedIn -= SignedIn;
            //appleAuthenticationManger.OnSginInEdWithApple -= OnHandleSignedWithApple;
        }
        #region  Google Login
        // void InitializePlayGamesLogin()
        // {
        //     var config = new PlayGamesClientConfiguration.Builder()
        //         // Requests an ID token be generated.  
        //         // This OAuth token can be used to
        //         // identify the player to other services such as Firebase.
        //         .RequestIdToken()
        //         .Build();

        //     PlayGamesPlatform.InitializeInstance(config);
        //     PlayGamesPlatform.DebugLogEnabled = true;
        //     PlayGamesPlatform.Activate();
        // }
        // void LoginGoogle()
        // {
        //     Social.localUser.Authenticate(OnGoogleLogin);
        // }
        // void OnGoogleLogin(bool success)
        // {
        //     if (success)
        //     {
        //         // Call Unity Authentication SDK to sign in or link with Google.
        //         Debug.Log("Login with Google done. IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
        //     }
        //     else
        //     {
        //         Debug.Log("Unsuccessful login");
        //     }
        // }
        async Task SignInWithGoogleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithGoogleAsync(idToken);
                Debug.Log(" [Authentication] ::SignIn is successful.");
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
        async Task LinkWithGoogleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithGoogleAsync(idToken);
                Debug.Log(" [Authentication] :: Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError(" [Authentication] :: This user is already linked with another account. Log in instead.");
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
        async Task UnlinkGoogleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.UnlinkGoogleAsync();
                Debug.Log(" [Authentication] :: Unlink is successful.");
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
        #endregion

        #region  Apple Login
        // public void Initialize()
        // {
        //     var deserializer = new PayloadDeserializer();
        //     m_AppleAuthManager = new AppleAuthManager(deserializer);
        // }
        // public void Update()
        // {
        //     if (m_AppleAuthManager != null)
        //     {
        //         m_AppleAuthManager.Update();
        //     }
        // }
        // public void LoginToApple()
        // {
        //     // Initialize the Apple Auth Manager
        //     if (m_AppleAuthManager == null)
        //     {
        //         Initialize();
        //     }

        //     // Set the login arguments
        //     var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        //     // Perform the login
        //     m_AppleAuthManager.LoginWithAppleId(
        //         loginArgs,
        //         credential =>
        //         {
        //             var appleIDCredential = credential as IAppleIDCredential;
        //             if (appleIDCredential != null)
        //             {
        //                 var idToken = Encoding.UTF8.GetString(
        //                     appleIDCredential.IdentityToken,
        //                     0,
        //                     appleIDCredential.IdentityToken.Length);
        //                 Debug.Log("Sign-in with Apple successfully done. IDToken: " + idToken);
        //                 Token = idToken;
        //             }
        //             else
        //             {
        //                 Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
        //                 Error = "Retrieving Apple Id Token failed.";
        //             }
        //         },
        //         error =>
        //         {
        //             Debug.Log("Sign-in with Apple error. Message: " + error);
        //             Error = "Retrieving Apple Id Token failed.";
        //         }
        //     );
        // }
        public async void OnHandleSignedWithApple(string idToken)
        {
            await SignInWithAppleAsync(idToken);
        }
        async Task SignInWithAppleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithAppleAsync(idToken);
                Debug.Log(" [Authentication] :: SignIn is successful.");
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
        async Task LinkWithAppleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithAppleAsync(idToken);
                Debug.Log(" [Authentication] :: Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError(" [Authentication] :: This user is already linked with another account. Log in instead.");
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
        async Task UnlinkAppleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.UnlinkAppleAsync();
                Debug.Log(" [Authentication] :: Unlink is successful.");
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


        #endregion
        #region  Google Play Game login
        async Task SignInWithGooglePlayGamesAsync(string authCode)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
                Debug.Log("SignIn is successful.");
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
        async Task LinkWithGooglePlayGamesAsync(string authCode)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
                Debug.Log("Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");
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
        async Task UnlinkGooglePlayGamesAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.UnlinkGooglePlayGamesAsync();
                Debug.Log("Unlink is successful.");
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

        #endregion
        #region Apple Game Center login
        async Task SignInWithAppleGameCenterAsync(string signature, string teamPlayerId, string publicKeyURL, string salt, ulong timestamp)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
                Debug.Log("SignIn is successful.");
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
        async Task LinkWithAppleGameCenterAsync(string signature, string teamPlayerId, string publicKeyURL, string salt, ulong timestamp)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
                Debug.Log("Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");
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
        async Task UnlinkAppleGameCenterAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.UnlinkAppleGameCenterAsync();
                Debug.Log("Unlink is successful.");
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
        #endregion
    }
}