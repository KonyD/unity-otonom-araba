using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    // Stores a reference to the waypoint system this object will use
    [SerializeField] private Waypoints waypoints;

    [SerializeField] public float moveSpeed = 5f;

    [Range(0f, 10f)]
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private float distanceThreshold = 0.1f;

    private string turnDirection;

    // The current waypoint target that the object is moving towards
    private Transform currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial position to the first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        // Set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toWaypoint = (currentWaypoint.position - transform.position);
        Vector3 forward = transform.forward;

        RotateTowardsWaypoint(toWaypoint, forward);

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }

        float angle = Vector3.SignedAngle(forward, toWaypoint, Vector3.up);
        if (Mathf.Abs(angle) < 10f)
            turnDirection = "Düz";
        else if (angle > 0)
            turnDirection = "Sað";
        else
            turnDirection = "Sol";
    }

    // Will slowly rotate the agent towards the current waypoint it is moving towards 
    private void RotateTowardsWaypoint(Vector3 toWaypoint, Vector3 forward)
    {
        if (toWaypoint.magnitude > 0.01f)
        {
            Vector3 desiredDirection = toWaypoint.normalized;
            float steerAmount = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.Slerp(forward, desiredDirection, steerAmount);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    // Draw the GUI
    private void OnGUI()
    {
        GUI.Label(
        new Rect(10, 10, 300, 20),
        $"Araba Yönü: {turnDirection}"
    );
    }
}
