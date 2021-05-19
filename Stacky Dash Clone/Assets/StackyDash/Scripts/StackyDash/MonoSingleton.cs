using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                if(Application.isPlaying)
                {
                    Debug.LogWarning($"Assigning instance {typeof(T).FullName} due to a call from {new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name}");
                }
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    Debug.LogError($"Cannot find the instance of {typeof(T).FullName}");
                }
            }
            return instance;
        }
    }

    protected void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            Debug.LogWarning($"Instance of {typeof(T).FullName} is already assigned");
        }
    }

    private void OnApplicationQuit()
    {
        instance = null;
    }
}
