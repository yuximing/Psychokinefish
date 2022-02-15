using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyPlankScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerScript = collision.gameObject.GetComponentInParent<PlayerController>();
        playerScript?.Die();
    }
}
