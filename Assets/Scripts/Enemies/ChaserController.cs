using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserController : MonoBehaviour
{
    GameObject targetObject;

    Rigidbody2D rbody;
    private int hp = 5;
    readonly float maxSpeed = 5.0f;
    readonly float seekForce = 10.0f;

    Timer damagedTimer;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        targetObject = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        damagedTimer = new Timer(0.1f);
    }
    private void Update()
    {
        damagedTimer.Tick();
        if (damagedTimer.IsReady())
        {
            spriteRenderer.material.SetColor("_Color", Color.black);
        } else
        {
            spriteRenderer.material.SetColor("_Color", Color.Lerp(Color.blue,Color.grey,Random.value));
        }
    }
    private void FixedUpdate()
    {
        Vector2 target = targetObject.transform.position;
        Seek(target);
    }
    void Seek(Vector2 target)
    {
        Vector2 position = transform.position;
        Vector2 forceDir = target - position;
        forceDir.Normalize();
        forceDir *= seekForce;
        rbody.AddForce(forceDir);
        if(rbody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rbody.velocity = rbody.velocity.normalized * (maxSpeed + (rbody.velocity.magnitude - maxSpeed) * 0.015f / Time.fixedDeltaTime) ;
        }
        if (rbody.bodyType != RigidbodyType2D.Static)
        {
            rbody.MoveRotation(Vector2.SignedAngle(Vector2.right, rbody.velocity));
        }
    }

    public void InflictDamage(int dmg)
    {
        hp -= dmg;
        damagedTimer.ResetTimer();
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var playerScript = collision.gameObject.GetComponent<PlayerController>();
        playerScript?.ChangeHealth(-1);
    }
}
