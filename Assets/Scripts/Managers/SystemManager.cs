
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    List<Manager> managers = new List<Manager>();

    static SystemManager instance;
    public static SystemManager Instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
        GetComponentsInChildren(managers);

        foreach (var manager in managers)
            manager.OnStart();
    }

    public T Get<T>() where T : Manager
    {
        foreach (var manager in managers)
            if (manager.GetType() == typeof(T))
                return (T)manager;

        var temp = new GameObject(typeof(T).Name).AddComponent<T>();
        temp.transform.SetParent(transform, false);
        return (T)temp;
    }

    public void OnDestroy()
    {
        foreach (var manager in managers)
            manager.OnEnd();

        instance = null;
    }
}
