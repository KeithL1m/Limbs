using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [Header("Move Positions")]
    public Transform[] waypoints;

    [Header("Adjustable Settings")]
    public float speed = 5f;
    public float waitTime = 1f;

    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            StartCoroutine(MovePlatform());
        }
        else
        {
            Debug.LogError("No waypoints assigned to the platform.");
        }
    }

    IEnumerator MovePlatform()
    {
        while (true)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            yield return MoveToPosition(targetPosition);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition; // Ensure exact positioning
        isMoving = false;
        yield return new WaitForSeconds(waitTime);
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        if (waypoints.Length > 1)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }
    }
}
