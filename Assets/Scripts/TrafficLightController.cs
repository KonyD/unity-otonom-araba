using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrafficLightState { Red, Yellow, Green }

public class TrafficLightController : MonoBehaviour
{
    private Renderer redLight;
    private Renderer yellowLight;
    private Renderer greenLight;

    [SerializeField] private Material offMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material greenMaterial;

    [SerializeField] private float redDuration = 5f;
    [SerializeField] private float greenDuration = 5f;
    [SerializeField] private float yellowDuration = 2f;

    public TrafficLightState CurrentState = TrafficLightState.Red;
    

    void Start()
    {
        Transform trafficLight = transform.parent.Find("Plane.001/trafik ýþýðý");
        if (trafficLight == null)
        {
            Debug.LogError("trafik ýþýðý not found!");
            return;
        }

        foreach (Transform child in trafficLight)
        {
            switch (child.name)
            {
                case "Light 1":
                    redLight = child.GetComponent<Renderer>();
                    break;
                case "Light2":
                    yellowLight = child.GetComponent<Renderer>();
                    break;
                case "light 3":
                    greenLight = child.GetComponent<Renderer>();
                    break;
            }
        }

        if (redLight && yellowLight && greenLight)
            StartCoroutine(TrafficLightCycle());
        else
            Debug.LogError("One or more light objects not found.");
    }

    IEnumerator TrafficLightCycle()
    {
        while (true)
        {
            // Red
            CurrentState = TrafficLightState.Red;
            SetLight(redLight, redMaterial);
            SetLight(yellowLight, offMaterial);
            SetLight(greenLight, offMaterial);
            yield return new WaitForSeconds(redDuration);

            // Green
            CurrentState = TrafficLightState.Green;
            SetLight(redLight, offMaterial);
            SetLight(greenLight, greenMaterial);
            yield return new WaitForSeconds(greenDuration);

            // Yellow
            CurrentState = TrafficLightState.Yellow;
            SetLight(greenLight, offMaterial);
            SetLight(yellowLight, yellowMaterial);
            yield return new WaitForSeconds(yellowDuration);
        }
    }

    void SetLight(Renderer renderer, Material mat)
    {
        if (renderer != null)
            renderer.material = mat;
    }
}
