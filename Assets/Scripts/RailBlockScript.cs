using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using PathCreation;
public class RailBlockScript : MonoBehaviour
{
    public PathCreator pathCreator;

    readonly float speed = 50.0f;
    float distance = 0.0f;

    void Start()
    {
        transform.position = pathCreator.path.GetPoint(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OnActive();
    }

    void OnActive()
    {
        float pathDist = pathCreator.path.GetClosestDistanceAlongPath(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float pathLength = pathCreator.path.length;

        if (pathDist > distance)
        {
            if (pathDist - distance < pathLength * 0.5f)
                distance = Mathf.Clamp(distance + speed * Time.fixedDeltaTime, distance, pathDist);
            else
                distance = Mathf.Clamp(distance - speed * Time.fixedDeltaTime, pathDist - pathLength, distance);
        }
        else if (pathDist < distance)
        {
            if (distance - pathDist < pathLength * 0.5f)
                distance = Mathf.Clamp(distance - speed * Time.fixedDeltaTime, pathDist, distance);
            else
                distance = Mathf.Clamp(distance + speed * Time.fixedDeltaTime, distance, pathDist + pathLength);
        }

        while (distance < 0.0f)
        {
            distance += pathLength;
        }

        while (distance >= pathLength)
        {
            distance -= pathLength;
        }

        transform.position = pathCreator.path.GetPointAtDistance(distance);
    }
}
