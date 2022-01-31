using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    float speed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        var cameraPos = Camera.main.gameObject.transform.position;
        cameraPos.x += speed * Time.deltaTime;
        Camera.main.gameObject.transform.position = cameraPos;
    }
}
