using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour, IDamageable
{
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform firePoint;
    Vector3 firePointOffset;

    private float bulletSpeed = 15.0f;
    private float moveSpeed = 1.5f;

    public GameObject destroyParticle;

    Timer betweenSeriesTimer;
    private float timeBetweenBullets = 0.1f;
    private int bulletsInSeries = 5;

    public bool spawnRight = true;
    bool isActive = false;
    CameraScroll cameraScript;

    private int hp = 7; //was 10 if need to switch back
    Timer damagedTimer;
    SpriteRenderer spriteRenderer;

    public AudioClip enemyDieSfx;
    AudioManager audioManager;

    private Animator anim;

    Rigidbody2D rb;

    public bool IsFrozen { get; set; }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cameraScript = Camera.main.GetComponent<CameraScroll>();
        betweenSeriesTimer = new Timer(1.5f, 0.5f);
        damagedTimer = new Timer(0.1f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();

        Vector3 tempPos = firePoint.localPosition;
        if(!spawnRight) tempPos.x = -tempPos.x;
        firePointOffset = tempPos;
    }
    private void Update()
    {
        if (!spawnRight) anim.SetBool("to_right", true);
        else anim.SetBool("to_right", false);
        if (!isActive)
        {
            if (!CameraScroll.IsSpriteOffScreen(gameObject, 2.0f))
            {
                Spawn();
                
            } else
            {
                return; // off screen
            }
        }

        if (CameraScroll.IsSpriteOffScreen(gameObject, 2.0f)) isActive = false;

        if (!IsFrozen) betweenSeriesTimer.Tick();
        damagedTimer.Tick();

        //if (betweenSeriesTimer.ResetTimer())
        //{
        //    Shoot();
        //}

        if (damagedTimer.IsReady())
        {
            spriteRenderer.material.SetColor("_Color", Color.black);
        }
        else
        {
            spriteRenderer.material.SetColor("_Color", Color.Lerp(Color.grey, Color.white, Random.value));
        }

        //Move();
    }
    private void FixedUpdate()
    {
        if (isActive) Move();
        

        if (betweenSeriesTimer.ResetTimer())
        {
            Shoot();
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

    void Shoot()
    {
        StartCoroutine(ShootCorutine());
    }

    void Spawn()
    {
        isActive = true;
        if (!spawnRight)
        {
            Rect screenRect = CameraScroll.GetScreenRect();
            var sprite = GetComponent<Renderer>();
            Vector2 center = sprite.bounds.center;
            Vector2 extends = sprite.bounds.extents;

            Vector3 oldPos = transform.position;
            oldPos.x = screenRect.x - extends.x * 1.0f;
            transform.position = oldPos;
        }
    }

    IEnumerator ShootCorutine()
    {
        betweenSeriesTimer.Paused = true;
        int i = 0;
        while (i < bulletsInSeries)
        {
            GameObject projectileObj = Instantiate(projectile, transform.position + firePointOffset, firePoint.rotation);
            projectileObj.GetComponent<Rigidbody2D>().velocity = (spawnRight ? -1 : 1) * transform.right * bulletSpeed;
            do
            {
                yield return new WaitForSeconds(timeBetweenBullets);
            } while (IsFrozen); // bruh, actually used a do-while for once
            i++;
        }
        betweenSeriesTimer.Paused = false;
    }

    void Move()
    {
        //transform.Translate(moveSpeed * Time.deltaTime * (spawnRight ? -1 : 1) * Vector2.right);

        rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * (spawnRight ? -1 : 1) * Vector2.right);
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
}
