using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArsenalButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _limb;

    [SerializeField] private Material _selectMaterial;
    [SerializeField] private Material _unselectMaterial;
    [SerializeField] private Color _selectColor = Color.white;
    [SerializeField] private Color _unselectColor = Color.white;

    [SerializeField] private GameObject _connectedLimb;

    [SerializeField] private Button _button;

    private LimbManager _limbManager;
    private bool _selected = true;

    private void Awake()
    {
        GameLoader gl = ServiceLocator.Get<GameLoader>();
        gl.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _limbManager = ServiceLocator.Get<LimbManager>();
    }

    public void OnSelect(BaseEventData eventData)
    {

    }

    public void ArsenalClicked()
    {
        if (_selected)
        {
            RemoveLimb();
        }
        else
        {
            AddLimb();
        }
    }

    private void RemoveLimb()
    {
        _limbManager.RemoveFromChosen(_connectedLimb);
        _limb.material = _unselectMaterial;
        _background.color = _unselectColor;
        _selected = false;
    }

    private void AddLimb()
    {
        _limbManager.AddToChosen(_connectedLimb);
        _limb.material = _selectMaterial;
        _background.color = _selectColor;
        _selected = true;
    }
}
