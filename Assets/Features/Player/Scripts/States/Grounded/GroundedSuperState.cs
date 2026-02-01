using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class GroundedSuperState : SuperState<PlayerController>
    {
        public override State<PlayerController> InitialSubState => IdleState;
        [field: SerializeField] public GroundedIdleState IdleState { get; private set; } = new();
        [field: SerializeField] public GroundedMoveState MoveState { get; private set; } = new();

        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private StringAsset _jumpSfxName;
        
        private protected override void InitializeSubStates()
        {
            IdleState.Init(SubStateMachine, _context);
            MoveState.Init(SubStateMachine, _context);
        }
        
        private protected override void OnEnter()
        {
            _context.Input.OnJump += Jump;
        }

        private protected override void OnExit()
        {
            _context.Input.OnJump -= Jump;
        }

        private protected override void OnUpdate()
        {
            
        }

        private protected override void OnFixedUpdate()
        {
            
        }
        
        private protected override State<PlayerController> GetTransition()
        {
            if (!_context.IsGrounded)
                return _context.AirState;
            
            return null;
        }

        private void Jump()
        {
            float jumpForce = CustomUtils.GetJumpForce(_jumpHeight, Physics.gravity.y);
            _context.RigidBody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            
            AudioManager.Instance.Play(_jumpSfxName, AudioManager.MixerTarget.SFX, _context.transform.position, Random.Range(0.9f, 1.1f));
        }
    }
}