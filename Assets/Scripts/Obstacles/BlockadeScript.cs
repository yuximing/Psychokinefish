using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockadeScript : MonoBehaviour, IDamageable
{
    int hp = 8;
    Timer damagedTimer;
    SpriteRenderer spriteRenderer;

    public GameObject destroyParticle;

    public AudioClip boxBreakSfx;
    AudioManager audioManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        damagedTimer = new Timer(0.1f);
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();


    }

    private void Update()
    {
        damagedTimer.Tick();

        if (damagedTimer.IsReady())
        {
            spriteRenderer.material.SetColor("_Color", Color.black);
        }
        else
        {
            spriteRenderer.material.SetColor("_Color", Color.Lerp(Color.black, Color.white, damagedTimer.TimeRatio));
        }
    }
    public void InflictDamage(int dmg)
    {
        hp -= dmg;
        damagedTimer.ResetTimer();
        if (hp <= 0)
        {
            Instantiate(destroyParticle, transform.position, Quaternion.identity);
            audioManager.PlayOneShot(boxBreakSfx);
            Destroy(gameObject);
        }
    }
}
