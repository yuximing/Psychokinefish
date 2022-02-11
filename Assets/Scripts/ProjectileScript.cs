using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private void Update()
    {
        if (Camera.main.GetComponent<CameraScroll>().IsSpriteOffScreen(gameObject))
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemyScript = collision.gameObject.GetComponent<ChaserController>();
        Destroy(gameObject);

        if (enemyScript != null)
        {
            enemyScript.InflictDamage(1);
        }
    }
}
