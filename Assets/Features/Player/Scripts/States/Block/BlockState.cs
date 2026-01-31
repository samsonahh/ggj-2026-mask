namespace PlayerStates
{
    [System.Serializable]
    public class BlockState : State<PlayerController>
    {
        private protected override void OnEnter()
        {
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