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
        isActive = true;
        var clickableScript = obj.GetComponent<ClickableGameObject>();
        if (clickableScript != null)
        {
            clickableScript.ToggleActive();
            clickableScript.IsPopped = true;
        }
        GetComponent<CircleCollider2D>().enabled = false;
    }

    protected override void OnActive()
    {

        anim.SetBool("isPopped", true);
    }

    protected override void OnInactive()
    {
        anim.SetBool("isPopped", false);
        transform.position = obj.transform.position;
    }
}
