using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : ClickableGameObject
{
    [SerializeField]
    GameObject obj;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void ToggleActive()
    {
        base.ToggleActive();
        var clickableScript = obj.GetComponent<ClickableGameObject>();
        if (clickableScript != null)
        {
            clickableScript.ToggleActive();
            clickableScript.IsPopped = true;
        }
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
