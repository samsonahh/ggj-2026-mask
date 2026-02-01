using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class AirSuperState : SuperState<PlayerController>
    {
        public override State<PlayerController> InitialSubState => IdleState;
        [field: SerializeField] public AirIdleState IdleState { get; private set; } = new();
        [field: SerializeField] public AirMoveState MoveState { get; private set; } = new();
        
        [SerializeField] private StringAsset _landSfxName;
        
        private protected override void InitializeSubStates()
        {
            IdleState.Init(SubStateMachine, _context);
            MoveState.Init(SubStateMachine, _context);
        }
        
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
            
        }

        private protected override State<PlayerController> GetTransition()
        {
            if (_context.IsGrounded)
            {
                AudioManager.Instance.Play(_landSfxName, AudioManager.MixerTarget.SFX, _context.transform.position, Random.Range(0.9f, 1.1f));
                return _context.GroundedState;
            }
            
            return null;
        }
    }
}