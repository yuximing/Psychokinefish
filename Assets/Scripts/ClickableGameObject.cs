using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickableGameObject : MonoBehaviour
{
    protected bool isActive = false;
    public bool IsActive { get { return isActive; } }
    protected bool isPopped = false;
    public bool IsPopped { get { return isPopped; } set { isPopped = value; } }

    protected virtual void Update()
    {
        if (isActive) OnActive();
        else OnInactive();
    }

    protected virtual void FixedUpdate()
    {
        if (isActive) OnActiveFixed();
        else OnInactiveFixed();
        if (CameraScroll.IsSpriteOffScreen(gameObject)) OnOffScreen();
    }

    protected virtual void OnOffScreen()
    {
        isActive = false;
    }
    

    public virtual void ToggleActive()
    {
        isActive = !isActive;
    }
    protected abstract void OnActive();
    protected abstract void OnInactive();

    protected virtual void OnActiveFixed() { }

    protected virtual void OnInactiveFixed() { }
}
