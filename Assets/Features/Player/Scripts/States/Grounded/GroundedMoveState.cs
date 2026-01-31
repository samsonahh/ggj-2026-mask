using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class GroundedMoveState : State<PlayerController>
    {
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private ClipTransition _animationClip;
        [SerializeField] private Transform _modelTransform;
        
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
            Vector3 direction = Vector3.zero;
            if(_context.Input.MoveInput > 0)
                direction = Vector3.right;
            else if (_context.Input.MoveInput < 0)
                direction = Vector3.left;

            // Mult by scale to flip anim when flipped
            _animationClip.Speed = Mathf.Sign(_context.Input.MoveInput) * Mathf.Abs(_animationClip.Speed) * Mathf.Sign(_modelTransform.localScale.z);
            _context.Move(direction * _moveSpeed);
        }
        
        private protected override State<PlayerController> GetTransition()
        {
            if (_context.Input.MoveInput == 0)
                return _context.GroundedState.IdleState;
            
            return null;
        }
    }
}