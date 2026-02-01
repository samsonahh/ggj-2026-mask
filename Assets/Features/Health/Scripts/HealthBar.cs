using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    [SerializeField] private Health playerHealth;
    
    [Header("Shake Config")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeIntensity = 10f;
    [SerializeField] private int shakeFrequency = 25;

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        healthSlider.value = playerHealth.currentHP/(float)playerHealth.maxHP;
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
        
        transform.position = _startPosition;
        transform.DOShakePosition(shakeDuration, shakeIntensity, shakeFrequency);
    }
}
