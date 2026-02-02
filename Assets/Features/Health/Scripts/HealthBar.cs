using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider delayedHealthSlider;
    public Image healthSliderFill;
    public Image delayedHealthSliderFill;
    [SerializeField] private Health playerHealth;
    
    [Header("Delayed Difference Config")]
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float delayAnimDuration = 0.15f;
    [SerializeField] private Ease delayEaseType = Ease.OutCirc;
    [SerializeField] private Color healTargetColor = Color.green;
    [SerializeField] private Color damageTargetColor = Color.red;
    
    [Header("Shake Config")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeIntensity = 10f;
    [SerializeField] private int shakeFrequency = 25;
    
    [Header("Heal Config")]
    [SerializeField] private Color healthBarHealColor = Color.green;
    private Color healthBarDefaultColor;
    
    private Vector3 _startLocalPosition;

    private void Awake()
    {
        _startLocalPosition = transform.localPosition;
        healthBarDefaultColor = healthSliderFill.color;
    }

    private void Start()
    {
        OnDamage(0);
    }

    private void OnEnable()
    {
        playerHealth.onDamage.AddListener(OnDamage);
        playerHealth.onHeal.AddListener(OnHeal);
    }

    private void OnDisable()
    {
        playerHealth.onDamage.RemoveListener(OnDamage);
        playerHealth.onHeal.AddListener(OnHeal);
    }

    private void OnHeal(int healAmount)
    {
        transform.DOKill();
        healthSlider.DOKill();
        delayedHealthSlider.DOKill();
        healthSliderFill.DOKill();
        
        healthSliderFill.color = healthBarDefaultColor;
        delayedHealthSliderFill.color = healTargetColor;
        transform.localPosition = _startLocalPosition;

        float targetSliderValue = playerHealth.currentHP/(float)playerHealth.maxHP;
        delayedHealthSlider.value = targetSliderValue;
        DOVirtual.DelayedCall(delay, () =>
        {
            healthSlider.DOValue(targetSliderValue, delayAnimDuration).SetEase(delayEaseType);
        })
        .SetId(healthSlider);
        healthSliderFill.DOColor(healthBarHealColor, delay + delayAnimDuration)
            .SetEase(delayEaseType)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                healthSliderFill.color = healthBarDefaultColor;
            });
    }

    private void OnDamage(int damage)
    {
        transform.DOKill();
        healthSlider.DOKill();
        delayedHealthSlider.DOKill();
        healthSliderFill.DOKill();
        
        healthSliderFill.color = healthBarDefaultColor;
        delayedHealthSliderFill.color = damageTargetColor;
        transform.localPosition = _startLocalPosition;
        transform.DOShakePosition(shakeDuration, shakeIntensity, shakeFrequency);
        
        float targetSliderValue = playerHealth.currentHP/(float)playerHealth.maxHP;
        healthSlider.value = targetSliderValue;
        DOVirtual.DelayedCall(delay, () =>
        {
            delayedHealthSlider.DOValue(targetSliderValue, delayAnimDuration).SetEase(delayEaseType);
        })
        .SetId(delayedHealthSlider);
        
        healthSliderFill.DOColor(healthBarHealColor, delay + delayAnimDuration)
            .SetEase(delayEaseType)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                healthSliderFill.color = healthBarDefaultColor;
            });
    }
}
