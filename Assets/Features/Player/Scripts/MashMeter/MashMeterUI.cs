using System;
using UnityEngine;
using UnityEngine.UI;

public class MashMeterUI : MonoBehaviour
{
    [SerializeField] private MashMeter _meter;
    [SerializeField] private Slider _slider;

    private void LateUpdate()
    {
        _slider.value = _meter.MashMeterValue / _meter.MaxMashMeterValue;
    }
}