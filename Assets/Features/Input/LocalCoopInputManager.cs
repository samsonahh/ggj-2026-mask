using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalCoopInputManager : MonoBehaviour
{
    [Header("Join Actions")]
    [SerializeField, Required] private InputActionReference _joinKeyboardMainAction;
    [SerializeField, Required] private InputActionReference _joinKeyboardAltAction;
    [SerializeField, Required] private InputActionReference _joinGamepadAction;
    private bool _isAcceptingJoinInputs;
    private bool _isKeyboardMainOccupied;
    private bool _isKeyboardAltOccupied;
    
    [Header("References")]
    [SerializeField, Required] private PlayerInput _playerInputPrefab;
    [SerializeField, Required] private List<PlayerInputReader> _playerInputReaders = new List<PlayerInputReader>();

    [Header("Config")]
    [SerializeField] private int _maxPlayerCount = 2;
    
    public enum ControlScheme
    {
        KeyboardMain,
        KeyboardAlt,
        Gamepad
    }

    public static Dictionary<ControlScheme, string> ControlSchemeNames { get; set; } =
        new Dictionary<ControlScheme, string>()
        {
            { ControlScheme.KeyboardMain, "Keyboard&Mouse"},
            { ControlScheme.KeyboardAlt, "Keyboard&MouseAlt"},
            { ControlScheme.Gamepad, "Gamepad"},
        };

    private void Awake()
    {
        _isAcceptingJoinInputs = true;
        
        _joinKeyboardMainAction.action.Enable();
        _joinKeyboardAltAction.action.Enable();
        _joinGamepadAction.action.Enable();
        
        _joinKeyboardMainAction.action.performed += JoinMainKeyboardInput;
        _joinKeyboardAltAction.action.performed += JoinAltKeyboardInput;
        _joinGamepadAction.action.performed += JoinGamepadInput;
    }

    private void OnDestroy()
    {
        _joinKeyboardMainAction.action.performed -= JoinMainKeyboardInput;
        _joinKeyboardAltAction.action.performed -= JoinAltKeyboardInput;
        _joinGamepadAction.action.performed -= JoinGamepadInput;
    }

    private void BootstrapPlayerInput(InputAction.CallbackContext context, ControlScheme controlScheme)
    {
        Debug.Log($"Bootstrapping player input with control scheme '{controlScheme}' and device '{context.control.device.displayName}'");

        PlayerInput joinedInput = PlayerInput.Instantiate(
                _playerInputPrefab.gameObject,
                -1, 
                ControlSchemeNames[controlScheme], 
                -1,
                context.control.device
                ); 
        
        joinedInput.gameObject.name = $"Player{joinedInput.playerIndex}Input";

        PlayerInputReader targetReader = _playerInputReaders[joinedInput.playerIndex];
        targetReader.SetPlayerInput(joinedInput);
        targetReader.enabled = true;

        if (controlScheme == ControlScheme.KeyboardMain)
            _isKeyboardMainOccupied = true;
        else if(controlScheme == ControlScheme.KeyboardAlt)
            _isKeyboardAltOccupied = true;

        if (PlayerInput.all.Count >= _maxPlayerCount)
            _isAcceptingJoinInputs = false; // disable future joins
    }
    
    private void JoinMainKeyboardInput(InputAction.CallbackContext context)
    {
        if (!_isAcceptingJoinInputs)
            return;

        if (_isKeyboardMainOccupied)
            return;
        
        BootstrapPlayerInput(context, ControlScheme.KeyboardMain);
    }
    
    private void JoinAltKeyboardInput(InputAction.CallbackContext context)
    {
        if (!_isAcceptingJoinInputs)
            return;
        
        if (_isKeyboardAltOccupied)
            return;

        BootstrapPlayerInput(context, ControlScheme.KeyboardAlt);
    }
    
    private void JoinGamepadInput(InputAction.CallbackContext context)
    {
        if (!_isAcceptingJoinInputs)
            return;

        BootstrapPlayerInput(context, ControlScheme.Gamepad);
    }
}