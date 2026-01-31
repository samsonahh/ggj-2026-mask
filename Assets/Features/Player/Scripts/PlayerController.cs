using System;
using Animancer;
using PlayerStates;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField, Required] public AnimancerComponent Animator { get; private set; }
    [field: SerializeField, Required] public Rigidbody RigidBody { get; private set; }
    [field: SerializeField, Required] public CapsuleCollider CapsuleCollider { get; private set; }
    
    [field: SerializeField, Required] public PlayerInputReader Input { get; private set; }
    
    public StateMachine<PlayerController> StateMachine { get; private set; }
    [field: SerializeField] public GroundedSuperState GroundedState { get; private set; } = new();
    [field: SerializeField] public AirSuperState AirState { get; private set; } = new();
    [field: SerializeField] public AttackState AttackState { get; private set; } = new();
    
    [Header("Ground Check")]
    [SerializeField] private float _groundCheckLength = 0.5f;
    [SerializeField] private LayerMask _groundLayer;
    [field: SerializeField, ReadOnly] public bool IsGrounded { get; private set; }

    private void Awake()
    {
        SetupStateMachine();
    }

    private void Update()
    {
        StateMachine.Update();
        
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }

    private void SetupStateMachine()
    {
        StateMachine = new StateMachine<PlayerController>(this);
        
        // Init any states here
        GroundedState.Init(StateMachine, this);
        AirState.Init(StateMachine, this);
        AttackState.Init(StateMachine, this);
        
        // Set the starting state
        StateMachine.ChangeState(GroundedState,true);
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics.CheckSphere(transform.position + Vector3.down * _groundCheckLength, CapsuleCollider.radius * 0.9f, _groundLayer);
    }
    
    public void Move(Vector3 move)
    {
        RigidBody.MovePosition(RigidBody.position + move * Time.fixedDeltaTime);
    }
    
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position - Vector3.up * _groundCheckLength, CapsuleCollider.radius * 0.9f);
        }
#endif
    }
}