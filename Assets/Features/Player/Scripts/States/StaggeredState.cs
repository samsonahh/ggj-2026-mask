using Animancer;
using DG.Tweening;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class StaggeredState : State<PlayerController>
    {
        [SerializeField] private ClipTransition _animationClip;
        [SerializeField] private float _staggerDuration = 0.5f;
        private float _timer;
        
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private float _modelDeathShakeIntensity = 0.5f;
        [SerializeField] private int _modelDeathShakeFrequency = 15;
        private Vector3 _modelStartLocalPos;
        
        private protected override void OnEnter()
        {
            _context.Animator.Play(_animationClip);

            _timer = 0f;
            
            _modelTransform.DOKill();
            _modelTransform.localPosition = _modelStartLocalPos;
            _modelTransform.DOShakePosition(_staggerDuration, _modelDeathShakeIntensity, _modelDeathShakeFrequency);
        }

        private protected override void OnExit()
        {
            _modelTransform.localPosition = _modelStartLocalPos;
        }

        private protected override void OnUpdate()
        {
            _timer += Time.deltaTime;
        }

        private protected override void OnFixedUpdate()
        {
            
        }

        private protected override State<PlayerController> GetTransition()
        {
            if (_timer > _staggerDuration)
                return _context.GroundedState;

            return null;
        }
    }
}