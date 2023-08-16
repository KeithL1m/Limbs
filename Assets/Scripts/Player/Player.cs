using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
public class Player : MonoBehaviour
{
    public enum LimbState
    {
        TwoLeg,
        OneLeg,
        NoLimb
    };

    public enum MovementState
    {
        Move,
        Jump
    };

    public enum SelectedLimb
    {
        LeftLeg,
        RightLeg,
        LeftArm,
        RightArm
    };

    //Player components
    PlayerMovement _playerMovement;
    PlayerJump _playerJump;
    PlayerHealth _health;

    public float _throwLimbInput;
    bool _canThrow;

    public float _strafeInput;


    //the location of the limb in the list dictates what limb it is
    //left leg
    //right leg
    //left arm
    //right arm
    public List<Limb> _limbs;
    [SerializeField] List<Transform> _limbAnchors;

    //Colliders
    public BoxCollider2D _limbCollider;
    public BoxCollider2D _playerCollider;


    public LimbState _limbState;
    public MovementState _movementState;
    SelectedLimb _selectedLimb = SelectedLimb.LeftLeg;

    //facing left = -1, right = 1
    public int direction;


    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<PlayerJump>();
        _limbs = new List<Limb>();
        _limbs.Capacity = 4;
        for (int i = 0; i < 4; i++)
        {
            _limbs.Add(null);
        }

    }


    void Update()
    {
        //for testing
        if (_limbs[(int)_selectedLimb] != null)
        {
            _limbs[(int)_selectedLimb].GetComponent<SpriteRenderer>().color = Color.red;
        }


        if (_playerMovement.facingRight)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        /*throwing limbs*/
        if (_throwLimbInput > 0.5f && _limbs[(int)_selectedLimb] != null && _limbs[(int)_selectedLimb]._limbState == Limb.LimbState.Attached && _canThrow) 
        {
            if (_strafeInput > 0.5f)
            {
                direction *= -1;
            }
            _limbs[(int)_selectedLimb].ThrowLimb(direction);
            if (_selectedLimb != SelectedLimb.LeftLeg)
            {
                _selectedLimb--;
            }
            _canThrow = false;
        }


        //limb attack?


        /*horizontal movement*/

        CheckLimbState();
        
        _playerMovement.Move(_limbState);

        /*vertical movement*/
        _playerJump.Jump();

        if (_throwLimbInput == 0.0f)
        {
            _canThrow = true;
        }
    }

    public bool CanPickUpLimb(Limb limb)
    {
        if (limb._limbState != Limb.LimbState.PickUp && limb._limbState != Limb.LimbState.Returning)
        {
            return false;
        }

        //check if limb is alread picked up
        for (int i = 0; i < 4; i++)
        {
            if (_limbs[i] == limb)
            {
                return false;
            }
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

    private void AttachLimb(int i)
    {
        if (i == 0 || i == 1)
        {
            _limbs[i]._limbType = Limb.LimbType.Leg;
            if (i == 1)
            {
                _limbs[(int)_selectedLimb].GetComponent<SpriteRenderer>().color = Color.green;
                _selectedLimb++;
            }
        }
        else
        {
            _limbs[(int)_selectedLimb].GetComponent<SpriteRenderer>().color = Color.green;
            _selectedLimb++;
            _limbs[i]._limbType = Limb.LimbType.Arm;
        }

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), _limbs[i].GetComponent<Collider2D>(), true);
        _limbs[i]._anchorPoint = _limbAnchors[i].transform;
        _limbs[i]._limbState = Limb.LimbState.Attached;
        _limbs[i].GetComponent<Rigidbody2D>().simulated = false;
    }

    public void RemoveLimb(Limb limb)
    {
        for (int i = 0; i < _limbs.Count; i++)
        {
            if (limb == _limbs[i])
            {
                _limbs[i] = null;
            }
        }
    }

    public void ThrowLimbInput(InputAction.CallbackContext ctx) => _throwLimbInput = ctx.ReadValue<float>();

    public void StrafeInput(InputAction.CallbackContext ctx) => _strafeInput = ctx.ReadValue<float>();

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
}
