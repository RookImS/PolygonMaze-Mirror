using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;
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
                DontDestroyOnLoad(Instance);
            }
            return instance;
        }
    }

    protected void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                instance = new GameObject("@" + typeof(T).ToString()).AddComponent<T>();  
            }
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
