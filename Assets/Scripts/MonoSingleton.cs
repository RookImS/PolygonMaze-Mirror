using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance = null;
    public static bool isDontDestroy = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    instance = new GameObject("@" + typeof(T).ToString()).AddComponent<T>();
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
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                instance = new GameObject("@" + typeof(T).ToString()).AddComponent<T>();  
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
}
