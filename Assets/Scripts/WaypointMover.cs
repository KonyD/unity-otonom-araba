using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;

    [SerializeField] public float targetSpeed = 5f;

    [Range(0f, 100f)]
    [SerializeField] private float rotationSpeed = 100f;

    public float currentSpeed = 0f;
    private float actualSpeed = 0f;
    private Vector3 lastPosition;

    [SerializeField] private float distanceThreshold = 0.1f;

    private string turnDirection;

    private Transform currentWaypoint;

    private PIDController rotationPID;
    private PIDController speedPID;

    [Header("Speed PID Settings")]
    [SerializeField] private float speedKp = 1f;
    [SerializeField] private float speedKi = 0f;
    [SerializeField] private float speedKd = 0.2f;

    [Header("Rotation PID Settings")]
    [SerializeField] private float Kp = 1.5f;
    [SerializeField] private float Ki = 0.05f;
    [SerializeField] private float Kd = 1.0f;

    void Start()
    {
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);

        speedPID = new PIDController(speedKp, speedKi, speedKd);
        lastPosition = transform.position;
        rotationPID = new PIDController(Kp, Ki, Kd);
    }

    void Update()
    {
        Vector3 toWaypoint = currentWaypoint.position - transform.position;
        Vector3 forward = transform.forward;

        float angleError = Vector3.SignedAngle(forward, toWaypoint, Vector3.up);
        if (Mathf.Abs(angleError) < 1f) angleError = 0f;

        float pidOutput = rotationPID.Update(angleError, Time.deltaTime);
        pidOutput = Mathf.Clamp(pidOutput, -rotationSpeed, rotationSpeed);

        Quaternion deltaRotation = Quaternion.AngleAxis(pidOutput * Time.deltaTime, Vector3.up);
        transform.rotation = deltaRotation * transform.rotation;

        actualSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        float speedError = targetSpeed - actualSpeed;
        float speedOutput = speedPID.Update(speedError, Time.deltaTime);

        currentSpeed += speedOutput * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, targetSpeed);

        Vector3 moveDirection = toWaypoint.normalized;
        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            rotationPID.Reset();
            speedPID.Reset();
        }

        if (Mathf.Abs(angleError) < 10f)
            turnDirection = "Düz";
        else if (angleError > 0)
            turnDirection = "Sað";
        else
            turnDirection = "Sol";
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Araba Yönü: {turnDirection}");
    }
}
