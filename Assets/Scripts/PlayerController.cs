using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PathCreation;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public PathCreator pathCreator;
    Animator animator;

    bool hasBeatenLevel = false;

    public float speed;
    private int currentNodeIndex = 0;  // If player is between node n and n + 1, then currentNode = n
    Rigidbody2D rbody;
    int moveDirection = 0;             // normalized value where -1 = left, 1 = right

    int hp = 2;
    Timer invincibleTimer;
    SpriteRenderer spriteRenderer;

    CircleCollider2D hitbox;
    Timer colorBlinkTimer;

    bool isAlive = true;

    void Start()
    {
        transform.position = pathCreator.path.GetPoint(currentNodeIndex);
        rbody = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        hitbox = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponent<Animator>();

        invincibleTimer = new Timer(2.0f);
        colorBlinkTimer = new Timer(1.5f, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        invincibleTimer.Tick();
        colorBlinkTimer.Tick();

        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) hp += 1;
        #endif

        if (invincibleTimer.IsReady())
        {
            spriteRenderer.color = Color.white;
        } else
        {
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        

        moveDirection = 0;
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + hitbox.radius * hitbox.transform.localScale.x) moveDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - hitbox.radius * hitbox.transform.localScale.x) moveDirection = 1;
        }

        if (hasBeatenLevel) moveDirection = 1;

        var cameraScript = Camera.main.GetComponent<CameraScroll>();
        if (CameraScroll.IsSpriteOffScreen(hitbox.gameObject) && !hasBeatenLevel) Die();
        if (IsDerailed()) Die();

        ColorBlink();
    }

    void ColorBlink()
    {
        if(hp == 1)
        {
            if (colorBlinkTimer.TimeRatio < 0.05f) spriteRenderer.material.SetColor("_ColorSub", new Color(0.0f, 1.0f, 1.0f, 1.0f));
            else spriteRenderer.material.SetColor("_ColorSub", new Color(0.0f, 0.1f, 0.25f, 1.0f));
        } else
        {
             spriteRenderer.material.SetColor("_ColorSub", Color.black);
        }
        colorBlinkTimer.ResetTimer();

    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        if (moveDirection == -1) MoveLeft(speed);
        else if (moveDirection == 1) MoveRight(speed);
        else ClampDown();

        animator.SetInteger("Move X", moveDirection);
    }

    public void ChangeHealth(int healthChange)
    {
        if(healthChange < 0)
        {
            if (invincibleTimer.ResetTimer()) hp += healthChange;
        } else
        {
            hp += healthChange;
        }

        if(hp > 1)
        {
            spriteRenderer.material.SetColor("_Color", Color.black);
        } else if(hp == 1)
        {
            spriteRenderer.material.SetColor("_Color", new Color(0.5f, 0.0f, 0.0f));
        }

        if(hp <= 0) Die();

    }

    public void Die()
    {
        animator.SetBool("Alive", false);
        if(isAlive) StartCoroutine(DieCoroutine());
        isAlive = false;
    }

    IEnumerator DieCoroutine()
    {
        GetComponent<ClickManager>().enabled = false;
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private bool IsDerailed()
    {
        Vector2 pathPos = pathCreator.path.GetClosestPointOnPath(transform.position);
        float radius = hitbox.radius;
        Vector2 diffVec = pathPos - (Vector2)transform.position;
        return (diffVec.sqrMagnitude > radius * radius);
    }

    void ClampDown()
    {
        if (currentNodeIndex == pathCreator.path.NumPoints - 1)
        {
            rbody.MovePosition(pathCreator.path.GetPoint(currentNodeIndex));
            return;
        }

        Vector2 currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
        Vector2 nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

        // Vector from nextNode to curNode;
        Vector2 railLineProj = nextNodePosition - currentNodePosition;

        Vector2 playerProj = rbody.position - currentNodePosition;

        // Vector projection to get the t value
        float t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;
        Vector2 targetPos = Vector2.Lerp(currentNodePosition, nextNodePosition, t);
        rbody.MovePosition(targetPos);
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


            while (remainingDist - distVec.magnitude >= -0.001f && currentNodeIndex != pathCreator.path.NumPoints - 1)
            {
                remainingDist -= distVec.magnitude;
                ++currentNodeIndex;

                if (currentNodeIndex == pathCreator.path.NumPoints - 1) break;


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

                nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

                 currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);


                 railLineProj = nextNodePosition - currentNodePosition;
                 playerProj = (rbody.position + speed * Time.fixedDeltaTime * railLineProj.normalized) - currentNodePosition;

                // Vector projection to get t value for Lerp
                 t = Vector2.Dot(playerProj, railLineProj) / railLineProj.sqrMagnitude;

                if (movePos == nextNodePosition || t >= 1.0f) ++currentNodeIndex;
            }
        } else
        {
            rbody.MovePosition(targetPos);
        }

    }

    IEnumerator LevelEndCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        MoveToNextLevel();
    }

    private void MoveToNextLevel()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        int totalIndex = SceneManager.sceneCountInBuildSettings;

        if (++levelIndex >= totalIndex)
        {
            levelIndex = 0;
        }
        SceneManager.LoadScene(levelIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(LevelEndCoroutine());
            hasBeatenLevel = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var obj = collision.collider.gameObject;
        if (obj.GetComponent<Camera>() != null)
        {
            if(!hasBeatenLevel) CollideWithEdge();
            else GetComponentInChildren<CircleCollider2D>().isTrigger = true;
        }
    }

    private void CollideWithEdge()
    {
        if (currentNodeIndex == pathCreator.path.NumPoints - 1) return;
        else
        {
            Vector2 currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
            Vector2 nextNodePosition = pathCreator.path.GetPoint(currentNodeIndex + 1);

            Vector2 railLineProj = nextNodePosition - currentNodePosition;
            Vector2 playerProj = (rbody.position) - currentNodePosition;

            float t;
            Vector2 chooseDirVec = Camera.main.WorldToViewportPoint((Vector3)rbody.position);

            chooseDirVec -= new Vector2(0.5f, 0.5f);

            if (Mathf.Abs(chooseDirVec.x) - Mathf.Abs(chooseDirVec.y) > 0.0f) t = railLineProj.x != 0.0f ? playerProj.x / railLineProj.x : 1.0f;
            else t = railLineProj.y != 0.0f ? playerProj.y / railLineProj.y : 1.0f;

            Vector2 targetPos = Vector2.Lerp(currentNodePosition, nextNodePosition, t);
            if (t < 0.0f && targetPos != currentNodePosition && currentNodeIndex != 0)
            {
                --currentNodeIndex;
                currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
                if ((currentNodePosition - (Vector2)transform.position).sqrMagnitude > 0.5f * 0.5f) ++currentNodeIndex;
            }
            else if (t >= 1.0f || targetPos == nextNodePosition)
            {
                ++currentNodeIndex;
                currentNodePosition = pathCreator.path.GetPoint(currentNodeIndex);
                if ((currentNodePosition - (Vector2)transform.position).sqrMagnitude > 0.5f * 0.5f) --currentNodeIndex;
            }

            if (currentNodeIndex > pathCreator.path.NumPoints - 1) currentNodeIndex = pathCreator.path.NumPoints - 1;
            if (currentNodeIndex < 0) currentNodeIndex = 0;
            transform.position = targetPos;
        }
    }
}
