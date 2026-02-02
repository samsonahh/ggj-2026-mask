using System;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMashBridge : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    [SerializeField, Required] private Transform _chudFace;
    [SerializeField,  Required] private Transform _chumFace;
    [SerializeField,  Required] private MashMeter _mashMeter;
    private float _faceGroundedYOffset;

    [SerializeField] private float _targetOrthoSize = 1f;
    [SerializeField] private float _transitionDuration = 0.3f;
    [SerializeField] private Ease _transitionEaseType = Ease.OutCubic;

    private float _originalOrthoSize;
    private Vector3 _startingPosition;
    private Quaternion _startingRotation;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();

        Transform chudTransform = _chudFace.GetComponentInParent<PlayerController>().transform;
        _faceGroundedYOffset = (_chudFace.transform.position - chudTransform.position).y;
        
        _originalOrthoSize = _camera.m_Lens.OrthographicSize;
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;
    }

    private void OnEnable()
    {
        _mashMeter.OnMashStarted.AddListener(OnMashStarted);
        _mashMeter.OnWin.AddListener(OnMashFinished);
    }

    private void OnDisable()
    {
        _mashMeter.OnMashStarted.RemoveListener(OnMashStarted);
        _mashMeter.OnWin.RemoveListener(OnMashFinished);
    }

    private void OnMashStarted()
    {
        transform.DOKill();
        _camera.DOKill();

        DOVirtual.Float(_camera.m_Lens.OrthographicSize, _targetOrthoSize, _transitionDuration, (newSize) =>
        {
            _camera.m_Lens.OrthographicSize = newSize;
        }).SetEase(_transitionEaseType)
        .SetId(_camera);;
        
        Vector3 betweenPlayersPos = (_chudFace.transform.position + _chumFace.transform.position).WithY(_faceGroundedYOffset) / 2f;
        Vector3 outwardDirection = _startingPosition - betweenPlayersPos;
        transform.DOMove(betweenPlayersPos + outwardDirection.WithX(0f).WithY(0f), _transitionDuration).SetEase(_transitionEaseType);
        transform.DORotate(Vector3.zero, _transitionDuration).SetEase(_transitionEaseType);
    }

    private void OnMashFinished(MashMeter.Ripper winner)
    {
        transform.DOKill();
        _camera.DOKill();
        DOVirtual.Float(_camera.m_Lens.OrthographicSize, _originalOrthoSize, _transitionDuration, (newSize) =>
        {
            _camera.m_Lens.OrthographicSize = newSize;
        }).SetEase(_transitionEaseType)
        .SetId(_camera);
        transform.DOMove(_startingPosition, _transitionDuration).SetEase(_transitionEaseType);
        transform.DORotateQuaternion(_startingRotation, _transitionDuration).SetEase(_transitionEaseType);
    }
}