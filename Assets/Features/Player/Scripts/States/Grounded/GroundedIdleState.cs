using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class GroundedIdleState : State<PlayerController>
    {
        [SerializeField] private ClipTransition _animationClip;
        
        private protected override void OnEnter()
        {
            _context.Animator.Play(_animationClip, 0.1f);
        }

        private protected override void OnExit()
        {
            
        }

        private protected override void OnUpdate()
        {
            
        }

        private protected override void OnFixedUpdate()
        {
            
        }

        private protected override State<PlayerController> GetTransition()
        {
            if (_context.Input.MoveInput != 0)
                return _context.GroundedState.MoveState;
            
            return null;
        }
    }
}