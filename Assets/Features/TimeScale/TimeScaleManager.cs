using System;
using System.Collections;
using UnityEngine;

public class TimeScaleManager : Singleton<TimeScaleManager>
{
    public float DefaultFixedDeltaTime { get; private set; }
    private float _previousTimeScale = 1f;
    
    private float _impactFramesRemainingTime;
    private Coroutine _impactFramesCoroutine;
    
    private void Start()
    {
        DefaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// Sets the time scale of the game.
    /// You can specify whether to save the previous time scale value.
    /// </summary>
    /// <param name="timeScale">The new time scale value.</param>
    /// <param name="trackPrevious">Whether to save the previous time scale value.</param>
    public void SetTimeScale(float timeScale, bool trackPrevious = false)
    {
        if (trackPrevious) 
            _previousTimeScale = Time.timeScale;
        else 
            _previousTimeScale = 1f;
        
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = DefaultFixedDeltaTime * timeScale;
    }
    
    public void StartImpactFrames(float timeScale, float duration)
    {
        if (duration <= 0) 
            return;

        if(_impactFramesCoroutine != null)
        {
            _impactFramesRemainingTime = Mathf.Max(_impactFramesRemainingTime, duration);
            return;
        }
        
        _impactFramesRemainingTime = duration;
        _impactFramesCoroutine = StartCoroutine(ImpactFramesCoroutine(timeScale));
    }
    
    private IEnumerator ImpactFramesCoroutine(float timeScale)
    {
        SetTimeScale(timeScale);

        while (_impactFramesRemainingTime > 0f)
        {
            _impactFramesRemainingTime -= Time.unscaledDeltaTime;
            yield return null;
        }
        _impactFramesRemainingTime = 0f;

        SetTimeScale(1);
        _impactFramesCoroutine = null;
    }
}