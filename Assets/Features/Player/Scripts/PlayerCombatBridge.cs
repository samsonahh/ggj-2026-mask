using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerBlock))]
[RequireComponent(typeof(Health))]
public class PlayerCombatBridge : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerAttack _playerAttack;
    private PlayerBlock _playerBlock;
    private Health _health;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerBlock = GetComponent<PlayerBlock>();
        _health = GetComponent<Health>();
    }

    public void TryTakeDamage(int damage)
    {
        if (_playerController.StateMachine.CurrentState == _playerBlock.BlockState)
        {
            _health.TakeDamage(damage/2);
            return;
        }

        _health.TakeDamage(damage);
        _playerController.StateMachine.ChangeState(_playerAttack.StaggeredState, true);
    }
}