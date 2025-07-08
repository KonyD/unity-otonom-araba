using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float waypointSize = 0.3f;

    [Header("Path Settings")]
    [SerializeField] private bool looped = true;
    [SerializeField] private bool isMovingForward = true;

    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
        }

        // If the path is set to loop then draww a line between the last and first waypoint
        if (looped)
        {
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }
    }

    // Will get the correct next waypoint based on the direction currently travelling
    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        // Stores the index of the current waypoint
        int currentIndex = currentWaypoint.GetSiblingIndex();
        // Stores the index of the next waypoint to travel towards
        int nextIndex = currentIndex;

        // Agent is moving forward on the path
        if (isMovingForward) 
        {
            nextIndex++;

            if (nextIndex == transform.childCount)
            {
                if (looped)
                {
                    nextIndex = 0;
                }
                else
                {
                    nextIndex--;
                }
            }
        }
        // Agent is moving forward on the path
        else
        {
            nextIndex--;

            if (nextIndex < 0)
            {
                if (looped)
                {
                    nextIndex = transform.childCount - 1;
                }
                else
                {
                    nextIndex++;
                }
            }
        }

        return transform.GetChild(nextIndex);
    }
}
