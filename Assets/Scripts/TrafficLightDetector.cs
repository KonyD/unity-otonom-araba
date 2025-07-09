using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightDetector : MonoBehaviour
{
    private WaypointMover mover;
    private float carSpeed;
    [SerializeField] private int raycastLength;

    void Start()
    {
        mover = GetComponent<WaypointMover>();
        carSpeed = mover.moveSpeed;
    }

    void Update()
    {
        Vector3[] directions = {
        transform.forward,
        Quaternion.Euler(0, 15, 0) * transform.forward,
        Quaternion.Euler(0, -15, 0) * transform.forward
        };

        foreach (var dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, raycastLength))
            {
                Debug.DrawRay(transform.position, dir * hit.distance, Color.green);
                if (hit.collider.CompareTag("Traffic Light"))
                {
                    TrafficLightController tl = hit.collider.GetComponent<TrafficLightController>();
                    if (tl != null)
                    {
                        switch (tl.CurrentState)
                        {
                            case TrafficLightState.Red:
                                mover.moveSpeed = 0;
                                break;
                            case TrafficLightState.Yellow:
                                mover.moveSpeed = carSpeed / 2f;
                                break;
                            case TrafficLightState.Green:
                                mover.moveSpeed = carSpeed;
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, dir * raycastLength, Color.red);
            }
        }
    }
}
