using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class GameManagerSetup : MonoBehaviour
{
    GameLoader _loader;
    GameManager _gm;

    [SerializeField] 
    public UIManager UIManager;
    [SerializeField]
    public PauseManager PauseManager;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm  = ServiceLocator.Get<GameManager>();

        _gm.SetUp(this);
    }
}
