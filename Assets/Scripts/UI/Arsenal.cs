using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    [SerializeField] private List<GameObject> _limbList;

    private void Awake()
    {
        var limbManager = ServiceLocator.Get<LimbManager>();
        limbManager.SetLimbOptions(_limbList);
    }
}
