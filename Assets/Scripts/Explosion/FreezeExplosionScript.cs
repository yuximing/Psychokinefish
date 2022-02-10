using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeExplosionScript : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;

    Timer explosionTimer;
    // Start is called before the first frame update
    void Start()
    {
        explosionTimer = new Timer(1.5f, false);
    }

    // Update is called once per frame
    void Update()
    {
        explosionTimer.Tick();
        Collider2D[] frozen = Freeze();
        if (explosionTimer.IsReady())
        {
            UnFreeze(frozen);
            Destroy(gameObject);
        }
    }

    Collider2D[] Freeze()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            var rb = hitCollider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
        return hitColliders;

    }

    void UnFreeze(Collider2D[] hitColliders)
    {
        foreach (var hitCollider in hitColliders)
        {
            var rb = hitCollider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }


}
