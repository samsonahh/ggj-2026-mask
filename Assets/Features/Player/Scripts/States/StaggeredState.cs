using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class StaggeredState : State<PlayerController>
    {
        [SerializeField] private ClipTransition _animationClip;
        [SerializeField] private float _staggerDuration = 0.5f;
        private float _timer;
        
        private protected override void OnEnter()
        {
            _context.Animator.Play(_animationClip, 0.1f);

            _timer = 0f;
        }

        private protected override void OnExit()
        {
            
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