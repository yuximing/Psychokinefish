using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 0, layerMask);

            if(hit.collider != null)
            {
                var clickableScript = hit.collider.gameObject.GetComponent<ClickableGameObject>();
                if (clickableScript != null)
                {
                    clickableScript.ToggleActive();
                }
            }
        }
    }
}
