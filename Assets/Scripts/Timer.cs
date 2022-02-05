using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    float timeLeft;
    float coolDownTime;

    public Timer(float coolDownTime)
    {
        Debug.Assert(coolDownTime >= 0.0f);
        this.coolDownTime = coolDownTime;
        timeLeft = 0.0f;
    }

    public Timer(float coolDownTime, bool initiallyActive) : this(coolDownTime)
    {
        if (!initiallyActive) timeLeft = coolDownTime;
    }

    public void Tick()
    {
        timeLeft = Mathf.Max(0.0f, timeLeft - Time.deltaTime);
    }

    public bool IsReady()
    {
        return timeLeft == 0.0f;
    }

    public bool ResetTimer()
    {
        if (IsReady())
        {
            ResetTimerUnsafe();
            return true;
        }
        return false;
        
    }

    public void ResetTimerUnsafe()
    {
        timeLeft = coolDownTime;
    }

    public void ChangeCoolDownTime(float newTime)
    {
        Debug.Assert(newTime >= 0.0f);
        coolDownTime = newTime;
        timeLeft = Mathf.Min(timeLeft, coolDownTime);
    }
}
