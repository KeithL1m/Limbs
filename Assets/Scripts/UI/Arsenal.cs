using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    [SerializeField] private List<Limb> _limbList;

    public void StartGame()
    {
        var limbManager = ServiceLocator.Get<LimbManager>();
        limbManager.SetLimbOptions(_limbList);
    }
}
