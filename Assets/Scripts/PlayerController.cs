using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject railPrefab;

    public float speed;
    private int currentNodeIndex = 3;  // If player is between node n and n + 1, then currentNode = n
    void Start()
    {
        RailScript rail = railPrefab.GetComponent<RailScript>();
        transform.position = rail.Nodes[currentNodeIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            MoveLeft(speed);
        } else if (Input.GetKey(KeyCode.D))
        {
            MoveRight(speed);
        }
    }

    /*
     * Move player to the left along the rail
     */
    void MoveLeft(float speed)
    {
        RailScript rail = railPrefab.GetComponent<RailScript>();
        Vector2 currentNodePosition = rail.Nodes[currentNodeIndex].transform.position;

        // Player is exactly on the node
        if(currentNodePosition == (Vector2) transform.position)
        {
            // Return if player is at the left most node
            if (currentNodeIndex == 0) return;

            currentNodeIndex--;
            Vector2 newCurNodePos = rail.Nodes[currentNodeIndex].transform.position;
            Vector2 nextNodePos = rail.Nodes[currentNodeIndex + 1].transform.position;

            // Vector from nextNode to curNode
            Vector2 railLineProj = newCurNodePos - nextNodePos;

            // Get t val for Lerp
            float t = railLineProj.normalized.sqrMagnitude * speed * Time.deltaTime / railLineProj.sqrMagnitude;

            transform.position = Vector2.Lerp(nextNodePos, newCurNodePos, t);
        } 
        else // Player is between two nodes
        {
            Vector2 nextNodePos = rail.Nodes[currentNodeIndex + 1].transform.position;

            // Vector from nextNode to curNode;
            Vector2 railLineProj = currentNodePosition - nextNodePos;
            // Vector from nextNode to playerPos after moving
            Vector2 playerProj = ((Vector2)transform.position + railLineProj.normalized * speed * Time.deltaTime) - nextNodePos;

            // Vector projection to get the t value
            float t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;
            transform.position = Vector2.Lerp(nextNodePos, currentNodePosition, t);
        }
    }

    /*
     * Move player to the right along the rail
     */
    void MoveRight(float speed)
    {
        RailScript rail = railPrefab.GetComponent<RailScript>();
        if (currentNodeIndex == rail.Nodes.Count - 1) return;

        Vector2 nextNodePosition = rail.Nodes[currentNodeIndex + 1].transform.position;

        // Player is exactly on the node
        if (nextNodePosition == (Vector2)transform.position)
        {
            Vector2 newCurNodePos = rail.Nodes[currentNodeIndex].transform.position;
            Vector2 nextNodePos = rail.Nodes[currentNodeIndex + 1].transform.position;

            // Vector from curNode to nextNode;
            Vector2 railLineProj = nextNodePos - newCurNodePos;

            // Get t value for Lerp
            float t = railLineProj.normalized.sqrMagnitude * speed * Time.deltaTime / railLineProj.sqrMagnitude;
            transform.position = Vector2.Lerp(newCurNodePos, nextNodePos, t);
        }
        else // Player is between two nodes
        {
            Vector2 currentNodePosition = rail.Nodes[currentNodeIndex].transform.position;
            

            Vector2 railLineProj = nextNodePosition - currentNodePosition;
            Vector2 playerProj = ((Vector2)transform.position + railLineProj.normalized * speed * Time.deltaTime) - currentNodePosition;

            // Vector projection to get t value for Lerp
            float t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;
            transform.position = Vector2.Lerp(currentNodePosition, nextNodePosition, t);

            // If player reached the next node, increment node index
            if (t >= 1.0f) ++currentNodeIndex;
        }
    }
}
