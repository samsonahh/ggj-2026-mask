using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class BlockState : State<PlayerController>
    {
        [SerializeField] private ClipTransition _animationClip;
        
        private protected override void OnEnter()
        {
            _context.Animator.Play(_animationClip, 0.1f);
            
            _context.Input.OnBlockReleased += OnBlockReleased;
        }

        private protected override void OnExit()
        {
            _context.Input.OnBlockReleased -= OnBlockReleased;
        }

        private protected override void OnUpdate()
        {
            
        }

        private protected override void OnFixedUpdate()
        {
            
        }
        
        private void OnBlockReleased()
        {
            _stateMachine.ChangeState(_context.GroundedState);
        }
    }
}