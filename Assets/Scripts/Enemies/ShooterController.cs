using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Transform firePoint;

    private float speed = 15.0f;

    // For changing testing values easily in the inspector
    public float timeBetweenSeries = 2f;
    public float timeBetweenBullets = 0.2f;
    public int bulletsInSeries = 5;

    // Start is called before the first frame update
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
            GameObject projectileObj = Instantiate(projectile, transform.position, transform.rotation);
            projectileObj.GetComponent<Rigidbody2D>().velocity = -transform.right * speed;
            yield return new WaitForSeconds(timeBetweenBullets);
            i++;
        }
    }
}
