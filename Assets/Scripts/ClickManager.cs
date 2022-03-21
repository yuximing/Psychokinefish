using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject toggleActivateParticle;

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

    //        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0, layerMask);

    //        if (hit.collider != null)
    //        {
    //            var clickableScript = hit.collider.gameObject.GetComponent<ClickableGameObject>();
    //            if (clickableScript != null)
    //            {
    //                clickableScript.ToggleActive();
    //                Instantiate(toggleActivateParticle, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    //            }
    //        }
    //    }
    //}

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero, 0, layerMask);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null) Debug.Log(hit.collider.name);
                if (hit.collider != null)
                {
                    var clickableScript = hit.collider.gameObject.GetComponent<ClickableGameObject>();
                    if (clickableScript != null)
                    {
                        clickableScript.ToggleActive();
                        Instantiate(toggleActivateParticle, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                    }
                }
            }


            //RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0, layerMask);

            
        }
    }
}
