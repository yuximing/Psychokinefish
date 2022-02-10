using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserController : MonoBehaviour
{
    GameObject targetObject;

    Rigidbody2D rbody;
    public int hp = 5;
    float maxSpeed = 5.0f;
    float seekForce = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        targetObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
