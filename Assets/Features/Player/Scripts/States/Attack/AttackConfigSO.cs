using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "AttackConfig")]
    public class AttackConfigSO : ScriptableObject
    {
        [field: SerializeField] public ClipTransition AnimationClip { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }
}