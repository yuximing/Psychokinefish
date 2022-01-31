using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public float speed = 5;

    private Rigidbody2D rb;

    private bool toRight = true;

    public bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn();
        //rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(-4, 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) Move();


    }

    void Move()
    {
        if (toRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector2.right * speed * Time.deltaTime);
        }

        if (transform.position.x >= 4.0f)
        {
            toRight = false;
        }
        if (transform.position.x <= -4.0f)
        {
            toRight = true;
        }
    }

    //void Spawn()
    //{
    //    float yPos = Random.Range
    //        (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
    //    float xPos = Random.Range
    //        (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
    //    Vector2 spawnPos = new Vector2(xPos, yPos);
    //    transform.position = spawnPos;
    //}
}