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
        explosionTimer = new Timer(4.0f, false);
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x/2.0f, layerMask);

        foreach (var hitCollider in hitColliders)
        {
            var rb = hitCollider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                hitCollider.gameObject.GetComponent<ChaserController>()?.Freeze();

                if (hitCollider.tag == "Shooter")
                {
                    hitCollider.gameObject.GetComponent<SpreadShooterController>()?.Freeze();
                    hitCollider.gameObject.GetComponent<ShooterController>()?.Freeze();
                }
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
                //rb.bodyType = RigidbodyType2D.Dynamic;
                rb.constraints = RigidbodyConstraints2D.None;
                hitCollider.gameObject.GetComponent<ChaserController>()?.UnFreeze();

                if (hitCollider.tag == "EnemyProjectile")
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    Destroy(hitCollider.gameObject);
                }
                if (hitCollider.tag == "Shooter")
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                    hitCollider.gameObject.GetComponent<SpreadShooterController>()?.UnFreeze();
                    hitCollider.gameObject.GetComponent<ShooterController>()?.UnFreeze();
                }
            }
        }
    }


}
