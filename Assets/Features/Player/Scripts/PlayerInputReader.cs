using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public PlayerInput PlayerInput { get; private set; }

    [field: SerializeField, ReadOnly] public int MoveInput { get; private set; }
    
    public event Action OnJump = delegate { };
    public event Action OnHit1 = delegate { };
    public event Action OnHit2 = delegate { };
    public event Action OnBlockStarted = delegate { };
    public event Action OnBlockReleased = delegate { };
    
    private InputAction _jumpAction;
    private InputAction _hit1Action;
    private InputAction _hit2Action;
    private InputAction _blockAction;
    private InputAction _moveAction;

    public void SetPlayerInput(PlayerInput playerInput)
    {
        UnsubscribeFromPlayerInput();

        PlayerInput = playerInput;

        _jumpAction = playerInput.actions["Jump"];
        _hit1Action = playerInput.actions["Hit1"];
        _hit2Action = playerInput.actions["Hit2"];
        _blockAction = playerInput.actions["Block"];
        _moveAction = playerInput.actions["Move"];
    
        _jumpAction.performed += InvokeJump;
        _hit1Action.performed += InvokeHit1;
        _hit2Action.performed += InvokeHit2;
        _blockAction.started += InvokeBlock;
        _blockAction.canceled += InvokeBlock;
        
        // Enable actions if this component is enabled
        if (enabled)
            EnableActions();
    }

    private void OnEnable()
    {
        EnableActions();
    }

    private void OnDisable()
    {
        DisableActions();
    }

    private void OnDestroy()
    {
        UnsubscribeFromPlayerInput();
    }

    private void EnableActions()
    {
        _jumpAction?.Enable();
        _hit1Action?.Enable();
        _hit2Action?.Enable();
        _blockAction?.Enable();
        _moveAction?.Enable();
    }

    private void DisableActions()
    {
        _jumpAction?.Disable();
        _hit1Action?.Disable();
        _hit2Action?.Disable();
        _blockAction?.Disable();
        _moveAction?.Disable();
    }

    private void UnsubscribeFromPlayerInput()
    {
        if (_jumpAction != null)
        {
            _jumpAction.performed -= InvokeJump;
            _jumpAction.Disable();
        }
    
        if (_hit1Action != null)
        {
            _hit1Action.performed -= InvokeHit1;
            _hit1Action.Disable();
        }
    
        if (_hit2Action != null)
        {
            _hit2Action.performed -= InvokeHit2;
            _hit2Action.Disable();
        }
    
        if (_blockAction != null)
        {
            _blockAction.started -= InvokeBlock;
            _blockAction.canceled -= InvokeBlock;
            _blockAction.Disable();
        }
        
        _moveAction?.Disable();
    }

    private void Update()
    {
        if (_moveAction != null && enabled)
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            MoveInput = (int)input.x;
        }
        else
        {
            MoveInput = 0;
        }
    }
    
    public void InvokeJump(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        
        if (context.performed)
            OnJump.Invoke();    
    }
    
    public void InvokeHit1(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        
        if(context.performed)
            OnHit1.Invoke();
    }
    
    public void InvokeHit2(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        
        if(context.performed)
            OnHit2.Invoke();
    }
    
    public void InvokeBlock(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;
        
        if(context.started)
            OnBlockStarted.Invoke();
        else if(context.canceled)
            OnBlockReleased.Invoke();
    }
}