
using UnityEngine;
using UnityEngine.SceneManagement;
using W3Labs.ViralRunner.Network;

namespace W3Labs
{
    public class GameStartController : MonoBehaviour
    {
        [SerializeField] private APIHandlerW3labs aPIHandlerW3Labs;
        private UserInfo userInfo;
        void OnEnable()
        {
            AddUser();
        }
        public void AddUser()
        {

            aPIHandlerW3Labs.AddUser("userName", "country", (status, data) =>
                   {
                       if (status)
                       {
                           userInfo = data;
                           PlayerPrefs.SetString("userId", data.id);
                           SceneManager.LoadSceneAsync(1);
                       }
                       else
                           Debug.Log("Value Not Set");

                   });
        }

    }
}
