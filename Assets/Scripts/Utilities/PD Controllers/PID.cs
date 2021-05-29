using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PID
{
    // @Refactor: 
    // Doesn't seem to work, also needs to be generalized to work with different types.
    // (i.e. Vector3, Quaternions, etc.)

    public float Kp, Ki, Kd;
    
    float tau; // Derivative low-pass filter

    float limitMin, limitMax; // Output limits
    float intLimitMin, intLimitMax; // Integrator limits

    float integrator, differentiator; // Integral & Differential
    float prevError, prevMeasurement;

    float result;

    public PID(float P, float I, float D, float tau, float limitMin, float limitMax, float intLimitMin, float intLimitMax)
    {
        Kp = P;
        Ki = I;
        Kd = D;

        this.tau         = tau;
        this.limitMin    = limitMin;
        this.limitMax    = limitMax;
        this.intLimitMin = intLimitMin;
        this.intLimitMax = intLimitMax;

        integrator      = 0;
        differentiator  = 0;
        prevError       = 0;
        prevMeasurement = 0;
        result          = 0;
    }

    public float Update(float target, float measurement, float sampleTime)
    {
        float error = target - measurement;

        float proportional = Kp * error;

        integrator += 0.5f * Ki * sampleTime * (error + prevError);
        integrator = Mathf.Clamp(integrator, intLimitMin, intLimitMax); // Anti-windup via integrator clamping

        differentiator = -(2f * Kd * (error - prevError)
                       +  (2f * tau - sampleTime) * differentiator) / (2f * tau + sampleTime);


        prevError = error;
        prevMeasurement = measurement;

        result = proportional + integrator + differentiator;
        return Mathf.Clamp(result, limitMin, limitMax);
    }
}
