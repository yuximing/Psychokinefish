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

    private bool is_popped = false;

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
            //GameObject top = null;

            //foreach (RaycastHit2D hit in hits)
            //{
            //    SpriteRenderer spriteRenderer = hit.transform.GetComponent<SpriteRenderer>();
            //    //if (spriteRenderer != null) Debug.Log(spriteRenderer.sortingLayerName);


            //    if (hit.collider != null && spriteRenderer != null && spriteRenderer.sortingLayerName == "Poppables")
            //    {
            //        var clickableScript = hit.collider.gameObject.GetComponent<ClickableGameObject>();
            //        if (clickableScript != null)
            //        {
            //            if (clickableScript.IsActive) audioManager.PlayOneShot(deactivateSfx);
            //            else audioManager.PlayOneShot(activateSfx);
            //            clickableScript.ToggleActive();
            //            Instantiate(toggleActivateParticle, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            //        }

            //        break;
            //    }
            //}

            // if not popped, ToggleActive bubble, play activation sound
            if (!is_popped)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    SpriteRenderer spriteRenderer = hit.transform.GetComponent<SpriteRenderer>();
                    if (hit.collider != null && spriteRenderer != null && spriteRenderer.sortingLayerName == "Poppables")
                    {
                        var clickableScript = hit.collider.gameObject.GetComponent<ClickableGameObject>();
                        if (clickableScript != null)
                        {
                            audioManager.PlayOneShot(activateSfx);
                            clickableScript.ToggleActive();
                            Instantiate(toggleActivateParticle, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                        }
                        is_popped = true;
                        break;
                    }
                }
            }
            // if popped, ToggleActive object, play deactivation sound
            else
            {
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
}
