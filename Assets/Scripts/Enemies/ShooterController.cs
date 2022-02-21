using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour, IDamageable
{
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform firePoint;

    private float bulletSpeed = 15.0f;
    private float moveSpeed = 1.5f;

    Timer betweenSeriesTimer;
    private float timeBetweenBullets = 0.1f;
    private int bulletsInSeries = 5;

    bool isActive = false;
    CameraScroll cameraScript;

    void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraScroll>();
        betweenSeriesTimer = new Timer(1.5f, 0.5f);
    }
    private void Update()
    {
        

        if (!CameraScroll.IsSpriteOffScreen(gameObject, 2.0f))
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }

        if (!isActive) return; // out of screen

        betweenSeriesTimer.Tick();

        if (betweenSeriesTimer.ResetTimer())
        {
            Shoot();
        }

        Move();
    }

    void Shoot()
    {
        StartCoroutine(ShootCorutine());
    }

    IEnumerator ShootCorutine()
    {
        betweenSeriesTimer.Paused = true;
        int i = 0;
        while (i < bulletsInSeries)
        {
            GameObject projectileObj = Instantiate(projectile, firePoint.position, firePoint.rotation);
            projectileObj.GetComponent<Rigidbody2D>().velocity = -transform.right * bulletSpeed;
            yield return new WaitForSeconds(timeBetweenBullets);
            i++;
        }
        betweenSeriesTimer.Paused = false;
    }

    void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime * -Vector2.right);
    }

    public void InflictDamage(int dmg)
    {
        if (!isActive) return;
        //hp -= dmg;
        //damagedTimer.ResetTimer();
        //if (hp <= 0)
        //{
        //    Instantiate(destroyParticle, transform.position, Quaternion.identity);
        //    Destroy(gameObject);
        //}
    }
}
