using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyLaserController : MonoBehaviour
{
    [SerializeField]
    float laserLength = 5.0f;
    [SerializeField]
    float spinRate = 90.0f;
    [SerializeField]
    private LayerMask layerMask;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, spinRate) * Time.fixedDeltaTime);
        var rotVec = Quaternion.Euler(0, 0, transform.eulerAngles.z) * (laserLength * Vector3.right);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rotVec, laserLength, layerMask);
        lineRenderer.SetPosition(0, transform.position);

        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
            var playerScript = hit.collider.gameObject.GetComponentInParent<PlayerController>();
            playerScript?.Die();
        } else
        {
            lineRenderer.SetPosition(1, transform.position + rotVec);
        }
    }
    void OnValidate()
    {
        var lRenderer = GetComponent<LineRenderer>();
        lRenderer.SetPosition(0, transform.position);
        var rotVec = Quaternion.Euler(0, 0, transform.eulerAngles.z) * (laserLength * Vector3.right);
        lRenderer.SetPosition(1, transform.position + rotVec);
    }
}
