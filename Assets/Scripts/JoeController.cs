using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoeController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        position = new Vector2(Mathf.Cos(Time.realtimeSinceStartup), Mathf.Sin(Time.realtimeSinceStartup));
        position *= 3.0f;
        transform.position = position;
    }
}
