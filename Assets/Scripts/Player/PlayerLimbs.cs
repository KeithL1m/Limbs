using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimbs : MonoBehaviour
{
    public enum SelectedLimb
    {
        LeftLeg,
        RightLeg,
        LeftArm,
        RightArm
    };

    public enum LimbState
    {
        TwoLeg,
        OneLeg,
        NoLimb
    };

    public List<Limb> _limbs;
    public SelectedLimb _selectedLimb = SelectedLimb.LeftLeg;
    public LimbState _limbState;

    [SerializeField] Transform _groundCheck;
    [SerializeField] List<Transform> _limbAnchors;
    [SerializeField] CapsuleCollider2D _collider;

    Vector2 _originalSize;
    Vector2 _originalOffset;

    public bool _canThrow;

    // Start is called before the first frame update
    void Awake()
    {
        _limbs = new List<Limb>();
        _limbs.Capacity = 4;
        for (int i = 0; i < 4; i++)
        {
            _limbs.Add(null);
        }
        _originalSize = _collider.size;
        _originalOffset = _collider.offset;
    }

    //check if limb can be picked up
    public bool CanPickUpLimb(Limb limb)
    {
        if (limb._limbState != Limb.LimbState.PickUp && limb._limbState != Limb.LimbState.Returning)
        {
            return false;
        }

        //check if there is an empty spot 
        for (int i = 0; i < 4; i++)
        {
            if (_limbs[i] == null)
            {
                _limbs[i] = limb;
                AttachLimb(i);
                return true;
            }
        }
        return false;
    }

    //used to attach a limb
    private void AttachLimb(int i)
    {
        if (i == 0 || i == 1)
        {
            _limbs[i]._limbType = Limb.LimbType.Leg;
            if (i == 1)
            {
                _selectedLimb++;

                if (_limbs[1]._size > _limbs[0]._size)
                {
                    MoveBodyUp(i);
                    _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + _limbs[0]._size);
                }
            }
            else
            {
                MoveBodyUp(i);
            }
            _limbs[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            _limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x, _limbAnchors[i].position.y - _limbs[i]._size * 0.5f);
        }
        else
        {
            _selectedLimb++;
            _limbs[i]._limbType = Limb.LimbType.Arm;
            _limbs[i].transform.rotation = Quaternion.Euler(0, 0, 90);
            if (i == 2)
                _limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x - _limbs[i]._size * 0.5f, _limbAnchors[i].position.y);
            else if (i == 3)
                _limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x + _limbs[i]._size * 0.5f, _limbAnchors[i].position.y);
        }

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _limbs[i].GetComponent<Collider2D>(), true);
        _limbs[i]._anchorPoint = _limbAnchors[i].transform;
        _limbs[i]._limbState = Limb.LimbState.Attached;
        _limbs[i].GetComponent<Rigidbody2D>().simulated = false;
    }

    //called when losing a leg limb
    private void MoveBodyUp(int i)
    {
        _collider.size = new Vector2(_originalSize.x, _originalSize.y + _limbs[i]._size);
        _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y - _limbs[i]._size * 0.5f);
        _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y - _limbs[i]._size);
        transform.position = new Vector3(transform.position.x, transform.position.y + _limbs[i]._size);
    }

    //called when picking up a limb
    public void MoveBodyDown()
    {
        switch (_selectedLimb)
        {
            case SelectedLimb.RightArm:
                _limbAnchors[3].position = new Vector3(_limbAnchors[3].position.x - _limbs[3]._size * 0.5f, _limbAnchors[3].position.y);
                return;
            case SelectedLimb.LeftArm:
                _limbAnchors[2].position = new Vector3(_limbAnchors[2].position.x + _limbs[2]._size * 0.5f, _limbAnchors[2].position.y);
                return;
            case SelectedLimb.RightLeg:
                {
                    _collider.size = new Vector2(_originalSize.x, _originalSize.y + _limbs[0]._size);
                    _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y - _limbs[0]._size * 0.5f);
                    _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + _limbs[1]._size);
                    _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y - _limbs[0]._size);
                    _limbAnchors[1].position = new Vector3(_limbAnchors[1].position.x, _limbAnchors[1].position.y + _limbs[1]._size * 0.5f);
                    break;
                }
            case SelectedLimb.LeftLeg:
                {
                    _collider.size = new Vector2(_originalSize.x, _originalSize.y);
                    _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y);
                    _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + _limbs[0]._size);
                    _limbAnchors[0].position = new Vector3(_limbAnchors[0].position.x, _limbAnchors[0].position.y + _limbs[0]._size * 0.5f);
                    break;
                }
            default: break;
        }
    }

    //affects player movement
    public void CheckLimbState()
    {
        if (_limbs[0] != null || _limbs[1] != null)
        {
            if (_limbs[0] == null || _limbs[1] == null)
            {
                _limbState = LimbState.OneLeg;
                return;
            }

            _limbState = LimbState.TwoLeg;
            return;
        }
        _limbState = LimbState.NoLimb;
    }

    //called when round ends
    public void ClearLimbs()
    {
        for (int i = 3; i >= 0; i--)
        {
            if (_limbs[i] != null && _limbs[i]._limbState == Limb.LimbState.Attached)
            {
                ThrowLimb(0);
            }
        }

        _selectedLimb = SelectedLimb.LeftLeg;
    }

    //check if can throw limb
    public bool CanThrowLimb()
    {
        if (_limbs[(int)_selectedLimb] == null)
            return false;
        else if (_limbs[(int)_selectedLimb]._limbState != Limb.LimbState.Attached)
            return false;
        else if (!_canThrow)
            return false;

        return true;
    }

    public void ThrowLimb(int direction)
    {
        _limbs[(int)_selectedLimb].ThrowLimb(direction);
        _limbs[(int)_selectedLimb] = null;
        if (_selectedLimb != SelectedLimb.LeftLeg)
        {
            _selectedLimb--;
        }
        _canThrow = false;
    }
}
