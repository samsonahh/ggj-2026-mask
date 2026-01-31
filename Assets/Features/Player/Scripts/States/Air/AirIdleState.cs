namespace PlayerStates
{
    [System.Serializable]
    public class AirIdleState : State<PlayerController>
    {
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
            if (_context.Input.MoveInput != 0)
                return _context.AirState.MoveState;
            
            return null;
        }
    }
}