using System;
using Animancer;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEvent = UnityEngine.Events.UnityEvent;

public class ReadyUI : MonoBehaviour
{
    [SerializeField] private LocalCoopInputManager _localCoopInputManager;
    [SerializeField] private Color _readyImageColor = Color.green;
    [SerializeField] private float _readyFinishedDelayed = 1f;
    [SerializeField] private StringAsset _readySfxName;
    public UnityEvent OnReadyFinished = new UnityEvent();
    
    [Header("Title")]
    [SerializeField] private Transform _titleTransform;
    [SerializeField] private float _titlePulseInterval = 0.75f;
    [SerializeField] private float _titlePulseTargetSize = 1.1f;
    [SerializeField] private Ease _titlePulseEaseType = Ease.OutCirc;
    
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

    private void Start()
    {
        _titleTransform.DOScale(_titlePulseTargetSize, _titlePulseInterval)
            .SetEase(_titlePulseEaseType)
            .SetLoops(-1, LoopType.Yoyo);
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
        AudioManager.Instance.Play(_readySfxName, AudioManager.MixerTarget.UI);
        
        image.transform.DOKill();
        image.transform.localScale = Vector3.one;
        
        image.transform.DOScale(_readyImagePulseTargetScale, _readyImagePulseTweenDuration)
            .SetEase(_readyImagePulseEaseType)
            .SetLoops(2, LoopType.Yoyo);
    }
}