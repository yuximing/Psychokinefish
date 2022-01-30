using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public PathCreator pathCreator;

    public float speed;
    private int currentNodeIndex = 1;  // If player is between node n and n + 1, then currentNode = n
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
        if (currentNodeIndex == pathCreator.path.NumPoints - 1) --currentNodeIndex;
        Vector2 currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
        Vector2 nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

        // Vector from nextNode to curNode;
        Vector2 railLineProj = currentNodePosition - nextNodePosition;
        // Vector from nextNode to playerPos after moving
        Vector2 playerProj = (rbody.position + speed * Time.fixedDeltaTime * railLineProj.normalized) - nextNodePosition;

        // Vector projection to get the t value
        float t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;
        Vector2 targetPos = Vector2.Lerp(nextNodePosition, currentNodePosition, t);

        if (t > 1.0f && nextNodePosition != targetPos)
        {
            float remainingDist = speed * Time.fixedDeltaTime;
            Vector2 distVec = rbody.position - currentNodePosition;

            while (remainingDist - distVec.magnitude > 0.0f && currentNodeIndex != 0)
            {
                --currentNodeIndex;

                remainingDist -= distVec.magnitude;

                currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
                nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

                distVec = currentNodePosition - nextNodePosition;
            }

            if (remainingDist - distVec.magnitude > 0.0f)
            {
                rbody.MovePosition(pathCreator.path.GetPoint(0));
            }
            else
            {
                rbody.MovePosition(nextNodePosition + distVec.normalized * remainingDist);
            }
        }
        else
        {
            rbody.MovePosition(targetPos);
        }

        
    }

    /*
     * Move player to the right along the rail
     */
    void MoveRight(float speed)
    {
        if (currentNodeIndex == pathCreator.path.NumPoints - 1) return;

        Vector2 nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

        Vector2 currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
            

        Vector2 railLineProj = nextNodePosition - currentNodePosition;
        Vector2 playerProj = (rbody.position + speed * Time.fixedDeltaTime * railLineProj.normalized) - currentNodePosition;

        // Vector projection to get t value for Lerp
        float t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;

        Vector2 targetPos = Vector2.Lerp(currentNodePosition, nextNodePosition, t);

        if (t >= 1.0f || nextNodePosition == targetPos)
        {
            float remainingDist =  speed * Time.fixedDeltaTime;
            Vector2 distVec = nextNodePosition - rbody.position;

            while (remainingDist - distVec.magnitude > 0.0f && currentNodeIndex != pathCreator.path.NumPoints - 1)
            {
                ++currentNodeIndex;
                if (currentNodeIndex == pathCreator.path.NumPoints - 1) break;

                remainingDist -= distVec.magnitude;

                currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
                nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

                distVec = nextNodePosition - currentNodePosition;
            }

            if(currentNodeIndex == pathCreator.path.NumPoints - 1)
            {
                rbody.MovePosition(pathCreator.path.GetPoint(currentNodeIndex));
            } else
            {
                Vector2 movePos = currentNodePosition + distVec.normalized * remainingDist;
                rbody.MovePosition(movePos);
                if (movePos == nextNodePosition) ++currentNodeIndex;
            }
        } else
        {
            rbody.MovePosition(targetPos);
        }

    }
}
