using System;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using UnityEvent = UnityEngine.Events.UnityEvent;

public class MashMeter : MonoBehaviour
{
    [field: SerializeField] public float MaxMashMeterValue { get; private set; } = 100f;
    [field: SerializeField, ReadOnly] public float MashMeterValue { get; private set; }
    
    [SerializeField] private float _mashStrength = 1f;
    [SerializeField] private float _ripperPassiveMashSpeed = 0.25f;

    public enum Ripper
    {
        Ripper,
        Victim
    }

    [field: SerializeField] public UnityEvent OnMashStarted { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Ripper> OnWin { get; private set; } = new();
        
    [Header("Mash")]
    [SerializeField] private StringAsset _stretchOneShotSfxName;
    [SerializeField] private StringAsset _ripWinSfxName;
    [SerializeField] private StringAsset _ripFailSfxName;
    
    [Header("Passive Juice")]
    [SerializeField] private float _passiveCameraShakeInterval = 0.5f;
    [SerializeField] private float _passiveCameraShakeAmplitude = 2f;
    [SerializeField] private float _passiveCameraShakeFrequency = 7.5f;
    private float _passiveCameraShakeTimer;
    
    [Header("On Finish Juice")]
    [SerializeField] private float _cameraShakeDuration = 2f;
    [SerializeField] private float _cameraShakeAmplitude = 10f;
    [SerializeField] private float _cameraShakeFrequency = 10f;
    [SerializeField] private float _hitStopTimeScale = 0.01f;
    [SerializeField] private float _hitStopDuration = 0.3f;

    private bool _isMashing;
    
    public event Action OnMash = delegate { };
    public bool IsFlipped { get; private set; }

    private void Awake()
    {
        ResetMashMeter();
    }

    public void ResetMashMeter()
    {
        MashMeterValue = MaxMashMeterValue / 2f;
    }

    public void StartMash()
    {
        _isMashing = true;
        _passiveCameraShakeTimer = 0f;
        OnMashStarted.Invoke();
    }

    public void SetFlipped(bool flipped) => IsFlipped = flipped;

    private void Update()
    {
        if (!_isMashing)
            return;
        
        // passive cam shake
        _passiveCameraShakeTimer += Time.deltaTime;
        if (_passiveCameraShakeTimer >= _passiveCameraShakeInterval)
        {
            _passiveCameraShakeTimer = 0f;
            if(!CameraShaker.Instance.IsShaking)
                CameraShaker.Instance.ShakeCamera(_passiveCameraShakeAmplitude, _passiveCameraShakeFrequency, _passiveCameraShakeInterval);
        }
        
        // ripper has meter slowly creep towards their side
        MashMeterValue = Mathf.Clamp(MashMeterValue - _ripperPassiveMashSpeed * Time.deltaTime, 0f, MaxMashMeterValue);
        if(MashMeterValue <= 0f)
            Win(Ripper.Ripper);
        else if(MashMeterValue >= MaxMashMeterValue)
            Win(Ripper.Victim);
    }

    public void AddRipperMash()
    {
        MashMeterValue = Mathf.Clamp(MashMeterValue - _mashStrength, 0f, MaxMashMeterValue);
        
        OnMeterMashed();
        
        if(MashMeterValue <= 0f)
            Win(Ripper.Ripper);
    }

    public void AddVictimMash()
    {
        MashMeterValue = Mathf.Clamp(MashMeterValue + _mashStrength, 0f, MaxMashMeterValue);

        OnMeterMashed();
        
        if(MashMeterValue >= MaxMashMeterValue)
            Win(Ripper.Victim);
    }

    private void OnMeterMashed()
    {
        OnMash?.Invoke();
        AudioManager.Instance.Play(_stretchOneShotSfxName, AudioManager.MixerTarget.SFX, null, Random.Range(0.8f, 1.2f));
    }

    private void Win(Ripper winner)
    {
        _isMashing = false;

        if (winner == Ripper.Ripper)
        {
            AudioManager.Instance.Play(_ripWinSfxName, AudioManager.MixerTarget.SFX);
            AudioManager.Instance.Play(_ripFailSfxName, AudioManager.MixerTarget.SFX, null, Random.Range(0.4f, 1.6f));
        }
        
        TimeScaleManager.Instance.StartImpactFrames(_hitStopTimeScale, _hitStopDuration);
        CameraShaker.Instance.ShakeCamera(_cameraShakeAmplitude, _cameraShakeFrequency, _cameraShakeDuration);
        OnWin?.Invoke(winner);
    }
}