using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

//We're gonna need another script to add damage etc, this should work for when the bar has to
//be moved, but it won't do anything until damage is added
//ex: one hit would set the bar size to 0.5f, which is half the health bar

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
    public void Update()
    {
        
    }
}
