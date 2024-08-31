using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace W3Labs.ViralRunner.Network
{
    [Serializable]
    public class APIResponse<T>
    {
        // [JsonProperty("success")]
        public bool status { get; set; }
        //  [JsonProperty("msg")]
        public string msg { get; set; }
        // [JsonProperty("data")]
        public T data { get; set; }
        // [JsonProperty("display")]
        // public bool display { get; set; }
    }
    public class UserInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string language { get; set; }
        public string token { get; set; }
        public string player_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string id { get; set; }
    }

    public class TextPromsPOJO
    {
        public string user_id { get; set; }
        public string user_image_input { get; set; }
        public string user_text_input { get; set; }
        public string script_output { get; set; }
        public string video_response { get; set; }
        public string timestamp { get; set; }
        public string id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class VideoPromsPOJO
    {
        public string user_id { get; set; }
        public string user_image_input { get; set; }
        public string user_text_input { get; set; }
        public string script_output { get; set; }
        public string video_response { get; set; }
        public string timestamp { get; set; }
        public string id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class VideoGetPOJO
    {

    }


    [Serializable]
    public class SetPlayerData
    {
        [JsonProperty("userId ")]
        public string userID { get; set; }
        [JsonProperty("maxScore")]
        public int maxScore { get; set; }

        [JsonProperty("difficultyMode ")]
        public int playerScore { get; set; }


    }
    public class GettingVideoLink
    {
        public string video_url { get; set; }
        //[JsonProperty("playersDataArray")]
        //  public List<PlayerData> playersDataArray { get; set; }

    }
    public class PlayerDetailsPOJO
    {
        [JsonProperty("user")]
        // public PlayerInfoData playerInfoData { get; set; }
        // [JsonProperty("userScores")]
        public List<PlayerScoreData> playerScoresList { get; set; }

    }



    public class PlayerScoreData
    {

        [JsonProperty("playerScore")]
        public string playerScore { get; set; }
        [JsonProperty("difficultyMode ")]
        public int difficultyMode { get; set; }
    }


    // public class PlayersDataArray
    // {
    //     public string _id { get; set; }
    //     public string userId { get; set; }
    //     public int maxScore { get; set; }
    //     public int totalScore { get; set; }

    // }
    // public class Data
    // {
    //     public IList<PlayersDataArray> playersDataArray { get; set; }

    // }
    // public class Application
    // {
    //     public bool success { get; set; }
    //     public string msg { get; set; }
    //     public Data data { get; set; }

    // }

}
