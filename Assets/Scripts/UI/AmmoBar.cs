using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private List<Vector3> _positions = new();
    [SerializeField] private List<Quaternion> _rotations = new();
    [SerializeField] private List<Image> _limbImages;
    private PlayerLimbs _playerLimbs;

    [SerializeField] private float _bigScale;
    [SerializeField] private float _smallScale;

    private void Awake()
    {
        ServiceLocator.Get<GameLoader>().CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            _positions.Add(_limbImages[i].transform.localPosition);
            _rotations.Add(_limbImages[i].transform.localRotation);
        }
    }

    public void AddLimb(int index, Sprite limbSprite)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_limbImages[i].sprite == null)
            {
                if (i == 0)
                {
                    _limbImages[index].color = new Color(1, 1, 1, 1);
                    _limbImages[index].sprite = limbSprite;
                }
                else
                {
                    _limbImages[index].color = new Color(0.66f, 0.66f, 0.66f, 0.77f);
                    _limbImages[index].sprite = limbSprite;
                }
                break;
            }
        }
        ChangeListOrder();
    }

    public void RemoveLimb(int index)
    {
        _limbImages[index].color = new Color(0, 0, 0, 0);
        _limbImages[index].sprite = null;
    }

    public void ChangeListOrder()
    {
        List<int> order = _playerLimbs.GetLimbHierarchy();

        for (int i = 0; i < order.Count; i++)
        {
            _limbImages[order[i]].transform.localPosition = _positions[i];
            _limbImages[order[i]].transform.localRotation = _rotations[i];
            if (i == 0)
            {
                _limbImages[order[i]].transform.localScale = new Vector3(_bigScale, _bigScale, _bigScale); 
                _limbImages[order[i]].color = new Color(1, 1, 1, 1);
            }
            else
            {
                _limbImages[order[i]].transform.localScale = new Vector3(_smallScale, _smallScale, _smallScale);
                _limbImages[order[i]].color = new Color(0.66f, 0.66f, 0.66f, 0.77f);
            }
        }
    }

    public void SetPlayerLimbs(PlayerLimbs pLimbs)
    {
        _playerLimbs = pLimbs;
    }
}
