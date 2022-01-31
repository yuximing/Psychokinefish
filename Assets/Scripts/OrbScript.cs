using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    public float speed = 5;

    private Rigidbody2D rb;

    private bool toRight = true;

    public bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-4.10f, 2, 0);
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
        if (transform.position.x <= -4.10f)
        {
            toRight = true;
        }
    }
}