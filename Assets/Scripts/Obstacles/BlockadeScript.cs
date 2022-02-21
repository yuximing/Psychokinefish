using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockadeScript : MonoBehaviour, IDamageable
{
    int hp = 10;
    const int BREAK_FRAMES = 4;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        const float d = 10 / (0.0f + BREAK_FRAMES);
        animator.SetInteger("Ratio", (int) (hp / d));
    }
    public void InflictDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
