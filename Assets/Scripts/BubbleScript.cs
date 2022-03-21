using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : ClickableGameObject
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void ToggleActive()
    {
        base.ToggleActive();
    }

    protected override void OnActive()
    {
        anim.SetBool("isPopped", true);
    }

    protected override void OnInactive()
    {
        anim.SetBool("isPopped", false);
    }
}
