using System;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraMashBridge : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    [SerializeField, Required] private PlayerController _chud;
    [SerializeField,  Required] private PlayerController _chum;
    [SerializeField,  Required] private MashMeter _mashMeter;

    [SerializeField] private float _targetOrthoSize = 1f;
    [SerializeField] private float _transitionDuration = 0.3f;
    [SerializeField] private Ease _transitionEaseType = Ease.OutCubic;

    private float _originalOrthoSize;
    private Vector3 _startingPosition;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        
        _originalOrthoSize = _camera.m_Lens.OrthographicSize;
        _startingPosition = transform.position;
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
        
        Vector3 betweenPlayersPos = (_chum.transform.position + _chum.transform.position) / 2f;
        Vector3 outwardDirection = _startingPosition - betweenPlayersPos;
        transform.DOMove(betweenPlayersPos + outwardDirection.WithX(0f).WithZ(0f), _transitionDuration).SetEase(_transitionEaseType);
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
    }
}