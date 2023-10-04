using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerLimbs))]
public class Player : MonoBehaviour
{
    public enum MovementState
    {
        Move,
        Jump
    };

    //Player components
    PlayerMovement _playerMovement;
    PlayerJump _playerJump;
    PlayerLimbs _playerLimbs;

    //
    // make all limbs get thrown from same place?
    //
    //[SerializeField] Transform _leftLaunchPoint;
    //[SerializeField] Transform _rightLaunchPoint;
    [SerializeField] Transform _aimTransform;
    [SerializeField] Transform _groundCheck;

    //input 
    [HideInInspector]
    public Vector2 _aim;
    float _throwLimbInput;

    //the location of the limb in the list dictates what limb it is
    //left leg
    //right leg
    //left arm
    //right arm

    public MovementState _movementState;

    //facing left = -1, right = 1
    public int direction;


    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<PlayerJump>();
        _playerLimbs = GetComponent<PlayerLimbs>();
    }


    void Update()
    {
        if (_playerMovement.facingRight)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        /*throwing limbs*/
        if (_throwLimbInput > 0.5f && _playerLimbs.CanThrowLimb()) 
        {
            _playerLimbs.ThrowLimb(direction);
        }

        //limb attack?


        /*horizontal movement*/

        _playerLimbs.CheckLimbState();
        
        _playerMovement.Move(_playerLimbs._limbState);

        /*vertical movement*/
        _playerJump.Jump();

        //reset limb throw
        if (_throwLimbInput == 0.0f)
        {
            _playerLimbs._canThrow = true;
        }

        //updating arrow
        if (_aim.x == 0.0f && _aim.y == 0.0f)
        {
            if (direction == 1)
            {
                _aimTransform.eulerAngles = new Vector3(0, 0, -180);
            }
            else
            {
                _aimTransform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            _aimTransform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-_aim.y, -_aim.x) * Mathf.Rad2Deg);
        }
    }

    public void ThrowLimbInput(InputAction.CallbackContext ctx) => _throwLimbInput = ctx.ReadValue<float>();

    public void AimInput(InputAction.CallbackContext ctx) => _aim = ctx.action.ReadValue<Vector2>();

}
