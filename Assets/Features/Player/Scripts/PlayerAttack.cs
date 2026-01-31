using System;
using PlayerStates;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAttack : MonoBehaviour
{
    private PlayerController _player;
    
    [field: SerializeField] public AttackState AttackState { get; private set; } = new();
    [field: SerializeField] public StaggeredState StaggeredState { get; private set; } = new();

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void Start()
    {
        AttackState.Init(_player.StateMachine, _player);
        StaggeredState.Init(_player.StateMachine, _player);
    }

    private void OnEnable()
    {
        _player.Input.OnHit1 += OnHit1;
        _player.Input.OnHit2 += OnHit2;
    }

    private void OnDisable()
    {
        _player.Input.OnHit1 -= OnHit1;
        _player.Input.OnHit2 -= OnHit2;
    }

    private void OnHit1()
    {
        if (!CanHit())
            return;
        
        AttackState.SetAttackType(_player.IsGrounded ? AttackType.GroundPunch : AttackType.AirPunch);
        _player.StateMachine.ChangeState(AttackState, true);
    }
    
    private void OnHit2()
    {
        if (!CanHit())
            return;
        
        AttackState.SetAttackType(_player.IsGrounded ? AttackType.GroundKick : AttackType.AirKick);
        _player.StateMachine.ChangeState(AttackState, true);
    }

    private bool CanHit()
    {
        if (_player.StateMachine.CurrentState == AttackState)
            return false;
        
        if(_player.StateMachine.CurrentState == StaggeredState)
            return false;

        return true;
    }
}