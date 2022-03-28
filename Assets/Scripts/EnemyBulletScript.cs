using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    // if collide with blocks and turret and any other stuff, detroy
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (CameraScroll.IsSpriteOffScreen(gameObject)) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerScript = collision.gameObject.GetComponent<PlayerController>();
        playerScript?.ChangeHealth(-1);
        collision.gameObject.GetComponent<BlockadeScript>()?.InflictDamage(1);
        Destroy(gameObject);
    }
}
