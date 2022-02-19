using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform firePoint;

    private float bulletSpeed = 15.0f;
    private float moveSpeed = 1.5f;

    private float timeBetweenSeries = 1.5f;
    private float timeBetweenBullets = 0.1f;
    private int bulletsInSeries = 5;
    private float timestamp;

    bool isActive = false;
    CameraScroll cameraScript;

    void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraScroll>();
        timestamp = Time.time + timeBetweenSeries;
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
        
        if (timestamp <= Time.time)
        {
            Shoot();
            timestamp += timeBetweenSeries;
        }
        Move();
    }

    void Shoot()
    {
        StartCoroutine(ShootCorutine());
        
    }

    IEnumerator ShootCorutine()
    {
        int i = 0;
        while (i < bulletsInSeries)
        {
            GameObject projectileObj = Instantiate(projectile, firePoint.position, firePoint.rotation);
            projectileObj.GetComponent<Rigidbody2D>().velocity = -transform.right * bulletSpeed;
            yield return new WaitForSeconds(timeBetweenBullets);
            i++;
        }
    }

    void Move()
    {
        transform.Translate(moveSpeed * Time.deltaTime * -Vector2.right);
    }
}
