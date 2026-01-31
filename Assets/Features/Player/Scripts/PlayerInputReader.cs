using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [field: SerializeField, Required] public PlayerInput PlayerInput { get; private set; }

    [field: SerializeField, ReadOnly] public int MoveInput { get; private set; }
    
    public event Action OnJump = delegate { };
    public event Action OnHit1 = delegate { };
    public event Action OnHit2 = delegate { };
    public event Action OnBlockHeld = delegate { };
    public event Action OnBlockReleased = delegate { };
    
    public void InvokeMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        MoveInput = (int)input.x;
    }
    
    public void InvokeJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnJump.Invoke();    
    }
    
    public void InvokeHit1(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnHit1.Invoke();
    }
    
    public void InvokeHit2(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnHit2.Invoke();
    }
    
    public void InvokeBlock(InputAction.CallbackContext context)
    {
        if(context.started)
            OnBlockHeld.Invoke();
        else if(context.canceled)
            OnBlockReleased.Invoke();
    }
}