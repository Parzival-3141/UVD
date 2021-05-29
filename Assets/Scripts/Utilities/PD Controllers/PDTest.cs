using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDTest : MonoBehaviour
{
    [Header("Objects")]
    public Transform target;
    public Rigidbody followRB;

    [Header("Test Controls")]
    public bool doTests = true;
    public bool lerpPositions;
    public bool lerpRotations;

    [Range(0.1f, 60)] public float updateRate = 1; // per second
    [Space]
    public bool randomPositions;
    public bool randomRotations;
    [Space]
    public bool sampleAverage = false;
    public int averageSampleSize = 60;

    [ReadOnly] public float averagePosDelta;
    [ReadOnly] public float averageRotDelta;

    [Header("PD Controls")]
    public bool enablePD = true;
    public bool useFreqDamping = true;
    public float frequency;
    public float damping;
    public float kp;
    public float kd;

    private Vector3 randPos;
    private Quaternion randRot;
    private float timer;

    private Vector3 lastVel;
    private Vector3 lastRandomPos;
    private Vector3 origin;
    private Vector3 posOffset     = Vector3.right * 1.5f;
    private PDController pd       = new PDController();
    private List<float> posDeltas = new List<float>();
    private List<float> rotDeltas = new List<float>();
    private Coroutine runningCoroutine = null;


    private void Awake()
    {
        followRB.maxAngularVelocity = Mathf.Infinity;
        origin = target.position; // origin is position at start of play
        lastRandomPos = target.position;
        timer = 1f / updateRate;
    }

    private void Update()
    {
        if (doTests)
        {
            if(runningCoroutine == null)
                runningCoroutine = StartCoroutine(NewControlTransform());

            target.localPosition = lerpPositions && target.localPosition != randPos ? Vector3.Lerp(target.localPosition, randPos, 1 - timer) : randPos;
            target.rotation      = lerpRotations && target.rotation != randRot      ? Quaternion.Slerp(target.rotation, randRot, 1 - timer)  : randRot;

            timer = Mathf.MoveTowards(timer, 0, Time.deltaTime);
            if (timer == 0)
                timer = 1f / updateRate;
        }
        else if(runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        if (sampleAverage)
        {
            float posDelta = Mathf.Abs((target.position - followRB.position).magnitude - posOffset.magnitude);
            float rotDelta = Mathf.Abs((target.rotation * Quaternion.Inverse(followRB.rotation)).eulerAngles.magnitude * Mathf.Deg2Rad);

            averagePosDelta = CalculateAverage(posDelta, posDeltas, averageSampleSize);
            averageRotDelta = CalculateAverage(rotDelta, rotDeltas, averageSampleSize);
        }
    }

    private void FixedUpdate()
    {
        if (enablePD)
        {
            followRB.AddForce(pd.SPDComputeForce(followRB.position, target.position + posOffset, followRB.velocity, lastVel));
            followRB.AddTorque(pd.ComputeTorque(target.rotation, followRB.rotation, followRB));
        }
        
        lastVel = followRB.velocity;
    }

    private void OnValidate()
    {
        if (useFreqDamping)
        {
            pd.ComputeKpAndKd(frequency, damping);
            kp = pd.Kp;
            kd = pd.Kd;
        }
        else
        {
            pd.Kp = kp;
            pd.Kd = kd;
        }
    }

    private IEnumerator NewControlTransform()
    {

        while (true)
        {
            randPos = Vector3.zero;
            randRot = Quaternion.identity;

            if (randomPositions)
            { //@Incomplete: need to select random point from surface of a unitSphere at lastPos intersecting with another unitSphere thats at the origin
                randPos = origin + Random.onUnitSphere;
                //randPos = new Vector3(Random.value, Random.value, Random.value).normalized;
            }

            if (randomRotations)
                randRot = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            lastRandomPos = randPos;

            yield return new WaitForSeconds(1f / updateRate);
        }
    }

    private float CalculateAverage(float currentValue, List<float> list, int sampleSize)
    {
        list.Insert(0, currentValue);
        
        if (list.Count - sampleSize > 1)
            list.RemoveRange(sampleSize, list.Count - sampleSize);
        else if(list.Count > sampleSize)
            list.RemoveAt(list.Count - 1);

        float a = 0f;
        for (int i = 0; i < list.Count; i++)
            a += list[i];

        return a / sampleSize;
    }
}
