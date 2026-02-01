using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent<int, Vector3> OnFinalDamageTaken = new();

    [SerializeField] private GameObject _hitVfx;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerBlock = GetComponent<PlayerBlock>();
        _health = GetComponent<Health>();
    }

    public void TryTakeDamage(int damage, Vector3 hitPoint)
    {
        int finalDamage = damage;
        if (_playerController.StateMachine.CurrentState == _playerBlock.BlockState)
            finalDamage /= 2;
        
        GameObject.Instantiate(_hitVfx, hitPoint, Quaternion.identity);
        
        _health.TakeDamage(finalDamage);
        OnFinalDamageTaken?.Invoke(finalDamage, hitPoint);
    }
}