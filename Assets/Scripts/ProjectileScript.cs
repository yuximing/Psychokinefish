using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private void Update()
    {
        if (CameraScroll.IsSpriteOffScreen(gameObject))
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var damageableScript = collision.gameObject.GetComponent<IDamageable>();
        damageableScript?.InflictDamage(1);
        //var enemyScript = collision.gameObject.GetComponent<ChaserController>();
        //enemyScript?.InflictDamage(1);

        //var blockadeScript = collision.gameObject.GetComponent<BlockadeScript>();
        //blockadeScript?.InflictDamage(1);

        Destroy(gameObject);
    }
}
