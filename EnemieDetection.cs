using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public GameObject playerObject;
    public float visionDistance = 30.0f;
    public float visionAngle = 60.0f;

    public bool playerDetected = false;

    void Update()
    {
        playerDetected = false;

        if (playerObject == null)
        {
            Debug.LogWarning("Player object is not assigned in the EnemyDetection script.");
            return;
        }

        Vector3 directionToPlayer = playerObject.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= visionDistance)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= visionAngle / 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, visionDistance))
                {
                    if (hit.collider.gameObject == playerObject)
                    {
                        playerDetected = true;
                        Debug.Log("Enemy has detected the player!");
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (playerObject != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, playerObject.transform.position);
        }

        Gizmos.color = Color.yellow;
        Vector3 frontRayPoint = transform.position + (transform.forward * visionDistance);
        Gizmos.DrawLine(transform.position, frontRayPoint);
        Gizmos.DrawWireSphere(frontRayPoint, 0.5f);

        Gizmos.color = Color.blue;
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * visionDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * visionDistance;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}