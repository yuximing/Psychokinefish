using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceExplosionScript : MonoBehaviour
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
        var rbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rbody != null)
        {
            rbody.AddForce(50.0f / Time.fixedDeltaTime * (rbody.position - (Vector2)transform.position).normalized);
        }
        var enemyScript = collision.gameObject.GetComponent<ChaserController>();
        if(enemyScript != null)
        {
            enemyScript.InflictDamage(10);
        }
        var blockadeScript = collision.gameObject.GetComponent<BlockadeScript>();
        blockadeScript?.InflictDamage(10);
    }
}
