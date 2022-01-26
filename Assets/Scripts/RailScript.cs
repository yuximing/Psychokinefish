using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailScript : MonoBehaviour
{
    // Start is called before the first frame update
    public List<NodeScript> Nodes { get; protected set; }
    public float s = 3;
    void Start()
    {
        Nodes = new List<NodeScript>(GetComponentsInChildren<NodeScript>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
