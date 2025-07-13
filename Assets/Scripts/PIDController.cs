using UnityEngine;

public class PIDController
{
    public float Kp, Ki, Kd;
    private float integral;
    private float lastError;

    public PIDController(float kp, float ki, float kd)
    {
        Kp = kp;
        Ki = ki;
        Kd = kd;
        integral = 0f;
        lastError = 0f;
    }

    public float Update(float error, float deltaTime)
    {
        integral += error * deltaTime;
        float derivative = (error - lastError) / deltaTime;
        lastError = error;
        return Kp * error + Ki * integral + Kd * derivative;
    }

    public void Reset()
    {
        integral = 0f;
        lastError = 0f;
    }
}