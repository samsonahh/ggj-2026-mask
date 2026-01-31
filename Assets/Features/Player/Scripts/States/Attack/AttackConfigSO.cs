using Animancer;
using UnityEngine;

namespace PlayerStates
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "AttackConfig")]
    public class AttackConfigSO : ScriptableObject
    {
        [field: SerializeField] public ClipTransition AnimationClip { get; private set; }
        [field: SerializeField] public int Damage { get; private set; } = 1;
        
        [field: Header("Impact Frames")]
        [field: SerializeField] public float ImpactFramesTimeScale { get; private set; } = 0.05f;
        [field: SerializeField] public float ImpactFramesDuration { get; private set; } = 0.25f;
        
        [field: Header("Camera Shake")]
        [field: SerializeField] public float CameraShakeDuration { get; private set; } = 0.25f;
        [field: SerializeField] public float CameraShakeAmplitude { get; private set; } = 5f;
        [field: SerializeField] public float CameraShakeFrequency { get; private set; } = 5f;
        
        public void SetClip(ClipTransition clipTransition) => AnimationClip = clipTransition;
    }
}