using System;
using UnityEngine;

namespace TD.Script.GUI
{
    public class FixedCameraScene : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private const float TargetSizeX = 1920;
        private float _targetPosition;
        private int _targetPositionZ = -10;
        private readonly float _cameraAdjustment = 4f;
        private float _orthographicSize;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Awake()
        {
            ScrollCamera();
        }

        private void ScrollCamera()
        {
            _orthographicSize = Screen.width / TargetSizeX;

            if (_orthographicSize % 2 <= 1)
            {
                _targetPosition += _orthographicSize + _cameraAdjustment;
            }
            else
            {
                _targetPosition += _orthographicSize;
            }

            if (TargetSizeX < Screen.width)
            {
                _camera.transform.position = new Vector3(transform.position.x + _targetPosition, transform.position.y,
                    _targetPositionZ);
            }
        }
    }
}