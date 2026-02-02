using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReadyUI : MonoBehaviour
{
    [SerializeField] private LocalCoopInputManager _localCoopInputManager;
    [SerializeField] private Color _readyImageColor = Color.green;
    [SerializeField] private float _readyFinishedDelayed = 1f;
    public UnityEvent OnReadyFinished = new UnityEvent();
    
    [Header("Left Ready")]
    [SerializeField] private Image _leftReadyImage;
    [SerializeField] private TMP_Text _leftReadyText;
    
    [Header("Right Ready")]
    [SerializeField] private Image _rightReadyImage;
    [SerializeField] private TMP_Text _rightReadyText;
    
    [Header("Juice")]
    [SerializeField] private float _readyImagePulseTargetScale = 1.25f;
    [SerializeField] private float _readyImagePulseTweenDuration = 0.25f;
    [SerializeField] private Ease _readyImagePulseEaseType = Ease.OutCirc;

    private int _readyStatus;

    private void Awake()
    {
        _localCoopInputManager.OnPlayerInputJoined += OnPlayerInputJoined;
    }

    private void OnDestroy()
    {
        _localCoopInputManager.OnPlayerInputJoined -= OnPlayerInputJoined;
    }

    private void OnPlayerInputJoined()
    {
        if (_readyStatus >= 2)
            return;
        
        _readyStatus++;
        if (_readyStatus == 1)
        {
            _leftReadyImage.color = _readyImageColor;
            PulseImage(_leftReadyImage);
            _leftReadyText.text = "Ready";
        }
        else if (_readyStatus == 2)
        {
            _rightReadyImage.color = _readyImageColor;
            PulseImage(_rightReadyImage);
            _rightReadyText.text = "Ready";
            DOVirtual.DelayedCall(_readyFinishedDelayed, () => OnReadyFinished.Invoke());
        }
    }

    private void PulseImage(Image image)
    {
        image.transform.DOKill();
        image.transform.localScale = Vector3.one;
        
        image.transform.DOScale(_readyImagePulseTargetScale, _readyImagePulseTweenDuration)
            .SetEase(_readyImagePulseEaseType)
            .SetLoops(2, LoopType.Yoyo);
    }
}