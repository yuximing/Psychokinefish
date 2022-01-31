using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    float speed = 1.5f;
    Vector2 direction = Vector2.right;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        var cameraPos = Camera.main.gameObject.transform.position;
        cameraPos += speed * Time.deltaTime * (Vector3) direction;
        Camera.main.gameObject.transform.position = cameraPos;
    }
}
