using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : ClickableGameObject
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public override void ToggleActive()
    {
        base.ToggleActive();
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (isActive)
        {
            spriteRenderer.color = Color.green;
        } else
        {
            spriteRenderer.color = Color.cyan;
        }
    }

    protected override void OnActive()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, mousePosition - (Vector2)transform.position));

    }

    protected override void OnInactive()
    {
        
    }
}