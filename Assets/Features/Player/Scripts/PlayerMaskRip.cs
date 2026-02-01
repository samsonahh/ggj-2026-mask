using System;
using Animancer;
using DG.Tweening;
using PlayerStates;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerBlock))]
[RequireComponent(typeof(Damageable))]
public class PlayerMaskRip : MonoBehaviour
{
    private Health _health;
    private PlayerController _playerController;
    public PlayerInputReader InputReader {get; private set;}
    private PlayerAttack _playerAttack;
    private PlayerBlock _playerBlock;
    private Damageable _damageable;
    
    [SerializeField, Required] private PlayerMaskRip _otherPlayerMaskRip;
    [SerializeField, Required] private MashMeter _mashMeter;

    [SerializeField] private float _startMaskRipDelay = 0.5f;
    [SerializeField] private StringAsset _deathSfxName;
    [SerializeField] private ClipTransition _deathAnimClip;
    [SerializeField] private Transform _modelTransform;
    [SerializeField] private float _modelDeathShakeIntensity = 10f;
    [SerializeField] private int _modelDeathShakeFrequency = 20;
    private Vector3 _modelStartLocalPos;
    
    [field: SerializeField] public MaskRippingState MaskRippingState { get; private set; }  
    [field: SerializeField] public MaskRipVictimState MaskRipVictimState { get; private set; }
    [field: SerializeField] public WinState WinState { get; private set; }
    [field: SerializeField] public LoseState LoseState { get; private set; }
    
    [Header("Camera Shake")]
    [SerializeField] private float _mashCameraShakeDuration = 0.25f;
    [SerializeField] private float _mashCameraShakeAmplitude = 5f;
    [SerializeField] private float _mashCameraShakeFrequency = 5f;
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _playerController = GetComponent<PlayerController>();
        InputReader = GetComponent<PlayerInputReader>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerBlock = GetComponent<PlayerBlock>();
        _damageable = GetComponent<Damageable>();
        
        _modelStartLocalPos = _modelTransform.localPosition;
    }

    private void Start()
    {
        MaskRippingState.Init(_playerController.StateMachine, _playerController);
        MaskRipVictimState.Init(_playerController.StateMachine, _playerController);
        WinState.Init(_playerController.StateMachine, _playerController);
        LoseState.Init(_playerController.StateMachine, _playerController);
    }

    private void OnEnable()
    {
        _health.onDeath.AddListener(OnDeath);
    }

    private void OnDisable()
    {
        _health.onDeath.RemoveListener(OnDeath);
    }

    private void OnDeath()
    {
        DisablePlayer();
        DOVirtual.DelayedCall(_startMaskRipDelay, StartMaskRip);

        _playerController.StateMachine.ChangeState(_playerController.GroundedState, true); // kick player out of staggered state
        _modelTransform.DOKill();
        _modelTransform.localPosition = _modelStartLocalPos;
        _modelTransform.DOShakePosition(_startMaskRipDelay, _modelDeathShakeIntensity, _modelDeathShakeFrequency);
        
        _playerController.Animator.Play(_deathAnimClip, 0.1f);
        
        AudioManager.Instance.Play(_deathSfxName, AudioManager.MixerTarget.SFX, null, Random.Range(0.25f, 1.75f));
    }

    private void StartMaskRip()
    {
        _modelTransform.localPosition = _modelStartLocalPos;
        
        StartVictim();
        _otherPlayerMaskRip.StartRipper();

        InputReader.OnHit1 += Victim_OnMash;
        _otherPlayerMaskRip.InputReader.OnHit1 += Ripper_OnMash;

        _mashMeter.ResetMashMeter();
        _mashMeter.StartMash();
        _mashMeter.OnWin.AddListener(OnMashFinished);
    }

    private void OnMaskRippingSuccess()
    {
        Lose();
        _otherPlayerMaskRip.Win();
        
        InputReader.OnHit1 -= Victim_OnMash;
        _otherPlayerMaskRip.InputReader.OnHit1 -= Ripper_OnMash;
    }

    private void OnMaskRippingFailed()
    {
        Revive();
        
        EndRipping();
        _otherPlayerMaskRip.EndRipping();
        
        InputReader.OnHit1 -= Victim_OnMash;
        _otherPlayerMaskRip.InputReader.OnHit1 -= Ripper_OnMash;
    }

    private void Ripper_OnMash()
    {
        ShakeCamera();
        _mashMeter.AddRipperMash();
    }
    
    private void Victim_OnMash()
    {
        ShakeCamera();
        _mashMeter.AddVictimMash();
    }

    private void OnMashFinished(MashMeter.Ripper player)
    {
        if(player == MashMeter.Ripper.Ripper)
            OnMaskRippingSuccess();
        else
            OnMaskRippingFailed();
        
        _mashMeter.OnWin.RemoveListener(OnMashFinished);
    }

    public void StartRipper()
    {
        DisablePlayer();
        _playerController.StateMachine.ChangeState(MaskRippingState);
    }
    
    public void StartVictim()
    {
        DisablePlayer();
        _playerController.StateMachine.ChangeState(MaskRipVictimState);
    }

    public void EndRipping()
    {
        EnablePlayer();
        _playerController.StateMachine.ChangeState(_playerController.GroundedState);
    }

    public void Revive()
    {
        Debug.Log(gameObject.name + " Revived");
        _health.revive();
    }

    public void Win()
    {
        _playerController.StateMachine.ChangeState(WinState, true);
    }

    public void Lose()
    {
        _playerController.StateMachine.ChangeState(LoseState, true);
    }
    
    public void DisablePlayer()
    {
        _playerAttack.enabled = false;
        _playerBlock.enabled = false;
        _damageable.enabled = false;
    }

    public void EnablePlayer()
    {
        _playerAttack.enabled = true;
        _playerBlock.enabled = true;
        _damageable.enabled = true;
    }

    private void ShakeCamera()
    {
        CameraShaker.Instance.ShakeCamera(_mashCameraShakeAmplitude, _mashCameraShakeFrequency, _mashCameraShakeDuration);
    }
}