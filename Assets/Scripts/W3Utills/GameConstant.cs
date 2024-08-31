using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace W3Labs.Utils
{
    public class GameConstant : MonoBehaviour
    {
        public static int GroundSpeed { get; set; }
        public static uint CurrentGameTime { get; set; }
        public static bool IsGameContiune { get; set; }
        public static bool IsGameRepawn { get; set; }
        public static bool IsReStart { get; set; }
        public static bool IsLeaderBoardChanged { get; set; }
        public static int PlayerCount { get; set; }
        public static uint PlayerScore { get; set; }
        public static string UserID { get; set; }
        public static string PromsMessage { get; set; }
        public static string ApplicationVersionNumber { get; set; }
        //
        public static GameDifficultMode CurrentGameMode { get; set; }
        void Start()
        {
            CurrentGameTime = 0;
            ApplicationVersionNumber = Application.version;
            StartCoroutine(TimeContiune());
            IsGameContiune = true;
        }
        IEnumerator TimeContiune()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                // Debug.Log("CurrentGameTime::" + CurrentGameTime);
                if (IsGameContiune)
                    CurrentGameTime++;


            }
        }







        //Player Prefabs String
        public static string _authHeaderkey = "bypass_auth";
        public static string _authHeaderValue = "L7fYqrhT9e";
        public static string _vNo = "vNo";
        public static string _userID = "userID";
        public static string PlayerCurrentGameMode = "difficultyMode";
        public static string LastPlayerCount = "LastPlayerCount";
        public static string PlayerStartCount = "PlayerStartCount";
        public static string PlaySoundStatus = "PlaySoundStatus";
        public static string PlayerStart = "PlayStartGame";
        public static string LeaderBoardData = "LeaderBoardData";
    }

    public enum GameDifficultMode
    {
        NONE,
        EASY,
        DIFFICULT
    }
    public enum PlayerPrefabs
    {
        AuthHeaderkey

    }
}
