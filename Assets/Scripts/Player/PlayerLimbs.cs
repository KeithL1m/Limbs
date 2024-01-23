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

    private Transform _groundCheck;
    [SerializeField] private List<Transform> _limbAnchors;
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private Material _overlayMaterial;
    [SerializeField] private Material _standardMaterial;

    Vector2 _originalSize;
    Vector2 _originalOffset;

    public bool _canThrow;

    // Start is called before the first frame update
    void Awake()
    {
        _limbs = new List<Limb>();
        _limbs.Capacity = 8;
        for (int i = 0; i < 4; i++)
        {
            _limbs.Add(null);
        }
        _originalSize = _collider.size;
        _originalOffset = _collider.offset;
        _groundCheck = GetComponentInChildren<GroundCheck>().transform;
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
                if (_limbs[1].Size > _limbs[0].Size)
                {
                    MoveBodyUp(i);
                    _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + _limbs[0].Size);
                }
            }
            else
            {
                MoveBodyUp(i);
                _limbs[i].FlipX(-1);
            }
            _limbs[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            _limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x, _limbAnchors[i].position.y - _limbs[i].Size * 0.5f);
        }
        else
        {
            _limbs[i].Type = Limb.LimbType.Arm;
            _limbs[i].transform.rotation = Quaternion.Euler(0, 0, 90);
            if (i == 2)
            {
                _limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x - _limbs[i].Size * 0.5f, _limbAnchors[i].position.y);
                _limbs[i].FlipY(-1);
            }
            else if (i == 3)
            {
                _limbAnchors[i].position = new Vector3(_limbAnchors[i].position.x + _limbs[i].Size * 0.5f, _limbAnchors[i].position.y);
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
        _collider.size = new Vector2(_originalSize.x, _originalSize.y + _limbs[i].Size);
        _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y - _limbs[i].Size * 0.5f);
        _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y - _limbs[i].Size);
        transform.position = new Vector3(transform.position.x, transform.position.y + _limbs[i].Size);
    }

    //called when picking up a limb
    public void MoveBodyDown()
    {
        switch (_selectedLimb)
        {
            case SelectedLimb.RightArm:
                _limbAnchors[3].position = new Vector3(_limbAnchors[3].position.x - _limbs[3].Size * 0.5f, _limbAnchors[3].position.y);
                return;
            case SelectedLimb.LeftArm:
                _limbAnchors[2].position = new Vector3(_limbAnchors[2].position.x + _limbs[2].Size * 0.5f, _limbAnchors[2].position.y);
                return;
            case SelectedLimb.RightLeg:
                {
                    LegEdit(_limbs[1], _limbs[0], 1);
                    break;
                }
            case SelectedLimb.LeftLeg:
                {
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
            _collider.size = new Vector2(_originalSize.x, _originalSize.y);
            _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y);
            _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + current.Size);
            _limbAnchors[currentNum].position = new Vector3(_limbAnchors[currentNum].position.x, _limbAnchors[currentNum].position.y + current.Size * 0.5f);
        }
        else
        {
            _collider.size = new Vector2(_originalSize.x, _originalSize.y + other.Size);
            _collider.offset = new Vector2(_originalOffset.x, _originalOffset.y - other.Size * 0.5f);
            _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y + current.Size);
            _groundCheck.position = new Vector3(_groundCheck.position.x, _groundCheck.position.y - other.Size);
            _limbAnchors[currentNum].position = new Vector3(_limbAnchors[currentNum].position.x, _limbAnchors[currentNum].position.y + _limbs[currentNum].Size * 0.5f);
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
            if (_limbs[i] != null && _limbs[i].State == Limb.LimbState.Attached)
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
        else if (_limbs[(int)_selectedLimb].State != Limb.LimbState.Attached)
            return false;
        else if (!_canThrow)
            return false;

        return true;
    }

    public void ThrowLimb(int direction)
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
