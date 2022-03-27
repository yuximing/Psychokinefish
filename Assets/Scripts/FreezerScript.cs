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

    public AudioClip freezerExplosionSfx;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    protected override void OnOffScreen()
    {

        if (isActive) Destroy(gameObject);
    }

    public override void ToggleActive()
    {
        isActive = hasLaunched || !isActive ? true : false;
        lineRenderer.enabled = isActive ? true : false;
    }

    protected override void OnActive()
    {
        if (Input.GetMouseButtonDown(0) && !hasLaunched)
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && hasLaunched)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            audioManager.PlayOneShot(freezerExplosionSfx);
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isActive && hasLaunched)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            audioManager.PlayOneShot(freezerExplosionSfx);
            Destroy(gameObject);
        }
    }

}
