using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosionScript : MonoBehaviour
{
    Timer explosionTimer;

    [SerializeField]
    LayerMask layerMask;

    private void Start()
    {
        explosionTimer = new Timer(0.2f, false);
    }

    void Update()
    {
        explosionTimer.Tick();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2.0f, layerMask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "EnemyProjectile")
            {
                Destroy(hitCollider.gameObject);
            }
        }
        if (explosionTimer.IsReady())
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageableScript = collision.gameObject.GetComponent<IDamageable>();
        damageableScript?.InflictDamage(10);
    }
}
