using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private float speed = 5f;

    [SerializeField] private int raycastLength;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
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
                        if (tl.CurrentState == TrafficLightState.Red)
                        {
                            Debug.Log("Red");
                        }
                        else if (tl.CurrentState == TrafficLightState.Yellow)
                        {
                            Debug.Log("Yellow");
                        }
                        if (tl.CurrentState == TrafficLightState.Green)
                        {
                            Debug.Log("Green");
                        }

                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, dir * raycastLength, Color.red);
            }
            


            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            controller.Move(move * Time.deltaTime * speed);
        }
    }
}
