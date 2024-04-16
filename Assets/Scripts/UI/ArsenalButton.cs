using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArsenalButton : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _limb;

    [SerializeField] private Material _selectMaterial;
    [SerializeField] private Material _unselectMaterial;
    [SerializeField] private Color _selectColor = Color.white;
    [SerializeField] private Color _unselectColor = Color.white;

    [SerializeField] private string _limbName;

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

    public void ArsenalClicked()
    {
        if (_selected)
        {
            RemoveLimb();
            _limb.material = _unselectMaterial;
            _background.color = _unselectColor;
        }
        else
        {
            AddLimb();
            _limb.material = _selectMaterial;
            _background.color = _selectColor;
        }
    }

    private void RemoveLimb()
    {

    }

    private void AddLimb()
    {

    }
}
