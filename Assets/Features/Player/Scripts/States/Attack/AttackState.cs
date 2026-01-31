using Animancer;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class AttackState : State<PlayerController>
    {
        [Header("Animation")]
        [SerializeField] private StringAsset _enableHitboxEventName;
        [SerializeField] private StringAsset _disableHitboxEventName;

        [SerializeField, SerializedDictionary("Type", "Config")]
        private SerializedDictionary<AttackType, AttackConfigSO> _attackConfigs = new();
        private AttackConfigSO _currentConfig;

        public void SetAttackType(AttackType attackType)
        {
            if (!_attackConfigs.TryGetValue(attackType, out AttackConfigSO config))
            {
                _currentConfig = null;
                return;
            }
            _currentConfig = config;
        }
        
        private protected override void OnEnter()
        {
            if (_currentConfig == null)
            {
                _context.StateMachine.ChangeState(_context.GroundedState);
                return;
            }
            
            _context.Animator.Play(_currentConfig.AnimationClip, 0.1f);
            
            _currentConfig.AnimationClip.Events.SetCallback(_enableHitboxEventName, EnableHitbox);
            _currentConfig.AnimationClip.Events.SetCallback(_disableHitboxEventName, DisableHitbox);
            _currentConfig.AnimationClip.Events.OnEnd = OnAnimationClipEnd;
        }

        private protected override void OnExit()
        {
            _currentConfig.AnimationClip.Events.RemoveCallback(_enableHitboxEventName, EnableHitbox);
            _currentConfig.AnimationClip.Events.RemoveCallback(_disableHitboxEventName, DisableHitbox);
            _currentConfig.AnimationClip.Events.OnEnd = null;
        }

        private protected override void OnUpdate()
        {
            
        }

        private protected override void OnFixedUpdate()
        {
            
        }

        private void OnAnimationClipEnd()
        {
            _stateMachine.ChangeState(_context.GroundedState);
        }
        
        private void EnableHitbox()
        {
            Debug.LogWarning("Hitbox enabled");
        }

        private void DisableHitbox()
        {
            Debug.LogWarning("Hitbox disabled");
        }
    }
}