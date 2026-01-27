using System;
using UnityEngine;

/// <summary>
/// Makes this object always face the main camera.
/// Attach to any object you want to billboard.
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField] private bool _flip;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera == null)
            return;

        Vector3 direction = transform.position - _mainCamera.transform.position;
        if(_flip)
            direction = -direction;
        
        transform.rotation = Quaternion.LookRotation(direction);
    }
}