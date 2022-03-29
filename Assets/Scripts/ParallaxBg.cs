using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBg : MonoBehaviour
{
    private float length; // sprite
    private Vector3 startPos;
    public GameObject cam;
    
    [SerializeField]
    private Vector2 parallaxSpeed;

    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        //cam.transform.position.x* parallaxSpeed;
        Vector2 deltaMovement = new Vector2(cam.transform.position.x * parallaxSpeed.x, cam.transform.position.y * parallaxSpeed.y);
        transform.position = new Vector3(startPos.x + deltaMovement.x, startPos.y + deltaMovement.y, transform.position.z);
    }
}
