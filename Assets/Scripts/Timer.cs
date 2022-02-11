using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    float timeLeft;
    float coolDownTime;

    /*
     * Timer constructor
     * 
     * This constructor sets the initial timeLeft to 0.
     * To have the timer initially count down from coolDownTime, 
     * use the second constructor.
     */
    public Timer(float coolDownTime) : this(coolDownTime, true)
    {
        
    }

    public Timer(float coolDownTime, bool initiallyReady)
    {
        Debug.Assert(coolDownTime >= 0.0f);
        this.coolDownTime = coolDownTime;
        timeLeft = initiallyReady ? 0.0f : coolDownTime;
    }

    /*
     * Steps the timer forward
     * 
     * Call this at the <strong>very</strong> start of Update()
     */
    public void Tick()
    {
        timeLeft = Mathf.Max(0.0f, timeLeft - Time.deltaTime);
    }


    public bool IsReady()
    {
        return timeLeft == 0.0f;
    }

    /*
     * Resets Timer if timer is ready.
     * 
     * Returns true if reset was successful, else return false.
     */
    public bool ResetTimer()
    {
        if (IsReady())
        {
            ResetTimerUnsafe();
            return true;
        }
        return false;
        
    }

    /*
     * Reset timer regardless of whether it is ready or not.
     */
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
    public float TimeRatio
    {
        get
        {
            return timeLeft / coolDownTime;
        }
    }
}
