using System;
using System.Collections.Generic;
using SuperOne.Utils;
using UnityEngine;
using W3Labs.RemoteConfig;
using W3Labs.Utils;


namespace W3Labs.ViralRunner.Network
{
    public class APIHandlerW3labs : SingletonMonoBehaviour<APIHandlerW3labs>
    {

        public static APIHandlerW3labs Current;
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            Current = GetComponent<APIHandlerW3labs>();

        }
        [SerializeField] ApiClient _apiClient;
        // public static APIResponse<LeaderBoardDataPOJO> leaderBoardDataPOJO = new APIResponse<LeaderBoardDataPOJO>();
        private Dictionary<string, object> _postDict = new Dictionary<string, object>();

        public static GettingVideoLink leaderBoardDataPOJO = new GettingVideoLink();
        // string baseURL = $"https://3yss3qru2i56mu27zr3o5574vy0lloea.lambda-url.us-west-2.on.aws/api/";
        string baseURL => $"https://codekenawabs.edully.com/v1/api/";
        // string baseURL = $"https://7936-136-232-130-202.ngrok-free.app/api/";
        public async void GetVideolink(Action<bool, GettingVideoLink> actinOnResponse)
        {

            var url = "https://codekenawabs.edully.com/v1/api/history/video_status/1";
            var res = await _apiClient.Get<APIResponse<GettingVideoLink>>(url);
            if (res != null && res.status == true)
            {

                // Debug.Log("Data ::" + res.data + res.success);
                actinOnResponse?.Invoke(true, res.data);
            }
            else
            {
                //  Debug.Log("Data ::" + res.success);
                actinOnResponse?.Invoke(false, null);
            }
        }
        public async void GetUserDetails(string userID, Action<bool, PlayerDetailsPOJO> actinOnResponse)
        {

            var url = baseURL + "getUserDetails?userId=" + userID;
            var res = await _apiClient.Get<APIResponse<PlayerDetailsPOJO>>(url);

            if (res != null && res.status == true)
            {
                actinOnResponse?.Invoke(true, res.data);
            }
            else if (res.status == false)
                actinOnResponse?.Invoke(false, null);

            else
            {
                actinOnResponse?.Invoke(false, null);
            }
        }
        public async void AddUser(string username, string email, Action<bool, UserInfo> actionOnResponse)
        {
            var url = baseURL + "user/create";
            int currentMode = (int)GameConstant.CurrentGameMode;
            _postDict.Clear();
            // _postDict.Add("name", username);
            // _postDict.Add("email", email);
            _postDict.Add("player_id", PlayerPrefs.GetString("PlayerID"));
            _postDict.Add("token", PlayerPrefs.GetString("PlayerToken"));
            //  _postDict.Add("playerId", playerID);

            // _postDict.Add(GameConstant.PlayerCurrentGameMode, currentMode.ToString());
            var postData = MySerializer.Serialize(_postDict);
            //  string postData = "{\"username\":\"" + username + "\",\"country\":\"" + country + "\"}";

            Debug.Log($"{url} : {postData}");
            var res = await _apiClient.Post<APIResponse<UserInfo>>(url, postData);
            if (res != null && res.status)
            {
                actionOnResponse.Invoke(true, res.data);
            }
            else
            {
                actionOnResponse?.Invoke(false, null);
            }
        }


        public async void GenerateTextFromProms(string userTextInput, Action<bool, TextPromsPOJO> actionOnResponse)
        {
            var url = baseURL + "history/create";
            _postDict.Clear();
            // _postDict.Add("name", username);
            // _postDict.Add("email", email);
            _postDict.Add("user_id", PlayerPrefs.GetString("userId"));
            //  _postDict.Add("user_text_input", PlayerPrefs.GetString("PlayerToken"));
            _postDict.Add("user_text_input", userTextInput);

            // _postDict.Add(GameConstant.PlayerCurrentGameMode, currentMode.ToString());
            var postData = MySerializer.Serialize(_postDict);
            //  string postData = "{\"username\":\"" + username + "\",\"country\":\"" + country + "\"}";

            Debug.Log($"{url} : {postData}");
            var res = await _apiClient.Post<APIResponse<TextPromsPOJO>>(url, postData);
            if (res != null && res.status)
            {
                actionOnResponse.Invoke(true, res.data);
            }
            else
            {
                actionOnResponse?.Invoke(false, null);
            }
        }
        public async void GenerateVideoFromText(string aiTextInput, string id, Action<bool> actinOnResponse)
        {
            var url = baseURL + "history/generate_video/" + id;
            var res = await _apiClient.Get<APIResponse<VideoGetPOJO>>(url);
            if (res != null && res.status == true)
            {

                // Debug.Log("Data ::" + res.data + res.success);
                actinOnResponse?.Invoke(true);
            }
            else
            {
                //  Debug.Log("Data ::" + res.success);
                actinOnResponse?.Invoke(false);
            }
        }



        // public async void GenerateVideoFromText(string aiTextInput, string id, Action<bool> actionOnResponse)
        // {
        //     var url = baseURL + "history/update" + id;
        //     _postDict.Clear();
        //     // _postDict.Add("name", username);
        //     _postDict.Add("email", aiTextInput);
        //     // _postDict.Add("player_id", PlayerPrefs.GetString("PlayerID"));
        //     //  _postDict.Add("playerId", playerID);

        //     // _postDict.Add(GameConstant.PlayerCurrentGameMode, currentMode.ToString());
        //     var postData = MySerializer.Serialize(_postDict);
        //     //  string postData = "{\"username\":\"" + username + "\",\"country\":\"" + country + "\"}";

        //     Debug.Log($"{url} : {postData}");
        //     var res = await _apiClient.Post<APIResponse<TextPromsPOJO>>(url, postData);
        //     if (res != null && res.status)
        //     {
        //         actionOnResponse.Invoke(true);
        //     }
        //     else
        //     {
        //         actionOnResponse?.Invoke(false);
        //     }
        // }
        public async void UpdateUserScore(string userID, string score, Action<bool, SetPlayerData> actionOnResponse)
        {

            var url = baseURL + "addUserScore";
            int currentMode = (int)GameConstant.CurrentGameMode;
            _postDict.Clear();
            _postDict.Add("userId", userID);
            _postDict.Add("score", score);
            _postDict.Add(GameConstant.PlayerCurrentGameMode, currentMode.ToString());
            var postData = MySerializer.Serialize(_postDict);
            ///  string postData = "{\"userId\":\"" + userID + "\",\"score\":" + score + "}";
            try
            {
                var res = await _apiClient.Patch<APIResponse<SetPlayerData>>(url, postData);
                if (res != null && res.status == true)
                    actionOnResponse?.Invoke(true, res.data);
                else actionOnResponse?.Invoke(false, null);
            }
            catch (Exception e)
            {
                Debug.Log("UpdateUserScore exception : " + e);
            }
        }


    }
}