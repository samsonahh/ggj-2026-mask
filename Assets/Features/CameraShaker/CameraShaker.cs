using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShaker : Singleton<CameraShaker>
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private float _startingAmplitude;
    private float _startingFrequency;
    private float _shakeTimer;
    private float _shakeDuration;
    
    public bool IsShaking => _shakeTimer > 0;

    private protected override void Awake()
    {
        base.Awake();
        
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    
    [Button("Test Shake")]
    public void ShakeCamera(float amplitude, float frequency, float duration)
    {
        _cinemachineBasicMultiChannelPerlin.ReSeed();

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        _cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequency;
        _startingAmplitude = amplitude;
        _startingFrequency = frequency;
        _shakeDuration = duration;
        _shakeTimer = duration;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.unscaledDeltaTime;

            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(_startingAmplitude, 0, 1 - (_shakeTimer / _shakeDuration));
            _cinemachineBasicMultiChannelPerlin.m_FrequencyGain =
                Mathf.Lerp(_startingFrequency, 0, 1 - (_shakeTimer / _shakeDuration));
        }
    }
}