using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MashMeterUI : MonoBehaviour
{
    [SerializeField] private MashMeter _meter;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _buttonImage;
    
    [Header("Button Pulse")]
    [SerializeField] private float _buttonPulseDuration = 0.15f;
    [SerializeField] private float _buttonPulseScale = 1.2f;
    [SerializeField] private Ease _buttonPulseEaseType = Ease.OutCubic;

    private void Awake()
    {
        _meter.OnMashStarted.AddListener(OnMashStarted);
        _meter.OnMash += OnMash;
    }

    private void OnDestroy()
    {
        _meter.OnMashStarted.RemoveListener(OnMashStarted);
        _meter.OnMash -= OnMash;
    }

    private void OnMashStarted()
    {
        // _slider.direction = _meter.IsFlipped ? Slider.Direction.RightToLeft : Slider.Direction.LeftToRight;
    }

    private void OnMash()
    {
        _buttonImage.transform.localScale = Vector3.one;
        _buttonImage.transform.DOKill();
        _buttonImage.transform.DOScale(Vector3.one * _buttonPulseScale, _buttonPulseDuration)
            .SetEase(_buttonPulseEaseType)
            .SetLoops(2, LoopType.Yoyo);
    }

    private void LateUpdate()
    {
        if (!_meter.IsFlipped)
            _slider.value = _meter.MashMeterValue / _meter.MaxMashMeterValue;
        else
            _slider.value = 1f - (_meter.MashMeterValue / _meter.MaxMashMeterValue);
    }
}