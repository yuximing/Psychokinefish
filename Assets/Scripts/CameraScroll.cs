using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    float speed = 1.5f;
    Vector2 direction = Vector2.right;
    // Start is called before the first frame update
    void Start()
    {
        CreateCollisionBorders();
    }

    // Update is called once per frame
    void Update()
    {
        
        var cameraPos = Camera.main.gameObject.transform.position;
        cameraPos += speed * Time.deltaTime * (Vector3) direction;
        Camera.main.gameObject.transform.position = cameraPos;
    }

    void CreateCollisionBorders()
    {
        EdgeCollider2D edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();
        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        Vector2 topLeft = new Vector2(bottomLeft.x, topRight.y);
        Vector2 bottomRight = new Vector2(topRight.x, bottomLeft.y);

        Vector2[] edgePoints = new Vector2[5];

        edgePoints[0] = bottomLeft;
        edgePoints[1] = topLeft;
        edgePoints[2] = topRight;
        edgePoints[3] = bottomRight;
        edgePoints[4] = bottomLeft;

        edge.points = edgePoints;
    }
}
