using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [System.Serializable]
    public class MaskRipVictimState : State<PlayerController>
    {
        [SerializeField] private ClipTransition _animationClip;
        
        private protected override void OnEnter()
        {
            _context.Animator.Play(_animationClip);
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
    }
}