using System.Collections;
using UnityEngine;

public class ExampleGameModule : MonoBehaviour, IGameModule
{
    public IEnumerator LoadModule()
    {

        ServiceLocator.Register<ExampleGameModule>(this);
        yield break;
    }

}
