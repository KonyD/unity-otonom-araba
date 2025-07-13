using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightDetector : MonoBehaviour
{
    private WaypointMover mover;
    private float carSpeed;
    private string detectedLight = "Yok";

    [SerializeField] private int raycastLength;
    [SerializeField] private Transform rayOrigin;

    void Start()
    {
        mover = GetComponent<WaypointMover>();
        carSpeed = mover.currentSpeed;
    }

    void Update()
    {
        carSpeed = mover.currentSpeed;  // update continuously

        Vector3[] directions = {
        transform.forward,
        //Quaternion.Euler(0, 15, 0) * transform.forward,
        //Quaternion.Euler(0, -15, 0) * transform.forward
        };

        

        foreach (var dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin.position, dir, out hit, raycastLength))
            {
                Debug.DrawRay(rayOrigin.position, dir * hit.distance, Color.green);
                if (hit.collider.CompareTag("Traffic Light"))
                {
                    TrafficLightController tl = hit.collider.GetComponent<TrafficLightController>();
                    if (tl != null)
                    {
                        switch (tl.CurrentState)
                        {
                            case TrafficLightState.Red:
                                mover.currentSpeed = 0;
                                detectedLight = "Kýrmýzý";
                                break;
                            case TrafficLightState.Yellow:
                                mover.currentSpeed = carSpeed / 2f;
                                detectedLight = "Sarý";
                                break;
                            case TrafficLightState.Green:
                                detectedLight = "Yeþil";
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(rayOrigin.position, dir * raycastLength, Color.red);
                detectedLight = "Yok";
            }
        }
    }

    // Draw the GUI
    private void OnGUI()
    {
        GUI.Label(
        new Rect(10, 40, 300, 20),
        $"Trafik ýþýðý: {detectedLight}"
    );
    }
}
