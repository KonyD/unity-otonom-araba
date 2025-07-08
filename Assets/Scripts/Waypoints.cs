using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float waypointSize = 0.3f;

    [Header("Path Settings")]
    [SerializeField] private bool looped = true;

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

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            if (looped)
            {
                return transform.GetChild(0);
            }
            else
            {
                return transform.GetChild(currentWaypoint.GetSiblingIndex());
            }
        }
    }
}
