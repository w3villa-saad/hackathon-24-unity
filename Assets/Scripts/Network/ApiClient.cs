using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using W3Labs.Utils;
using Debug = UnityEngine.Debug;

namespace W3Labs.ViralRunner.Network
{
    public class ApiClient : MonoBehaviour
    {

        private readonly ISerializationOption serializationOption;
        private const int _timeout = 30;



        public async Task<TResultType>
        Get<TResultType>(string url, int timeOut = 30)
        {
            try
            {
                int currentMode = (int)GameConstant.CurrentGameMode;
                url += "?" + GameConstant.PlayerCurrentGameMode + "=" + currentMode.ToString();
                Debug.Log($"CALLING: {url}");
                using var webRequest = UnityWebRequest.Get(url);
                webRequest.SetRequestHeader(GameConstant._authHeaderkey, GameConstant._authHeaderValue);
                webRequest.SetRequestHeader(GameConstant._vNo, GameConstant.ApplicationVersionNumber);
                webRequest.SetRequestHeader(GameConstant._userID, GameConstant.UserID);


                if (timeOut > -1)
                    webRequest.timeout = timeOut;

                var operation = webRequest.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed: {url} {webRequest.error}{webRequest.downloadHandler.text} ");
                    return default;
                }
                else
                {
                    string Value = webRequest.downloadHandler.text;
                    Debug.Log($"Success: {url}  {Value}");
                    //var result = serializationOption.Deserialize<TResultType>(webRequest.downloadHandler.text);
                    var result = JsonConvert.DeserializeObject<TResultType>(webRequest.downloadHandler.text);
                    Debug.Log(MySerializer.Serialize(result));
                    return result;

                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{nameof(Get)} Failed: {e.Message} | {e.StackTrace}");
                return default;
            }
        }

        public async Task<TResultType> Patch<TResultType>(string url, string postData, int timeOut = -100)
        {
            try
            {
                Debug.Log($"CALLING: {url} {MySerializer.Serialize(postData)}");
                using var webRequest = new UnityWebRequest(url, "PATCH"); //Post(url, postData);
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
                //webRequest.method = "POST";
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SetRequestHeader(GameConstant._authHeaderkey, GameConstant._authHeaderValue);
                webRequest.SetRequestHeader(GameConstant._vNo, GameConstant.ApplicationVersionNumber);
                webRequest.SetRequestHeader(GameConstant._userID, GameConstant.UserID);


                if (timeOut == -100)
                    webRequest.timeout = _timeout;
                else
                {
                    if (timeOut <= 0)
                    {
                        return default;
                    }
                    else
                        webRequest.timeout = (timeOut > _timeout) ? _timeout : timeOut;
                }

                //Debug.Log(url + "  :  " + webRequest.timeout);
                var operation = webRequest.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed: {webRequest.error} , {webRequest.result} ");
                    //GameEvents.Current.ErrorReceived((int)webRequest.responseCode, webRequest.downloadHandler.text);
                    //OnCompletion?.Invoke();
                    return default;
                }
                else
                {

                    Debug.Log($"{url}: {webRequest.downloadHandler.text}");
                    var result = JsonConvert.DeserializeObject<TResultType>(webRequest.downloadHandler.text);
                    //Debug.Log(MySerializer.Serialize(result));
                    return result;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{nameof(Patch)} Failed: {url} | {e.Message} | {e.StackTrace} ");
                return default;
            }
        }

        public async Task<TResultType> Post<TResultType>(string url, string postData)
        {
            try
            {
                Debug.Log($"CALLING: {url} | {postData}");
                byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(postData);//.GetBytes(postData);
                using var webRequest = new UnityWebRequest(url, "POST"); //Post(url, postData);
                //webRequest.method = "POST"; 
                webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");
                //  webRequest.SetRequestHeader(GameConstant._authHeaderkey, GameConstant._authHeaderValue);
                //  webRequest.SetRequestHeader(GameConstant._vNo, GameConstant.ApplicationVersionNumber);
                // webRequest.SetRequestHeader(GameConstant._userID, GameConstant.UserID);

                webRequest.timeout = _timeout;


                var operation = webRequest.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed: {webRequest.error} , {webRequest.result} ");
                    // GameEvents.Current.ErrorReceived((int)webRequest.responseCode, webRequest.downloadHandler.text);
                    return default;
                }
                else
                {
                    Debug.Log($"Success: {url} | {webRequest.downloadHandler.text}");
                    var result = JsonConvert.DeserializeObject<TResultType>(webRequest.downloadHandler.text);
                    Debug.Log(MySerializer.Serialize(result));
                    return result;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{nameof(Post)} Failed: {url} | {e.Message} | {e.StackTrace} ");
                return default;
            }
        }

        // public async Task<TResultType> Post<TResultType>(string url, object postData)
        // {
        //     try
        //     {
        //         Debug.Log($"CALLING: {url} | {MySerializer.Serialize(postData)}", ErrorTypes.Debug);
        //         //Debug.Log(MySerializer.Serialize(postData));
        //         using var webRequest = new UnityWebRequest(url, "POST"); //Post(url, postData);
        //         byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(MySerializer.Serialize(postData));
        //         webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        //         webRequest.downloadHandler = new DownloadHandlerBuffer();
        //         webRequest.SetRequestHeader("Content-Type", serializationOption.ContentType);
        //         webRequest.SetRequestHeader("vNo", serializationOption.UserIdentifier.vNo);
        //         webRequest.SetRequestHeader("token", serializationOption.UserIdentifier.token);
        //         webRequest.SetRequestHeader("Device-Type", serializationOption.UserIdentifier.DeviceType);
        //         webRequest.SetRequestHeader("UUID", string.IsNullOrEmpty(serializationOption.UserIdentifier.UUID) ? SystemInfo.deviceUniqueIdentifier : serializationOption.UserIdentifier.UUID);
        //         webRequest.timeout = _timeout;

        //         var operation = webRequest.SendWebRequest();

        //         while (!operation.isDone)
        //             await Task.Yield();

        //         if (webRequest.result != UnityWebRequest.Result.Success)
        //         {
        //             Debug.LogError($"Failed: {webRequest.error} , {webRequest.result} ", ErrorTypes.Debug);
        //             GameEvents.Current.ErrorReceived((int)webRequest.responseCode, webRequest.downloadHandler.text);
        //             return default;

        //         }
        //         else
        //         {
        //             Debug.Log($"Response: {url} | {webRequest.downloadHandler.text}", ErrorTypes.API_CALL);
        //             var result = serializationOption.Deserialize<TResultType>(webRequest.downloadHandler.text);
        //             Debug.Log(MySerializer.Serialize(result));
        //             return result;
        //         }
        //     }
        //     catch (System.Exception e)
        //     {
        //         Debug.LogError($"{nameof(Post)} Failed: {url} | {e.Message} | {e.StackTrace} ", ErrorTypes.API_CALL);
        //         return default;
        //     }
        // }


        // public async Task<TResultType> PostForm<TResultType>(string url, WWWForm postData)
        // {
        //     try
        //     {
        //         Debug.Log($"CALLING: {url} | {MySerializer.Serialize(postData)}", ErrorTypes.API_CALL);
        //         //Debug.Log(MySerializer.Serialize(postData));
        //         using var webRequest = new UnityWebRequest(url, "POST"); //Post(url, postData);
        //         byte[] jsonToSend = postData.data;//new System.Text.UTF8Encoding().GetBytes(postData);
        //         //webRequest.method = "POST";

        //         webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        //         webRequest.downloadHandler = new DownloadHandlerBuffer();
        //         webRequest.SetRequestHeader("Content-Type", "multipart/form-data");
        //         webRequest.SetRequestHeader("vNo", serializationOption.UserIdentifier.vNo);
        //         webRequest.SetRequestHeader("token", serializationOption.UserIdentifier.token);
        //         webRequest.SetRequestHeader("Device-Type", serializationOption.UserIdentifier.DeviceType);
        //         webRequest.SetRequestHeader("UUID", string.IsNullOrEmpty(serializationOption.UserIdentifier.UUID) ? SystemInfo.deviceUniqueIdentifier : serializationOption.UserIdentifier.UUID);
        //         webRequest.timeout = _timeout;

        //         var operation = webRequest.SendWebRequest();

        //         while (!operation.isDone)
        //             await Task.Yield();

        //         if (webRequest.result != UnityWebRequest.Result.Success)
        //         {
        //             Debug.LogError($"Failed: {webRequest.error} , {webRequest.result} ", ErrorTypes.API_CALL);
        //             GameEvents.Current.ErrorReceived((int)webRequest.responseCode, webRequest.downloadHandler.text);
        //             return default;

        //         }
        //         else
        //         {
        //             Debug.Log($"Success: {url} | {webRequest.downloadHandler.text}", ErrorTypes.API_CALL);
        //             var result = serializationOption.Deserialize<TResultType>(webRequest.downloadHandler.text);
        //             Debug.Log(MySerializer.Serialize(result));
        //             return result;
        //         }
        //     }
        //     catch (System.Exception e)
        //     {
        //         Debug.LogError($"{nameof(Post)} Failed: {url} | {e.Message} | {e.StackTrace} ", ErrorTypes.API_CALL);
        //         return default;
        //     }
        // }

        // public async Task<TResultType> Put<TResultType>(string url, string postData, int timeOut = -100)
        // {
        //     try
        //     {
        //         Debug.Log($"CALLING: {url} {MySerializer.Serialize(postData)}", ErrorTypes.API_CALL);
        //         //MyLogger.Log(MySerializer.Serialize(postData));
        //         byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
        //         using var webRequest = UnityWebRequest.Put(url, jsonToSend);
        //         { // new UnityWebRequest(url, "PATCH"); //Post(url, postData);
        //           //webRequest.method = "POST";
        //             webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        //             webRequest.downloadHandler = new DownloadHandlerBuffer();
        //             webRequest.SetRequestHeader("Content-Type", serializationOption.ContentType);
        //             webRequest.SetRequestHeader("vNo", serializationOption.UserIdentifier.vNo);
        //             webRequest.SetRequestHeader("token", serializationOption.UserIdentifier.token);
        //             webRequest.SetRequestHeader("Device-Type", serializationOption.UserIdentifier.DeviceType);
        //             webRequest.SetRequestHeader("UUID", string.IsNullOrEmpty(serializationOption.UserIdentifier.UUID) ? SystemInfo.deviceUniqueIdentifier : serializationOption.UserIdentifier.UUID);

        //             if (timeOut == -100)
        //                 webRequest.timeout = _timeout;
        //             else
        //             {
        //                 if (timeOut <= 0)
        //                 {
        //                     webRequest.Dispose();
        //                     return default;
        //                 }
        //                 else
        //                     webRequest.timeout = (timeOut > _timeout) ? _timeout : timeOut;
        //             }

        //             //Debug.Log(url + "  :  " + webRequest.timeout);
        //             var operation = webRequest.SendWebRequest();

        //             while (!operation.isDone)
        //                 await Task.Yield();

        //             if (webRequest.result != UnityWebRequest.Result.Success)
        //             {
        //                 Debug.LogError($"Failed: {webRequest.error} , {webRequest.result} ", ErrorTypes.API_CALL);
        //                 GameEvents.Current.ErrorReceived((int)webRequest.responseCode, webRequest.downloadHandler.text);
        //                 webRequest.Dispose();
        //                 return default;

        //             }
        //             else
        //             {
        //                 Debug.Log($"Success: {url} | {webRequest.downloadHandler.text}", ErrorTypes.API_CALL);
        //                 var result = serializationOption.Deserialize<TResultType>(webRequest.downloadHandler.text);
        //                 Debug.Log(MySerializer.Serialize(result));
        //                 webRequest.Dispose();
        //                 return result;
        //             }
        //         }
        //     }
        //     catch (System.Exception e)
        //     {
        //         Debug.LogError($"{nameof(Put)} Failed: {url} | {e.Message} | {e.StackTrace} ", ErrorTypes.API_CALL);
        //         return default;
        //     }
        // }

        // public IEnumerator CoGetTexture(string url, int id)
        // {
        //     using var webRequest = UnityWebRequestTexture.GetTexture(url);
        //     webRequest.SetRequestHeader("vNo", serializationOption.UserIdentifier.vNo);
        //     webRequest.SetRequestHeader("token", serializationOption.UserIdentifier.token);
        //     webRequest.SetRequestHeader("Device-Type", serializationOption.UserIdentifier.DeviceType);
        //     webRequest.SetRequestHeader("UUID", string.IsNullOrEmpty(serializationOption.UserIdentifier.UUID) ? SystemInfo.deviceUniqueIdentifier : serializationOption.UserIdentifier.UUID);
        //     //webRequest.SetRequestHeader("Content-Type", serializationOption.ContentType);
        //     yield return webRequest.SendWebRequest();

        //     //while (!operation.isDone)
        //     //    yield return null;
        //     try
        //     {

        //         if (webRequest.result != UnityWebRequest.Result.Success)
        //             Debug.LogError($"Failed: {webRequest.error}", ErrorTypes.API_CALL);

        //         //var result = serializationOption.Deserialize<TResultType>(webRequest.downloadHandler.text);
        //         //Debug.Log($"Success: {webRequest.downloadHandler.text}");
        //         questionsDataSO.textureDict.Add(id, ((DownloadHandlerTexture)webRequest.downloadHandler).texture); //DownloadHandlerTexture.GetContent(webRequest);
        //     }
        //     catch (System.Exception e)
        //     {
        //         Debug.LogError($"{nameof(Get)} Failed: {e.Message} | Stacktrace: {e.StackTrace}", ErrorTypes.API_CALL);
        //     }
        // }

        // public async Task<Texture2D> GetTexture(string url)
        // {
        //     Debug.Log("getTexture : " + url);
        //     if (string.IsNullOrEmpty(url) || url.Contains("null", System.StringComparison.OrdinalIgnoreCase))
        //     {
        //         return default;
        //     }

        //     using var webRequest = UnityWebRequestTexture.GetTexture(url);
        //     webRequest.SetRequestHeader("vNo", serializationOption.UserIdentifier.vNo);
        //     webRequest.SetRequestHeader("token", serializationOption.UserIdentifier.token);
        //     webRequest.SetRequestHeader("Device-Type", serializationOption.UserIdentifier.DeviceType);
        //     webRequest.SetRequestHeader("UUID", string.IsNullOrEmpty(serializationOption.UserIdentifier.UUID) ? SystemInfo.deviceUniqueIdentifier : serializationOption.UserIdentifier.UUID);

        //     try
        //     {
        //         var operation = webRequest.SendWebRequest();

        //         while (!operation.isDone)
        //             await Task.Yield();

        //         Texture2D result;
        //         if (url.Contains(".webp"))
        //         {
        //             result = Texture2DExt.CreateTexture2DFromWebP(webRequest.downloadHandler.data, true, false, out Error error);
        //             if (error != Error.Success)
        //                 MyLogger.Log($"Unable to fetch texture: {url} | {error} | {webRequest.downloadHandler.error}", ErrorTypes.API_CALL);
        //         }
        //         else
        //         {
        //             if (webRequest.result != UnityWebRequest.Result.Success) // && !url.Contains("webp"))
        //                 MyLogger.Log($"Unable to fetch texture: {url} | {webRequest.error} | {webRequest.downloadHandler.error}", ErrorTypes.API_CALL);

        //             result = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
        //         }

        //         return result ?? default;
        //     }
        //     catch (System.Exception e)
        //     {
        //         MyLogger.Log($"Failed to Get Texture : {e.Message}, {e.StackTrace},{url}", ErrorTypes.API_CALL);
        //         return default;
        //     }
        // }


        // public async Task SaveTexture(string key, string url, string filename)
        // {
        //     using var webRequest = UnityWebRequestTexture.GetTexture(url);
        //     webRequest.SetRequestHeader("vNo", serializationOption.UserIdentifier.vNo);
        //     webRequest.SetRequestHeader("token", serializationOption.UserIdentifier.token);
        //     webRequest.SetRequestHeader("Device-Type", serializationOption.UserIdentifier.DeviceType);
        //     webRequest.SetRequestHeader("UUID", string.IsNullOrEmpty(serializationOption.UserIdentifier.UUID) ? SystemInfo.deviceUniqueIdentifier : serializationOption.UserIdentifier.UUID);
        //     try
        //     {
        //         var operation = webRequest.SendWebRequest();

        //         while (!operation.isDone)
        //             await Task.Yield();

        //         if (webRequest.result != UnityWebRequest.Result.Success)
        //             MyLogger.Log($"Unable to fetch texture: {url}. {webRequest.error} {webRequest.downloadHandler.error}", ErrorTypes.API_CALL);

        //         var path = $"{Application.persistentDataPath}/{key}/{filename}";
        //         byte[] dataBytes = webRequest.downloadHandler.data;//result.EncodeToPNG();
        //         File.WriteAllBytes(path, dataBytes);
        //     }
        //     catch (System.Exception e)
        //     {
        //         MyLogger.Log($"Failed to Get Texture : {e.Message}, {e.StackTrace}", ErrorTypes.API_CALL);
        //         //return default;
        //     }
        // }



        // public async Task GetFile(int key, string url)
        // {
        //     using var webRequest = UnityWebRequest.Get(url);
        //     var path = $"{Application.persistentDataPath}/{key}/";
        //     Debug.Log(path);
        //     webRequest.downloadHandler = new DownloadHandlerFile(path);

        //     var operation = webRequest.SendWebRequest();
        //     while (!operation.isDone)
        //         await Task.Yield();

        //     if (webRequest.result != UnityWebRequest.Result.Success)
        //         MyLogger.LogError($"Failed: {webRequest.error}", ErrorTypes.API_CALL);
        // }
    }
}
