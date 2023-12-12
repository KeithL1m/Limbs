using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    static private readonly Dictionary<System.Type, object> m_systems = new Dictionary<System.Type, object>();

    static public T Register<T>(object target)
    {
        if (m_systems.ContainsKey(typeof(T)))
        {
            Debug.Log("There is already a type of : " + typeof(T) + " that exists");
        }
        else
        {
            Debug.Log("Registering " + typeof(T));
            m_systems.Add(typeof(T), target);
        }

        return (T)target;
    }

    static public T Get<T>()
    {
        object registered = null;
        m_systems.TryGetValue(typeof(T), out registered);
        if (registered == null)
        {
            Debug.Log(" [ " + (typeof(T)) + " ] can not be found as a registered system");
        }
        return (T)registered;
    }

    static public bool Contains<T>()
    {
        return (m_systems.ContainsKey(typeof(T)));
    }
}
