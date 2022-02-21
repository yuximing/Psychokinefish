using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezerScript : ClickableGameObject
{
    [SerializeField]
    GameObject explosionPrefab;

    LineRenderer lineRenderer;

    private Vector2 direction = Vector2.zero;
    private float launchSpeed = 50.0f;
    private bool hasLaunched = false;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    protected override void OnOffScreen()
    {
        Destroy(gameObject);
    }

    public override void ToggleActive()
    {
        isActive = true;
        lineRenderer.enabled = true;
    }

    protected override void OnActive()
    {
        if (Input.GetMouseButtonUp(1) && !hasLaunched)
        {
            Vector2 toMouseVec = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (toMouseVec.sqrMagnitude > 0.0f)
            {
                direction = toMouseVec.normalized;
            }
            else
            {
                direction = Vector2.right;
            }
            hasLaunched = true;
            lineRenderer.enabled = false;
        }

        if (!hasLaunched)
        {
            lineRenderer.SetPosition(0, (Vector2)transform.position);
            lineRenderer.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (hasLaunched)
        {
            transform.position += (Vector3)direction * launchSpeed * Time.deltaTime;
        }
    }

    protected override void OnInactive()
    {
        
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
