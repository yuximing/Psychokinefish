using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockadeScript : MonoBehaviour
{
    int hp = 10;

    public void InflictDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
