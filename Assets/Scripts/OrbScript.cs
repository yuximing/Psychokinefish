using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : ClickableGameObject
{
    public float speed = 5;

    private Rigidbody2D rb;

    private Vector2 direction = Vector2.zero;
    private float launchSpeed = 10.0f;
    private bool hasLaunched = false;

    private bool toRight = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public override void ToggleActive()
    {
        /* base.ToggleActive() would be what you would call to activate/deactivate the object
         * and do something else with it after such as change a variable (speed, color, size, etc.)
         * 
         * In this case, we don't want to go from on to off so it is overriden to set
         * isActive to true.
         */
        // base.ToggleActive();
        //
        // Do something here. For ex.
        // speed = 3.0f;

        isActive = true;
    }

    // MAKE SURE TO OVERRIDE USING 'override'!!!
    protected override void OnActive()
    {
        if (Input.GetMouseButtonUp(1) && !hasLaunched)
        {
            Vector2 toMouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (toMouseVec.sqrMagnitude > 0.0f)
            {
                direction = toMouseVec.normalized;
            } else
            {
                direction = Vector2.right;
            }
            hasLaunched = true;
        }

        if (!hasLaunched) Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if(hasLaunched) transform.position += (Vector3) direction * launchSpeed * Time.deltaTime;
    }

    protected override void OnInactive()
    {
        Move();
    }

    void Move()
    {
        if (toRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector2.right * speed * Time.deltaTime);
        }

        if (transform.position.x >= 4.0f)
        {
            toRight = false;
        }
        if (transform.position.x <= -4.10f)
        {
            toRight = true;
        }
    }
}