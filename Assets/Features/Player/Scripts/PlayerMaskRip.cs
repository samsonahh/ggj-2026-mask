using System;
using PlayerStates;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerInputReader))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerBlock))]
public class PlayerMaskRip : MonoBehaviour
{
    private Health _health;
    private PlayerController _playerController;
    public PlayerInputReader InputReader {get; private set;}
    private PlayerAttack _playerAttack;
    private PlayerBlock _playerBlock;
    
    [SerializeField, Required] private PlayerMaskRip _otherPlayerMaskRip;
    [SerializeField, Required] private MashMeter _mashMeter;
    
    [field: SerializeField] public MaskRippingState MaskRippingState { get; private set; }  
    [field: SerializeField] public MaskRipVictimState MaskRipVictimState { get; private set; }
    
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
    }

    private void Start()
    {
        MaskRippingState.Init(_playerController.StateMachine, _playerController);
        MaskRipVictimState.Init(_playerController.StateMachine, _playerController);
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
        // start mask ripping
        StartRipper();
        _otherPlayerMaskRip.StartVictim();

        InputReader.OnHit1 += Ripper_OnMash;
        _otherPlayerMaskRip.InputReader.OnHit1 += Victim_OnMash;

        _mashMeter.ResetMashMeter();
        _mashMeter.StartMash();
        _mashMeter.OnWin.AddListener(OnMashFinished);
    }

    private void OnMaskRippingFailed()
    {
        _otherPlayerMaskRip.Revive();
        
        EndRipping();
        _otherPlayerMaskRip.EndRipping();
        
        InputReader.OnHit1 -= Ripper_OnMash;
        _otherPlayerMaskRip.InputReader.OnHit1 -= Victim_OnMash;
    }

    private void OnMaskRippingSuccess()
    {
        EndRipping();
        _otherPlayerMaskRip.EndRipping();
        
        InputReader.OnHit1 -= Ripper_OnMash;
        _otherPlayerMaskRip.InputReader.OnHit1 -= Victim_OnMash;
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
    
    private void DisablePlayer()
    {
        _playerAttack.enabled = false;
        _playerBlock.enabled = false;
    }

    private void EnablePlayer()
    {
        _playerAttack.enabled = true;
        _playerBlock.enabled = true;
    }

    private void ShakeCamera()
    {
        CameraShaker.Instance.ShakeCamera(_mashCameraShakeAmplitude, _mashCameraShakeFrequency, _mashCameraShakeDuration);
    }
}