using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float step = 0.002f;
        var cameraPos = Camera.main.gameObject.transform.position;
        cameraPos.x += step;
        Camera.main.gameObject.transform.position = cameraPos;
    }
}
