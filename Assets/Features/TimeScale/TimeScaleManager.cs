using System;
using UnityEngine;

public class TimeScaleManager : Singleton<TimeScaleManager>
{
    public float DefaultFixedDeltaTime { get; private set; }
    private float _previousTimeScale = 1f;

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
}