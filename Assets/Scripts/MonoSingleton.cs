using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance = null;
    public static bool isDontDestroy = false;
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
                return null;

            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }

                if (!isDontDestroy)
                {
                    DontDestroyOnLoad(instance.gameObject);
                    isDontDestroy = true;
                }
            }
            return instance;
        }
    }

    public void Awake()
    {
        if (applicationIsQuitting)
        {
            instance = null;
            return;
        }

        if (instance == null)
        {
            instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                instance = new GameObject(typeof(T).ToString()).AddComponent<T>();  
            }

            if (!isDontDestroy)
            {
                DontDestroyOnLoad(instance.gameObject);
                isDontDestroy = true;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
