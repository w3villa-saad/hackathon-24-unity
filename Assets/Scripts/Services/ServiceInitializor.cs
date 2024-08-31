using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Core.Environments;
using Unity.Services.RemoteConfig;

namespace W3Labs.Services
{
    public class ServiceInitializor : MonoBehaviour
    {
        private static bool IsProd = true;
        private static readonly string ENVIRONMENT_DEV_ID = "540a7982-aef6-475c-9a0a-63c429a120d4";
        private static readonly string ENVIRONMENT_PROD_ID = "fe13dfb7-3177-429d-a1aa-3398e773372a";
        private static readonly string ENVIRONMENT_DEV_STRING = "development";
        private static readonly string ENVIRONMENT_PROD_STRING = "production";
        public static bool IsInitialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var go = new GameObject(nameof(ServiceInitializor));
            go.AddComponent<ServiceInitializor>();
            DontDestroyOnLoad(go);
            IsInitialized = false;
            Initialize();
        }

        private static void Initialize()
        {
            SetEnvironment(IsProd);
            Debug.Log($"[SER] Done Initializing");
            IsInitialized = true;
        }
        private static async void SetEnvironment(bool isProd)
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName(isProd ? ENVIRONMENT_PROD_STRING : ENVIRONMENT_DEV_STRING);
            // RemoteConfigService.Instance.SetEnvironmentID(isProd ? ENVIRONMENT_PROD_ID : ENVIRONMENT_DEV_ID);

            // Debug.Log($"[SER] Initializing {options.ToString()} {(isProd ? ENVIRONMENT_PROD_ID : ENVIRONMENT_DEV_ID)}");
            Debug.Log($"[SER] Initializing {options.ToString()} {(isProd ? ENVIRONMENT_PROD_STRING : ENVIRONMENT_DEV_STRING)}");
            await UnityServices.InitializeAsync(options);

        }


    }
}