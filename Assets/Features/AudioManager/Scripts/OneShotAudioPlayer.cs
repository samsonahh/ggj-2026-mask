using Animancer;
using UnityEngine;

public class OneShotAudioPlayer : MonoBehaviour
{
    [SerializeField] private StringAsset _audioSfxName;
    [SerializeField] private AudioManager.MixerTarget _mixerTarget;
    [SerializeField] private Transform _transform;
    [SerializeField] private FloatRange _randomPitchRange = new FloatRange(0.8f, 1.2f);
    
    public void PlayAudio()
    {
        AudioManager.Instance.Play(_audioSfxName, _mixerTarget, _transform == null ? null : _transform.position, _randomPitchRange.RandomValue());
    }
}