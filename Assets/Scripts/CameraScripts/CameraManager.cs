using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public CameraFocus _focusLevel;

    public List<GameObject> _players;
    public List<PlayerInput> _playerList = new List<PlayerInput>();

    public float depthUpdateSpeed = 5.0f;
    public float angleUpdateSpeed = 7.0f;
    public float positionUpdateSpeed = 5.0f;

    public float depthMax = -10.0f;
    public float depthMin = -22.0f;

    public float angleMax = 11.0f;
    public float angleMin = 3.0f;

    private float _CameraEulerX;
    private Vector3 CameraPosition;

    void Start()
    {
        _players.Add(_focusLevel.gameObject);  
    }

    private void LateUpdate()
    {
        CalculateCameraLocation();
        MoveCamera();
    }

    private void MoveCamera()
    {

    }

    private void CalculateCameraLocation()
    {
        Vector3 averageCenter = Vector3.zero;
        Vector3 totalPositions = Vector3.zero;
        Bounds playerBounds = new Bounds();

        for (int i = 0; i < _playerList.Count; i++)
        {
            Vector3 playerPosition = _playerList[i].transform.position;

            if(!_focusLevel.focusBounds.Contains(playerPosition))
            {
                float playerX = Mathf.Clamp(playerPosition.x, _focusLevel.focusBounds.min.x, _focusLevel.focusBounds.max.x);
                float playerY = Mathf.Clamp(playerPosition.y, _focusLevel.focusBounds.min.y, _focusLevel.focusBounds.max.y);
                float playerZ = Mathf.Clamp(playerPosition.z, _focusLevel.focusBounds.min.z, _focusLevel.focusBounds.max.z);
            }

            totalPositions += playerPosition;
            playerBounds.Encapsulate(playerPosition);
        }

        averageCenter = (totalPositions / _playerList.Count);
    }
}
