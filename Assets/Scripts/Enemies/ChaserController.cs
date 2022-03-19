using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserController : MonoBehaviour, IDamageable
{
    GameObject targetObject;
    public GameObject destroyParticle;

    Rigidbody2D rbody;
    private int hp = 5;
    readonly float maxSpeed = 4.0f;
    readonly float seekForce = 8.0f;
    CameraScroll cameraScript;

    Timer damagedTimer;
    SpriteRenderer spriteRenderer;

    AudioManager audioManager;

    public AudioClip enemyDieSfx;

    public bool spawnRight = true;
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        targetObject = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        damagedTimer = new Timer(0.1f);
        cameraScript = Camera.main.GetComponent<CameraScroll>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }
    private void Update()
    {
        if (!isActive)
        {
            if (!CameraScroll.IsSpriteOffScreen(gameObject, 2.0f)) Spawn();
            else return;
        }
        damagedTimer.Tick();
        if (damagedTimer.IsReady())
        {
            spriteRenderer.material.SetColor("_Color", Color.black);
        } else
        {
            spriteRenderer.material.SetColor("_Color", Color.Lerp(Color.blue,Color.grey,Random.value));
        }
    }

    private void Spawn()
    {
        isActive = true;
        if(!spawnRight)
        {
            Rect screenRect = CameraScroll.GetScreenRect();
            var sprite = GetComponent<Renderer>();
            Vector2 center = sprite.bounds.center;
            Vector2 extends = sprite.bounds.extents;

            Vector3 oldPos = transform.position;
            oldPos.x = screenRect.x - extends.x * 2.0f;
            transform.position = oldPos;

            isActive = true;
        }
    }
    private void FixedUpdate()
    {
        if (!isActive) return;
        Vector2 target = targetObject.transform.position;
        Seek(target);
    }
    void Seek(Vector2 target)
    {
        Vector2 position = transform.position;
        Vector2 forceDir = target - position;
        forceDir.Normalize();
        forceDir *= seekForce * Time.fixedDeltaTime;
        if (rbody.bodyType != RigidbodyType2D.Static) rbody.velocity += forceDir;

        if(rbody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rbody.velocity = rbody.velocity.normalized * maxSpeed;
        }
        if (rbody.bodyType != RigidbodyType2D.Static)
        {
            rbody.MoveRotation(Vector2.SignedAngle(Vector2.right, rbody.velocity));
        }
    }

    public void InflictDamage(int dmg)
    {
        if (!isActive) return;
        hp -= dmg;
        damagedTimer.ResetTimer();
        if (hp <= 0)
        {
            Instantiate(destroyParticle, transform.position, Quaternion.identity);
            audioManager.PlayOneShot(enemyDieSfx);
            Destroy(gameObject);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        var playerScript = collision.gameObject.GetComponent<PlayerController>();
        playerScript?.ChangeHealth(-1); // Basically says: if playerScript not null, change health
    }
}
