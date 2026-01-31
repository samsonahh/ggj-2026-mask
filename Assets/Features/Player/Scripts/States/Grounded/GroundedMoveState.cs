using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class GroundedMoveState : State<PlayerController>
    {
        [SerializeField] private float _moveSpeed = 4f;
        
        private protected override void OnEnter()
        {
            
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