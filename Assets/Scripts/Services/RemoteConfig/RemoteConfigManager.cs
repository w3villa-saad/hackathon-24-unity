using System;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using UnityEngine;
using W3Labs.Services;
using ConfigResponse = Unity.Services.RemoteConfig.ConfigResponse;

namespace W3Labs.RemoteConfig
{
    public class RemoteConfigManager : MonoBehaviour
    {
        public String Demo = "HelloSetUpDone";
        public static float TimeDealyInMintesForInsteritialsAds = 3;
        public static float TimeDealyInSecondsForUpdateLeaderBoardData = 30;
        public static int PlayerCountLevelConstant = 10;
        public static int EnemyCountLevelConstant = 10;
        public static int GroundModeConstantEasyMode = 10;
        public static int GroundModeConstantDifficultMode = 100;
        public static int ExpressionConstantEasyMode = 10;
        public static int ExpressionConstantDifficuiltMode = 100;
        public static int ExpressionListCountIncressConstantEasyMode = 10;//
        public static int ExpressionListCountIncressConstantDifficultMode = 5;//
        public static int ExpressionListSize = 5;//

        public static string PrivacyPolicyURl = "https://www.w3villa.com/games/general-privacy-policy";
        public static string BaseURlString;// = $"https://gameleaderboard.w3villa.com/api/";
        public static string ContanctUsEmailString = "<color=White> <link=mailto:contact@w3villa.com?subject=Feedback:%20Trace%20and%20Learn&body=Hello%20Devs,>contact@w3villa.com</link></color>";

        public static bool IsInitialized = false;
        // Ad
        public static bool IsAdsEnable = false;
        private const bool IosAdsEnabled = false;
        private const bool AndroidAdsEnabled = false;
        //Analytics
        public static bool IsDataCollectionEnabled = false;
        private const bool IosDataCollection = false;
        private const bool AndroidDataCollection = false;
        private void UpdateConfig(ConfigResponse obj)
        {
            IsInitialized = true;
            Debug.Log($"RemoteConfig fetched : Status {obj.requestOrigin} {obj.status} Env: {RemoteConfigService.Instance.appConfig.environmentId}");

            //Update keys based on remote config response
            // Demo = RemoteConfigService.Instance.appConfig.GetString(nameof(Demo));
            TimeDealyInMintesForInsteritialsAds = RemoteConfigService.Instance.appConfig.GetInt(nameof(TimeDealyInMintesForInsteritialsAds));
            TimeDealyInSecondsForUpdateLeaderBoardData = RemoteConfigService.Instance.appConfig.GetInt(nameof(TimeDealyInSecondsForUpdateLeaderBoardData));
            PlayerCountLevelConstant = RemoteConfigService.Instance.appConfig.GetInt(nameof(PlayerCountLevelConstant));
            EnemyCountLevelConstant = RemoteConfigService.Instance.appConfig.GetInt(nameof(EnemyCountLevelConstant));
            // Game Mode 
            // Difficult
            ExpressionConstantDifficuiltMode = RemoteConfigService.Instance.appConfig.GetInt(nameof(ExpressionConstantDifficuiltMode));
            GroundModeConstantDifficultMode = RemoteConfigService.Instance.appConfig.GetInt(nameof(GroundModeConstantDifficultMode));
            //Easy
            ExpressionConstantEasyMode = RemoteConfigService.Instance.appConfig.GetInt(nameof(ExpressionConstantEasyMode));
            GroundModeConstantEasyMode = RemoteConfigService.Instance.appConfig.GetInt(nameof(GroundModeConstantEasyMode));
            ExpressionListCountIncressConstantDifficultMode = RemoteConfigService.Instance.appConfig.GetInt(nameof(ExpressionListCountIncressConstantDifficultMode));
            ExpressionListCountIncressConstantEasyMode = RemoteConfigService.Instance.appConfig.GetInt(nameof(ExpressionListCountIncressConstantEasyMode));
            ExpressionListSize = RemoteConfigService.Instance.appConfig.GetInt(nameof(ExpressionListSize));
            PrivacyPolicyURl = RemoteConfigService.Instance.appConfig.GetString(nameof(PrivacyPolicyURl));
            ContanctUsEmailString = RemoteConfigService.Instance.appConfig.GetString(nameof(ContanctUsEmailString));
            BaseURlString = RemoteConfigService.Instance.appConfig.GetString(nameof(BaseURlString));

            // IsAdsEnable = RemoteConfigService.Instance.appConfig.GetBool(nameof(IsAdsEnable));
#if UNITY_IOS
            IsAdsEnable =  RemoteConfigService.Instance.appConfig.GetBool(nameof(IosAdsEnabled));
            IsDataCollectionEnabled =  RemoteConfigService.Instance.appConfig.GetBool(nameof(IosDataCollection));
#elif UNITY_ANDROID
            IsAdsEnable = RemoteConfigService.Instance.appConfig.GetBool(nameof(AndroidAdsEnabled));
            IsDataCollectionEnabled = RemoteConfigService.Instance.appConfig.GetBool(nameof(AndroidDataCollection));
#endif

            //  Debug.Log($"ContanctUsEmailString::{ContanctUsEmailString} PrivacyPolicyURl{PrivacyPolicyURl} ExpressionListSize::{ExpressionListSize}ExpressionListCountIncressConstantEasyMode:::{ExpressionListCountIncressConstantEasyMode} ExpressionListCountIncressConstantDifficultMode:::{ExpressionListCountIncressConstantDifficultMode} GroundModeConstantEasyMode:::{GroundModeConstantEasyMode} ExpressionConstantEasyMode::{ExpressionConstantEasyMode} EnemyCountLevelConstant::{EnemyCountLevelConstant} PlayerCountLevelConstan:::{PlayerCountLevelConstant} TimeDealyInHoursForInsteritialsAds:: {TimeDealyInMintesForInsteritialsAds}");
        }

        #region Setup 
        private async void Awake()
        {
            do await Task.Delay(200);
            while (ServiceInitializor.IsInitialized == false);

            // RemoteConfigService.Instance.SetEnvironmentID(ServiceInitializor.IsProd ? ServiceInitializor.ENVIRONMENT_PROD_ID : ServiceInitializor.ENVIRONMENT_DEV_ID);

            RemoteConfigService.Instance.FetchCompleted += UpdateConfig;
            _ = RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
            DontDestroyOnLoad(gameObject);

        }
        public struct userAttributes { }

        public struct appAttributes { }
        #endregion

    }
}