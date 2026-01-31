using System;
using PlayerStates;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerBlock : MonoBehaviour
{
    private PlayerController _player;
    
    [field: SerializeField] public BlockState BlockState { get; private set; } = new();

    [SerializeField, ReadOnly] private bool _isBlockHeld;
    
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void Start()
    {
        BlockState.Init(_player.StateMachine, _player);
    }

    private void OnEnable()
    {
        _isBlockHeld = false;
        
        _player.Input.OnBlockStarted += OnBlockStarted;
        _player.Input.OnBlockReleased += OnBlockReleased;
    }

    private void OnDisable()
    {
        _isBlockHeld = false;
        
        _player.Input.OnBlockStarted -= OnBlockStarted;
        _player.Input.OnBlockReleased -= OnBlockReleased;
    }

    private void Update()
    {
        if (!_isBlockHeld)
            return;

        if (!_player.IsGrounded)
            return;

        if (_player.StateMachine.CurrentState == BlockState)
            return;
        
        _player.StateMachine.ChangeState(BlockState);
    }

    private void OnBlockStarted()
    {
        _isBlockHeld = true;
    }
    
    private void OnBlockReleased()
    {
        _isBlockHeld = false;
    }
}