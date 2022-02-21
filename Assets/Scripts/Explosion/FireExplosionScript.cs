using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosionScript : MonoBehaviour
{
    Timer explosionTimer;

    private void Start()
    {
        explosionTimer = new Timer(0.2f, false);
    }

    void Update()
    {
        explosionTimer.Tick();
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
