using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider delayedHealthSlider;
    [SerializeField] private Health playerHealth;
    
    [Header("Delayed Difference Config")]
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float delayAnimDuration = 0.15f;
    [SerializeField] private Ease delayEaseType = Ease.OutCirc;
    
    [Header("Shake Config")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeIntensity = 10f;
    [SerializeField] private int shakeFrequency = 25;

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void Start()
    {
        OnDamage(0);
    }

    private void OnEnable()
    {
        playerHealth.onDamage.AddListener(OnDamage);
    }

    private void OnDisable()
    {
        playerHealth.onDamage.RemoveListener(OnDamage);
    }

    private void OnDamage(int damage)
    {
        transform.DOKill();
        delayedHealthSlider.DOKill();
        
        transform.position = _startPosition;
        transform.DOShakePosition(shakeDuration, shakeIntensity, shakeFrequency);
        
        float targetSliderValue = playerHealth.currentHP/(float)playerHealth.maxHP;
        healthSlider.value = targetSliderValue;
        DOVirtual.DelayedCall(delay, () =>
        {
            delayedHealthSlider.DOValue(targetSliderValue, delayAnimDuration).SetEase(delayEaseType);
        })
        .SetId(delayedHealthSlider);
    }
}
