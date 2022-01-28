using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathDrawer : MonoBehaviour
{
    PathCreator pathCreator;
    // Start is called before the first frame update
    void Start()
    {
        pathCreator = GetComponent<PathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawLines();
    }

    void DrawLines()
    {
        int numPoints = pathCreator.path.NumPoints;
        if (numPoints < 2) return;

        for (int i = 0; i < numPoints - 1; ++i)
        {
            Vector2 fromPoint = pathCreator.path.GetPoint(i);
            Vector2 toPoint = pathCreator.path.GetPoint(i + 1);

            Debug.DrawLine(fromPoint, toPoint, Color.green);
        }
    }
}
