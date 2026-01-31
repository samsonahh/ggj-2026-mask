using System;
using PlayerStates;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> StateMachine { get; private set; }

    [field: SerializeField] public IdleState IdleState { get; private set; } = new();

    private void Awake()
    {
        SetupStateMachine();
    }

    private void Update()
    {
        StateMachine.Update();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void SetupStateMachine()
    {
        StateMachine = new StateMachine<PlayerController>(this);
        
        // Init any states here
        IdleState.Init(StateMachine, this);
        
        // Set the starting state
        StateMachine.ChangeState(IdleState,true);
    }
}

namespace PlayerStates
{
}