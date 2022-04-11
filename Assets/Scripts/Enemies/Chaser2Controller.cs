using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser2Controller : MonoBehaviour, IDamageable
{
    GameObject targetObject;
    public GameObject destroyParticle;

    Rigidbody2D rbody;
    private int hp = 2;
    float maxSpeed = 10f;
    float speedIncrease = 7f;
    float currentTime;
    float timeIncrease = 2.0f;
    float timeToWait = 4f;
    readonly float seekForce = 7.0f;
    CameraScroll cameraScript;

    Timer damagedTimer;
    SpriteRenderer spriteRenderer;

    AudioManager audioManager;
    Animator anim;

    public AudioClip enemyDieSfx;

    public bool spawnRight = true;
    bool isActive = false;

    public bool IsFrozen { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        targetObject = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        damagedTimer = new Timer(0.1f);
        cameraScript = Camera.main.GetComponent<CameraScroll>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        currentTime = Time.time + timeIncrease;
    }
    private void Update()
    {
        if (Time.time >= currentTime)
        {
            rbody.velocity = Vector2.zero;
            currentTime = Time.time + timeToWait;
            maxSpeed += speedIncrease;
            currentTime = Time.time + timeIncrease;
        }
        if (!isActive)
        {
            if (!CameraScroll.IsSpriteOffScreen(gameObject, 2.0f)) Spawn();
            else return;
        }
        damagedTimer.Tick();
        if (damagedTimer.IsReady())
        {
            spriteRenderer.material.SetColor("_Color", Color.black);
        }
        else
        {
            spriteRenderer.material.SetColor("_Color", Color.Lerp(Color.blue, Color.grey, Random.value));
        }
    }

    private void Spawn()
    {
        isActive = true;
        if (!spawnRight)
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
        spriteRenderer.flipY = Vector2.SignedAngle(Vector2.up, rbody.velocity) > 0.0f;
    }
    void Seek(Vector2 target)
    {
        Vector2 position = transform.position;
        Vector2 forceDir = target - position;
        forceDir.Normalize();
        forceDir *= seekForce * Time.fixedDeltaTime;
        if (rbody.bodyType != RigidbodyType2D.Static) rbody.velocity += forceDir;

        if (rbody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rbody.velocity = rbody.velocity.normalized * maxSpeed;
        }
        if (rbody.bodyType != RigidbodyType2D.Static)
        {
            if (rbody.velocity != Vector2.zero)
            {
                rbody.MoveRotation(Vector2.SignedAngle(Vector2.right, rbody.velocity));
            }
        } 
    }

    public void Freeze()
    {
        IsFrozen = true;
        anim.enabled = false;
    }

    public void UnFreeze()
    {
        IsFrozen = false;
        anim.enabled = true;
    }

    public void InflictDamage(int dmg)
    {
        if (!isActive) return;
        hp -= (IsFrozen ? 3 : 1) * dmg;
        damagedTimer.ResetTimer();
        if (hp <= 0)
        {
            Instantiate(destroyParticle, transform.position, Quaternion.identity);
            audioManager.PlayOneShot(enemyDieSfx);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerScript = collision.gameObject.GetComponent<PlayerController>();
        if (!IsFrozen) playerScript?.ChangeHealth(-1); // Basically says: if playerScript not null, change health
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var playerScript = collision.gameObject.GetComponent<PlayerController>();
        if (!IsFrozen) playerScript?.ChangeHealth(-1); // Basically says: if playerScript not null, change health
    }
}
