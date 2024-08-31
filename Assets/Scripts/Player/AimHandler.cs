using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimHandler
{
    private PlayerInputHandler _inputHandler;
    private PlayerMovement _playerMovement;

    //facing left = -1, right = 1
    public int Direction { get { return _direction; } }
    private int _direction;
    private Vector2 _lastAimed = Vector2.zero;
    private bool _aimConfused;

    public AimHandler(PlayerInputHandler input, PlayerMovement movement)
    {
        _inputHandler = input;
        _playerMovement = movement;
    }

    

    public void SetLastAimed()
    {
        if (_lastAimed != new Vector2(_inputHandler.Aim.x, _inputHandler.Aim.y) && _inputHandler.FlickAiming)
        {
            if (_inputHandler.Aim.magnitude > 0.6f)
            {
                if (_inputHandler.Aim.x != 0.0f && _inputHandler.Aim.y != 0.0f)
                {
                    _lastAimed = new Vector2(_inputHandler.Aim.x, _inputHandler.Aim.y);
                }
            }
        }
    }

    public void SetDirection()
    {
        if (_playerMovement.facingRight)
        {
            if (_aimConfused)
            {
                _direction = -1;
            }
            else
            {
                _direction = 1;
            }
        }
        else
        {
            if (_aimConfused)
            {
                _direction = 1;
            }
            else
            {
                _direction = -1;
            }
        }
    }

    public (Vector3, bool) GetAimResults()
    {
        (Vector3, bool) results;

        if (_inputHandler.Aim.x == 0.0f && _inputHandler.Aim.y == 0.0f && !_inputHandler.FlickAiming)
        {
            if (_direction == 1)
            {
                results.Item1 = new Vector3(0, 0, -180);
            }
            else
            {
                results.Item1 = new Vector3(0, 0, 0);
            }

            if (_playerMovement.facingRight)
            {
                results.Item2 = false;
            }
            else
            {
                results.Item2 = true;
            }
        }
        else if (!_inputHandler.FlickAiming)
        {
            results.Item1 = new Vector3(0, 0, Mathf.Atan2(-_inputHandler.Aim.y, -_inputHandler.Aim.x) * Mathf.Rad2Deg);
            if (_inputHandler.Aim.x > 0)
            {
                results.Item2 = false;
            }
            else
            {
                results.Item2 = true;
            }
        }
        else
        {
            results.Item1 = new Vector3(0, 0, Mathf.Atan2(-_lastAimed.y, -_lastAimed.x) * Mathf.Rad2Deg);
            if (_lastAimed.x > 0)
            {
                results.Item2 = false;
            }
            else
            {
                results.Item2 = true;
            }
        }

        return results;
    }

    public void MakeAimOpposite()
    {
        _aimConfused = true;
    }

    public void MakeAimNormal()
    {
        _aimConfused = false;
    }
}
