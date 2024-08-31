using UnityEngine;

namespace SuperOne.Utils
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance;
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
            }
            else if (Instance != null)
            {
                Destroy(this.gameObject);
            }
        }
    }


}