using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : ClickableGameObject
{
    public float speed = 5;

    [SerializeField]
    GameObject explosionPrefab;

    private Vector2 direction = Vector2.zero;
    readonly private float launchSpeed = 10.0f;
    private bool hasLaunched = false;

    private bool toRight = true;

    // Update is called once per frame
    public override void ToggleActive()
    {
        isActive = true;
    }

    protected override void OnOffScreen()
    {
        Destroy(gameObject);
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
        if(hasLaunched) transform.position += launchSpeed * Time.deltaTime * (Vector3)direction;
    }

    protected override void OnInactive()
    {
        Move();
    }

    void Move()
    {
        if (toRight)
        {
            transform.Translate(speed * Time.deltaTime * Vector2.right);
        }
        else
        {
            transform.Translate(speed * Time.deltaTime * -Vector2.right);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isActive && hasLaunched)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}