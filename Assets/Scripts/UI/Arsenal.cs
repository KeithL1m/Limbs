using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    [SerializeField] private List<GameObject> _limbList;

    public void Setup()
    {
        var limbManager = ServiceLocator.Get<LimbManager>();
        limbManager.SetLimbOptions(_limbList);
    }
}
