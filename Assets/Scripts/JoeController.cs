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
        transform.eulerAngles = new Vector3(0.0f, 0.0f, transform.eulerAngles.z + 90.0f * Time.deltaTime);
        transform.Rotate(Vector3.forward);
    }
}
