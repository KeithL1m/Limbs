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
    public bool _canThrow;

    private Transform _groundCheck;
    private Vector3 _groundCheckPosition = new();
    [SerializeField] private List<Transform> _limbAnchors;
    private List<Vector3> _anchorPositions;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private Material _overlayMaterial;
    [SerializeField] private Material _standardMaterial;

    public Vector2 _originalSize;
    public Vector2 _originalOffset;
    private float limbOffest=0.4f;
    public void Initialize()
    {
        _limbs = new List<Limb>();
        _anchorPositions = new List<Vector3>();
        _anchorPositions.Capacity = 4;
        _limbs.Capacity = 4;
        for (int i = 0; i < 4; i++)
        {
            _limbs.Add(null);
            _anchorPositions.Add(_limbAnchors[i].localPosition);
        }
        _originalSize = _collider.size;
        _originalOffset = _collider.offset;
        _groundCheck = GetComponentInChildren<GroundCheck>().transform;
        _groundCheckPosition = _groundCheck.localPosition;
    }

    //check if limb can be picked up
    public bool CanPickUpLimb(Limb limb)
    {
        if (limb.State != Limb.LimbState.PickUp && limb.State != Limb.LimbState.Returning)
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
        if (i < 2)
        {
            _limbs[(int)_selectedLimb].SetMaterial(_overlayMaterial);
            _limbs[i].Type = Limb.LimbType.Leg;
            if (i == 1)
            {
                if (_limbs[0] != null && _limbs[1].Size > _limbs[0].Size)
                {
                    _groundCheck.localPosition = _groundCheckPosition;
                    MoveBodyUp(i);
                }
            }
            else
            {
                if (_limbs[1] == null || _limbs[0].Size > _limbs[1].Size)
                {
                    _groundCheck.localPosition = _groundCheckPosition;
                    MoveBodyUp(i);
                }
                _limbs[i].FlipX(-1);
            }
            //_limbs[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            //_limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x, _limbAnchors[i].position.y - _limbs[i].Size * 0.5f);
            _limbs[i].transform.parent = _limbAnchors[i];
            _limbs[i].transform.localEulerAngles = new Vector3(0, 0, 0);
            _limbs[i].transform.localPosition = new Vector3(0, -_limbs[i].Size * 0.5f, 0);
        }
        else
        {
            _limbs[i].Type = Limb.LimbType.Arm;
            //_limbs[i].transform.rotation = Quaternion.Euler(0, 0, 90);
            _limbs[i].transform.parent = _limbAnchors[i].transform;
            _limbs[i].transform.localEulerAngles = new Vector3(0, 0, 90);
            if (i == 2)
            {
                //_limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x - _limbs[i].Size * 0.5f, _limbAnchors[i].position.y);
                _limbs[i].transform.localPosition = new Vector3(-_limbs[i].Size * 0.5f, 0, 0);
                _limbs[i].FlipY(-1);
            }
            else if (i == 3)
            {
                //_limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x + _limbs[i].Size * 0.5f, _limbAnchors[i].position.y);
                _limbs[i].transform.localPosition = new Vector3(_limbs[i].Size * 0.5f, 0, 0);
            }
        }

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _limbs[i].GetComponent<Collider2D>(), true);
        _limbs[i].AnchorPoint = _limbAnchors[i].transform;
        _limbs[i].State = Limb.LimbState.Attached;
        _limbs[i].GetComponent<Rigidbody2D>().simulated = false;
    }

    //called when picking up a leg limb
    private void MoveBodyUp(int i)
    {
        _collider.size = new Vector2(_originalSize.x, _originalSize.y + (_limbs[i].Size - limbOffest));
        _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y - _limbs[i].Size * 0.5f);
        _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y - _limbs[i].Size);
        transform.position = new Vector3(transform.position.x, transform.position.y + _limbs[i].Size);
        _limbs[i].transform.parent = _limbAnchors[i].transform;
        _limbs[i].transform.localPosition = new Vector3(-_limbs[i].Size * 0.5f, 0, 0);
    }

    //called when picking up a limb
    public void MoveBodyDown()
    {
        switch (_selectedLimb)
        {
            case SelectedLimb.RightArm:
                //_limbAnchors[3].localPosition = _anchorPositions[3];
                _limbs[3].transform.parent = _limbAnchors[3].transform;
                _limbs[3].transform.localPosition = new Vector3(-_limbs[3].Size * 0.5f, 0);
                return;
            case SelectedLimb.LeftArm:
                //_limbAnchors[2].localPosition = _anchorPositions[2];
                _limbs[2].transform.parent = _limbAnchors[2].transform;
                _limbs[2].transform.localPosition = new Vector3(_limbs[2].Size * 0.5f, 0);
                return;
            case SelectedLimb.RightLeg:
                {
                    _limbs[1].transform.parent = _limbAnchors[1].transform;
                    _limbs[1].transform.localEulerAngles = new Vector3(0, 0, 0);
                    _limbs[1].transform.localPosition = new Vector3(0, -_limbs[1].Size * 0.5f, 0);
                    LegEdit(_limbs[1], _limbs[0], 1);
                    break;
                }
            case SelectedLimb.LeftLeg:
                {
                    _limbs[0].transform.parent = _limbAnchors[0].transform;
                    _limbs[0].transform.localEulerAngles = new Vector3(0, 0, 0);
                    _limbs[0].transform.localPosition = new Vector3(0, -_limbs[0].Size * 0.5f, 0);
                    LegEdit(_limbs[0], _limbs[1], 0);
                    break;
                }
            default: break;
        }
    }

    private void LegEdit(Limb current, Limb other, int currentNum)
    { 
        if (other == null)
        {
            _collider.size = _originalSize;
            _collider.offset = _originalOffset;
            _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + current.Size);
            //_limbAnchors[currentNum].localPosition = _anchorPositions[currentNum];
        }
        else
        {
            _collider.size = new Vector2(_originalSize.x, _originalSize.y + (other.Size- limbOffest));
            _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y - other.Size * 0.5f);
            _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + current.Size);
            _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y - other.Size);
            //_limbAnchors[currentNum].localPosition = _anchorPositions[currentNum];
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
            _limbAnchors[i].localPosition = _anchorPositions[i];
            _limbs[i] = null;
        }
        foreach (var item in _limbAnchors)
        {
            item.DetachChildren();
        }
        _groundCheck.localPosition = _groundCheckPosition;
        _collider.size = _originalSize;
        _collider.offset = _originalOffset;

        _selectedLimb = SelectedLimb.LeftLeg;
    }

    //check if can throw limb
    public bool CanThrowLimb()
    {
        if (_limbs[(int)_selectedLimb] == null)
            return false;
        else if (_limbs[(int)_selectedLimb].State != Limb.LimbState.Attached)
            return false;
        else if (!_canThrow)
            return false;

        return true;
    }

    public virtual void ThrowLimb(int direction)
    {
        _limbs[(int)_selectedLimb].SetMaterial(_standardMaterial);
        _limbs[(int)_selectedLimb].ThrowLimb(direction);

        if (_limbs[(int)_selectedLimb].TripleShot)
        {
            return;
        }

        _limbs[(int)_selectedLimb] = null;
        _canThrow = false;

        int next = (((int)_selectedLimb + 1) % 2 == 0) ? -1 : 1;
        if (_limbs[(int)_selectedLimb + next] != null)
        {
            _selectedLimb += next;
            _limbs[(int)_selectedLimb].SetMaterial(_overlayMaterial);
            return;
        }

        next = ((int)_selectedLimb > 1) ? -2 : 2;
        if (_limbs[(int)_selectedLimb + next] != null)
        {
            _selectedLimb += next;
            _limbs[(int)_selectedLimb].SetMaterial(_overlayMaterial);
            return;
        }

        next = ((int)_selectedLimb > 1) ? -1 : 1;
        SelectedLimb limb = _selectedLimb;
        for (int i = 0; i < 3; i++)
        {
            limb += next;
            Debug.Log(limb);
            if (limb < 0 || (int)limb > 3)
            {
                break;
            }
            if (_limbs[(int)limb] != null)
            {
                _selectedLimb = limb;
                _limbs[(int)_selectedLimb].SetMaterial(_overlayMaterial);
                return;
            }
        }

        _selectedLimb = SelectedLimb.LeftLeg;
    }

    public void SwitchLimb(float direction)
    {
        int step = (direction > 0.5f) ? 1 : -1;
        int start = (int)_selectedLimb + step;
        int start2 = (direction > 0.5f) ? 0 : 3;
        int end = (direction > 0.5f) ? 4 : -1;
        
        int place = start;
        while (place != end)
        {
            if (_limbs[place] == null)
            {
                place += step;
            }
            else
            {
                _limbs[(int)_selectedLimb].SetMaterial(_standardMaterial);
                _selectedLimb = (SelectedLimb)place;
                _limbs[(int)_selectedLimb].SetMaterial(_overlayMaterial);
                return;
            }
        }

        place = start2;
        while (place != (int)_selectedLimb)
        {
            if (_limbs[place] == null)
            {
                place += step;
            }
            else
            {
                _limbs[(int)_selectedLimb].SetMaterial(_standardMaterial);
                _selectedLimb = (SelectedLimb)place;
                _limbs[(int)_selectedLimb].SetMaterial(_overlayMaterial);
                return;
            }
        }
    }

    public Vector3 GetSize()
    {
        return _collider.size;
    }
}
