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
        Vector3 position = gameObject.transform.position;
        if( position != CameraPosition)
        {
            Vector3 targetPosition = Vector3.zero;
            targetPosition.x = Mathf.MoveTowards(position.x, CameraPosition.x, positionUpdateSpeed * Time.deltaTime);
            targetPosition.y = Mathf.MoveTowards(position.y, CameraPosition.y, positionUpdateSpeed * Time.deltaTime);
            targetPosition.z = Mathf.MoveTowards(position.z, CameraPosition.z, depthUpdateSpeed * Time.deltaTime);
            gameObject.transform.position = targetPosition;
        }

        Vector3 localEulerAngles = gameObject.transform.localEulerAngles;
        if(localEulerAngles.x != _CameraEulerX)
        {
            Vector3 targetEulerAngles = new Vector3(_CameraEulerX, localEulerAngles.y, localEulerAngles.z);
            gameObject.transform.localEulerAngles = Vector3.MoveTowards(localEulerAngles, targetEulerAngles, angleUpdateSpeed * Time.deltaTime);
        }
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
                playerPosition = new Vector3(playerX, playerY, playerZ);
            }

            totalPositions += playerPosition;
            playerBounds.Encapsulate(playerPosition);
        }

        averageCenter = (totalPositions / _playerList.Count);

        float extents = (playerBounds.extents.x + playerBounds.extents.y);
        float lerpPercent = Mathf.InverseLerp(0, (_focusLevel._halfXBounds + _focusLevel._halfYBounds) / 2, extents);

        float depth = Mathf.Lerp(depthMax, depthMin, lerpPercent);
        float angle = Mathf.Lerp(angleMax, angleMin, lerpPercent);

        _CameraEulerX = angle;
        CameraPosition = new Vector3(averageCenter.x, averageCenter.y, depth);
    }
}
