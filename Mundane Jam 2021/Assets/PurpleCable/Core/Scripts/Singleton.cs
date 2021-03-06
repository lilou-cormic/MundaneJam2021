using UnityEngine;

namespace PurpleCable
{
    /// <summary>
    /// MonoBehaviour: Singleton
    /// </summary>
    /// <typeparam name="TSingleton"></typeparam>
    public abstract class Singleton<TSingleton> : MonoBehaviour
        where TSingleton : Singleton<TSingleton>
    {
        #region Instance

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static TSingleton Instance { get; protected set; }

        #endregion

        #region Unity callbacks

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as TSingleton;

            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}
