using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    // start shooting only when in camera
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform firePoint;

    private float bulletSpeed = 15.0f;

    private float timeBetweenSeries = 1.5f;
    private float timeBetweenBullets = 0.1f;
    private int bulletsInSeries = 5;

    void Start()
    {
        InvokeRepeating("Shoot", 0f, timeBetweenSeries);
    }


    void Shoot()
    {
        StartCoroutine(ShootCorutine());
    }

    IEnumerator ShootCorutine()
    {
        //var dir = new Vector2(transform.position.x - 10, transform.position.y) - (Vector2)transform.position;
        int i = 0;
        while (i < bulletsInSeries)
        {
            GameObject projectileObj = Instantiate(projectile, firePoint.position, firePoint.rotation);
            projectileObj.GetComponent<Rigidbody2D>().velocity = -transform.right * bulletSpeed;
            yield return new WaitForSeconds(timeBetweenBullets);
            i++;
        }
    }
}
