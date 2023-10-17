using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BFM
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        protected static T sInstance = null;

        public static T Instance
        {
            get
            {
                if (sInstance == null)
                {
                    sInstance = FindObjectOfType(typeof(T)) as T;

                    if (sInstance == null)
                    {
                        Debug.Log("Nothing" + sInstance.ToString());
                        return null;
                    }
                }
                return sInstance;
            }
        }

        // Use this for initialization
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance != this)
                Destroy(gameObject);
        }
    }
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 제네릭 형으로 게임 오브젝트 로드 (DontDestroy로)
                GameObject gameObject = new GameObject(typeof(T).ToString());
                GameObject.DontDestroyOnLoad(gameObject);

                // 컴포넌트 셋팅
                _instance = gameObject.AddComponent<T>();
            }

            return _instance;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
