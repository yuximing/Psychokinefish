using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathCreation;
public class CameraScroll : MonoBehaviour
{
    [SerializeField]
    static float speed = 1.0f;
    private static bool isMoving = true;

    static public float Speed { get { return speed; } set { speed = value > 0.0f ? value : 0.0f; } }

    readonly BoxCollider2D[] borders = new BoxCollider2D[4];
    public PathCreator cameraRail;

    float railDistance = 0.0f;

    void Awake()
    {
        BoxCollider2D up = gameObject.AddComponent<BoxCollider2D>();
        BoxCollider2D right = gameObject.AddComponent<BoxCollider2D>();
        BoxCollider2D down = gameObject.AddComponent<BoxCollider2D>();
        BoxCollider2D left = gameObject.AddComponent<BoxCollider2D>();

        borders[0] = up;
        borders[1] = right;
        borders[2] = down;
        borders[3] = left;

        UpdateCollisionBorders();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving) railDistance += speed * Time.deltaTime;

        var cameraPos = Camera.main.gameObject.transform.position;
        //cameraPos += speed * Time.deltaTime * (Vector3) direction;
        Vector2 railPosition = cameraRail.path.GetPointAtDistance(railDistance, endOfPathInstruction: EndOfPathInstruction.Stop);
        cameraPos = new Vector3(railPosition.x, railPosition.y, cameraPos.z);
        Camera.main.gameObject.transform.position = cameraPos;
        // Camera.main.orthographicSize -= Time.deltaTime * 0.1f;
        UpdateCollisionBorders();
        //CheckEndOfLevel();
    }

    void CheckEndOfLevel()
    {
        // Get second last vertex
        float endlength = cameraRail.path.cumulativeLengthAtEachVertex[cameraRail.path.NumPoints - 1];
        if (railDistance >= endlength)
        {
            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            int totalIndex = SceneManager.sceneCountInBuildSettings;

            if (++levelIndex >= totalIndex)
            {
                levelIndex = 0;
            }
            SceneManager.LoadScene(levelIndex);
        }
    }

    void UpdateCollisionBorders()
    {
        Vector2 cameraPos = Camera.main.transform.position;

        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z)) - (Vector3) cameraPos;
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)) - (Vector3)cameraPos;
        Vector2 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, Camera.main.transform.position.z)) - (Vector3)cameraPos;
        Vector2 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.transform.position.z)) - (Vector3)cameraPos;

        float height = topRight.y - bottomRight.y;
        float width = topRight.x - topLeft.x;

        borders[0].offset = new Vector2(0, topRight.y + 1);
        borders[0].size = new Vector2(width, 2);

        borders[1].offset = new Vector2(topRight.x + 1, 0);
        borders[1].size = new Vector2(2, height);

        borders[2].offset = new Vector2(0, bottomRight.y - 1);
        borders[2].size = new Vector2(width, 2);

        borders[3].offset = new Vector2(bottomLeft.x - 1, 0);
        borders[3].size = new Vector2(2, height);
    }
    public static void ToggleMove()
    {
        isMoving = !isMoving;
    }

    public static bool IsSpriteOffScreen(GameObject obj, float errorFactor = 1.0f)
    {
        var sprite = obj.GetComponent<Renderer>();
        Vector2 center = sprite.bounds.center;
        Vector2 extends = sprite.bounds.extents;

        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector2 pos = center;

        return pos.x < bottomLeft.x - extends.x * errorFactor ||
            pos.x > topRight.x + extends.x * errorFactor ||
            pos.y < bottomLeft.y - extends.y * errorFactor ||
            pos.y > topRight.y + extends.y * errorFactor;
    }
}
