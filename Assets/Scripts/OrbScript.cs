using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : ClickableGameObject
{
    public float speed = 5;

    [SerializeField]
    GameObject explosionPrefab;

    LineRenderer lineRenderer;

    private Vector2 direction = Vector2.zero;
    readonly private float launchSpeed = 50.0f;
    private bool hasLaunched = false;

    private bool toRight = true;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    public override void ToggleActive()
    {
        isActive = true;
        lineRenderer.enabled = true;
    }

    protected override void OnOffScreen()
    {

        if(isActive) Destroy(gameObject);
    }

    // MAKE SURE TO OVERRIDE USING 'override'!!!
    protected override void OnActive()
    {
        if (Input.GetMouseButtonDown(0) && !hasLaunched)
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

        if (!hasLaunched)
        {
            lineRenderer.SetPosition(0, (Vector2) transform.position);
            lineRenderer.SetPosition(1, (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Debug.DrawLine(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (hasLaunched)
        {
            lineRenderer.enabled = false;
            transform.position += launchSpeed * Time.deltaTime * (Vector3)direction;
        }
    }

    protected override void OnInactive()
    {
        //Move();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && hasLaunched)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}