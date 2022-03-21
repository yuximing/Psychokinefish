using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject toggleActivateParticle;

    public AudioClip activateSfx;
    public AudioClip deactivateSfx;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 0, layerMask);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    var clickableScript = hit.collider.gameObject.GetComponent<ClickableGameObject>();
                    if (clickableScript != null)
                    {
                        if (clickableScript.IsActive) audioManager.PlayOneShot(deactivateSfx);
                        else audioManager.PlayOneShot(activateSfx);
                        clickableScript.ToggleActive();
                        Instantiate(toggleActivateParticle, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                    }
                    
                }
            }
            
        }
    }
}
