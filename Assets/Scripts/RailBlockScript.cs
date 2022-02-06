using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using PathCreation;
public class RailBlockScript : ClickableGameObject
{
    public PathCreator pathCreator;

    readonly float speed = 50.0f;
    float distance = 0.0f;

    void Start()
    {
        transform.position = pathCreator.path.GetClosestPointOnPath(transform.position);
    }

    public override void ToggleActive()
    {
        base.ToggleActive();

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (isActive) spriteRenderer.color = Color.green;
        else spriteRenderer.color = Color.white;
    }
    protected override void OnActive()
    {
        if (pathCreator.path.isClosedLoop) ClosedLoopUpdate();
        else OpenLoopUpdate();
    }

    

    protected override void OnInactive()
    {
    }

    void OpenLoopUpdate()
    {
        float pathDist = pathCreator.path.GetClosestDistanceAlongPath(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float pathLength = pathCreator.path.length;

        if (pathDist > distance)
        {
                distance = Mathf.Clamp(distance + speed * Time.deltaTime, Mathf.Max(distance, 0.0f), pathDist);
        }
        else if (pathDist < distance)
        {
                distance = Mathf.Clamp(distance - speed * Time.deltaTime, Mathf.Min(pathDist, pathLength), distance);
        }

        transform.position = pathCreator.path.GetPointAtDistance(distance, endOfPathInstruction: EndOfPathInstruction.Stop);
    }
    void ClosedLoopUpdate()
    {
        float pathDist = pathCreator.path.GetClosestDistanceAlongPath(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float pathLength = pathCreator.path.length;

        if (pathDist > distance)
        {
            if (pathDist - distance < pathLength * 0.5f)
                distance = Mathf.Clamp(distance + speed * Time.deltaTime, distance, pathDist);
            else
                distance = Mathf.Clamp(distance - speed * Time.deltaTime, pathDist - pathLength, distance);
        }
        else if (pathDist < distance)
        {
            if (distance - pathDist < pathLength * 0.5f)
                distance = Mathf.Clamp(distance - speed * Time.deltaTime, pathDist, distance);
            else
                distance = Mathf.Clamp(distance + speed * Time.deltaTime, distance, pathDist + pathLength);
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
