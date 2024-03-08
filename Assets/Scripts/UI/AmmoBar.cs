using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//switching current limb changes the position of sprites
//

public class AmmoBar : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    [SerializeField] private List<Vector3> _positions = new();
    [SerializeField] private List<Image> _limbImages = new();
    private List<int> positions = new();
    private int _currentLimb;
    private float _bigScale;
    private float _smallScale;

    private void Awake()
    {
        ServiceLocator.Get<GameLoader>().CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            _positions.Add(_limbImages[i].transform.position);
        }
    }

    public void AddLimb(int index, Sprite limbSprite)
    {
        _limbImages[index].color = new Color(1, 1, 1, 1);
        _limbImages[index].sprite = limbSprite;
        positions[index] = index;
    }

    public void RemoveLimb(int index)
    {
        _limbImages[index].color = new Color(0, 0, 0, 0);
        _limbImages[index].sprite = null;
    }
}
