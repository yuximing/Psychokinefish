using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public PathCreator pathCreator;

    public float speed;
    private int currentNodeIndex = 3;  // If player is between node n and n + 1, then currentNode = n
    Rigidbody2D rbody;
    int moveDirection = 0;             // normalized value where -1 = left, 1 = right

    void Start()
    {
        transform.position = pathCreator.path.GetPoint(currentNodeIndex);
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            moveDirection = -1;
        else if (Input.GetKey(KeyCode.D))
            moveDirection = 1;
        else
            moveDirection = 0;
    }

    private void FixedUpdate()
    {
        if (moveDirection == -1) MoveLeft(speed);
        else if (moveDirection == 1) MoveRight(speed);
    }

    /*
     * Move player to the left along the rail
     */
    void MoveLeft(float speed)
    {
        //Vector2 currentNodePosition = rail.Nodes[currentNodeIndex].transform.position;
        Vector2 currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);

        // Player is exactly on the node
        if (currentNodePosition == rbody.position)
        {
            // Return if player is at the left most node
            if (currentNodeIndex == 0) return;

            currentNodeIndex--;

            Vector2 newCurNodePos = pathCreator.path.GetPoint(currentNodeIndex);
            Vector2 nextNodePos = pathCreator.path.GetPoint(currentNodeIndex + 1);

            // Vector from nextNode to curNode
            Vector2 railLineProj = newCurNodePos - nextNodePos;

            // Get t val for Lerp
            float t = railLineProj.normalized.magnitude * speed * Time.fixedDeltaTime / railLineProj.magnitude;

            rbody.MovePosition(Vector2.Lerp(nextNodePos, newCurNodePos, t));
        } 
        else // Player is between two nodes
        {
            Vector2 nextNodePos = pathCreator.path.GetPoint(currentNodeIndex + 1);

            // Vector from nextNode to curNode;
            Vector2 railLineProj = currentNodePosition - nextNodePos;
            // Vector from nextNode to playerPos after moving
            Vector2 playerProj = (rbody.position + railLineProj.normalized * speed * Time.fixedDeltaTime) - nextNodePos;

            // Vector projection to get the t value
            float t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;

            rbody.MovePosition(Vector2.Lerp(nextNodePos, currentNodePosition, t));
        }
    }

    /*
     * Move player to the right along the rail
     */
    void MoveRight(float speed)
    {
        if (currentNodeIndex == pathCreator.path.NumPoints - 1) return;

        Vector2 nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

        // Player is on the node
        if (nextNodePosition == rbody.position)
        {
            Vector2 newCurNodePos = pathCreator.path.GetPoint(currentNodeIndex);
            Vector2 nextNodePos = pathCreator.path.GetPoint(currentNodeIndex + 1);

            // Vector from curNode to nextNode;
            Vector2 railLineProj = nextNodePos - newCurNodePos;

            // Get t value for Lerp
            float t = railLineProj.normalized.magnitude * speed * Time.fixedDeltaTime / railLineProj.magnitude;

            rbody.MovePosition(Vector2.Lerp(newCurNodePos, nextNodePos, t));
        }
        else // Player is between two nodes
        {
            Vector2 currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
            

            Vector2 railLineProj = nextNodePosition - currentNodePosition;
            Vector2 playerProj = (rbody.position + railLineProj.normalized * speed * Time.fixedDeltaTime) - currentNodePosition;

            // Vector projection to get t value for Lerp
            float t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;

            Vector2 targetPos = Vector2.Lerp(currentNodePosition, nextNodePosition, t);
            rbody.MovePosition(targetPos);

            // If player reached the next node, increment node index
            if (nextNodePosition == targetPos) ++currentNodeIndex;
        }
    }
}
