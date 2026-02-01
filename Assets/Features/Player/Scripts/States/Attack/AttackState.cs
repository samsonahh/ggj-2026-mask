using System.Collections.Generic;
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
        private StringAsset _enableHitboxEventNameInstance;
        private StringAsset _disableHitboxEventNameInstance;

        [SerializeField, SerializedDictionary("Type", "Config")]
        private SerializedDictionary<AttackType, AttackConfigSO> _attackConfigs = new();
        private Dictionary<AttackType, AttackConfigSO> _attackConfigInstances = new();
        [SerializeField, SerializedDictionary("Type", "Weapon")]
        private SerializedDictionary<AttackType, Weapon> _attackWeapons = new();
        private AttackConfigSO _currentConfig;
        private Weapon _currentWeapon;
        
        private protected override void OnInit()
        {
            // Create runtime instances of all the clip events since players share the same event and clip data
            _enableHitboxEventNameInstance = ScriptableObject.Instantiate(_enableHitboxEventName);
            _disableHitboxEventNameInstance = ScriptableObject.Instantiate(_disableHitboxEventName);
            
            foreach (var kvp in _attackConfigs)
            {
                _attackConfigInstances.Add(kvp.Key, GetAttackConfigInstanceCopy(kvp.Value));
            }
        }
        
        private AttackConfigSO GetAttackConfigInstanceCopy(AttackConfigSO config)
        {
            AttackConfigSO configInstance = ScriptableObject.Instantiate(config);
            
            ClipTransition instanceClip = new ClipTransition();
            instanceClip.CopyFrom(configInstance.AnimationClip);
            
            var events = instanceClip.Events;
            if (events != null && events.Count > 0)
            {
                for (int i = 0; i < events.Count; i++)
                {
                    var stringAsset = events.Names[i];
                    if (stringAsset == _enableHitboxEventName)
                        events.Names[i] = _enableHitboxEventNameInstance;
                    else if (stringAsset == _disableHitboxEventName)
                        events.Names[i] = _disableHitboxEventNameInstance;
                }
            }

            configInstance.SetClip(instanceClip);
            return configInstance;
        }
        
        public void SetAttackType(AttackType attackType)
        {
            _currentConfig = _attackConfigInstances.GetValueOrDefault(attackType);
            _currentWeapon = _attackWeapons.GetValueOrDefault(attackType);
            
            if(_currentConfig == null)
                Debug.LogError($"No attack config associated with {attackType}");
            if(_currentWeapon == null)
                Debug.LogError($"No weapon associated with {attackType}");
        }
        
        private protected override void OnEnter()
        {
            if (_currentConfig == null || _currentWeapon == null)
            {
                _context.StateMachine.ChangeState(_context.GroundedState);
                return;
            }
            
            _currentWeapon.ClearObjectHitList();
            _currentWeapon.ConfigureImpactFrames(_currentConfig.ImpactFramesTimeScale, _currentConfig.ImpactFramesDuration);
            _currentWeapon.ConfigureDamage(_currentConfig.Damage);
            _currentWeapon.OnWeaponHit += OnWeaponHit;
            
            _context.Animator.Play(_currentConfig.AnimationClip, 0.1f);
            
            _currentConfig.AnimationClip.Events.SetCallback(_enableHitboxEventNameInstance, EnableHitbox);
            _currentConfig.AnimationClip.Events.SetCallback(_disableHitboxEventNameInstance, DisableHitbox);
            _currentConfig.AnimationClip.Events.OnEnd = OnAnimationClipEnd;
        }

        private protected override void OnExit()
        {
            _currentWeapon.DisableTriggers();
            _currentWeapon.OnWeaponHit -= OnWeaponHit;
            
            _currentConfig.AnimationClip.Events.RemoveCallback(_enableHitboxEventNameInstance, EnableHitbox);
            _currentConfig.AnimationClip.Events.RemoveCallback(_disableHitboxEventNameInstance, DisableHitbox);
            _currentConfig.AnimationClip.Events.OnEnd = null;
        }

        private protected override void OnUpdate()
        {
            
        }

        private protected override void OnFixedUpdate()
        {
            if(!_context.IsGrounded)
                _context.Move(Vector3.right * _context.LastHorizontalVelocity);
        }

        private void OnAnimationClipEnd()
        {
            if(_context.IsGrounded)
                _stateMachine.ChangeState(_context.GroundedState);
            else
                _stateMachine.ChangeState(_context.AirState);
        }
        
        private void EnableHitbox()
        {
            if (_currentWeapon == null)
                return;
            
            _currentWeapon.EnableTriggers();
        }

        private void DisableHitbox()
        {
            if (_currentWeapon == null)
                return;
            
            _currentWeapon.DisableTriggers();
        }

        private void OnWeaponHit(Damageable victim, Vector3 hitPoint)
        {
            if (_currentConfig == null)
                return;
            
            CameraShaker.Instance.ShakeCamera(_currentConfig.CameraShakeAmplitude, _currentConfig.CameraShakeFrequency, _currentConfig.CameraShakeDuration);
            AudioManager.Instance.Play(_currentConfig.SfxName, AudioManager.MixerTarget.SFX, hitPoint, Random.Range(0.8f, 1.2f));
        }
    }
}