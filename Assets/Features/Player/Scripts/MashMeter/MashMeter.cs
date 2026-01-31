using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class MashMeter : MonoBehaviour
{
    [field: SerializeField] public float MaxMashMeterValue { get; private set; } = 100f;
    [field: SerializeField, ReadOnly] public float MashMeterValue { get; private set; }

    [SerializeField] private float _ripperMashStrength = 1.1f;
    [SerializeField] private float _victimMashStrength = 1f;

    public enum Ripper
    {
        Ripper,
        Victim
    }

    [field: SerializeField] public UnityEvent OnMashStarted { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Ripper> OnWin { get; private set; } = new();
    
    [Header("On Finish Camera Shake")]
    [SerializeField] private float _cameraShakeDuration = 2f;
    [SerializeField] private float _cameraShakeAmplitude = 10f;
    [SerializeField] private float _cameraShakeFrequency = 10f;

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
        OnMashStarted.Invoke();
    }

    public void AddRipperMash()
    {
        MashMeterValue = Mathf.Clamp(MashMeterValue - _ripperMashStrength, 0f, MaxMashMeterValue);
        
        if(MashMeterValue <= 0f)
            Win(Ripper.Ripper);
    }

    public void AddVictimMash()
    {
        MashMeterValue = Mathf.Clamp(MashMeterValue + _victimMashStrength, 0f, MaxMashMeterValue);
        
        if(MashMeterValue >= MaxMashMeterValue)
            Win(Ripper.Victim);
    }

    private void Win(Ripper winner)
    {
        CameraShaker.Instance.ShakeCamera(_cameraShakeAmplitude, _cameraShakeFrequency, _cameraShakeDuration);
        OnWin?.Invoke(winner);
    }
}