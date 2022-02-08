using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    private Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        Transform bar = transform.Find("Bar");
    }

    //Set fill size of bar, 1f is full, 0f is empty
    public void SetSize(float sizeNormal)
    {
        bar.localScale = new Vector3(sizeNormal, 1f);
    }
}
