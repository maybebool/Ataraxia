using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TangentBasedTremorDetection : MonoBehaviour {
    public DataContainer scO;
    public XRRayInteractor raycastPoint;
    public GameObject detector;
    public Vector4 _outterCircle;
    public float _tangentCircleRadius;
    public GameObject lastPointPrefab;
    public float saveInterval = 2;
    public int _queueCapacity = 2;
    public int _pointsCapacity = 2;
    public float speedThreshold = 50f; 
    public float oscillationThreshold = 140f;

    private float previousDegree;
    private float previousDelta;
    private float oscillationDelta;
    private float lastUpdateTime;
    
    private float tremorIntensity = 0f;
    private float tremorDecayRate = 1f; // The rate at which tremorIntensity decreases per second
    private List<float> tremorEventTimes = new();
    private float timeWindow = 5f; // Time window in seconds to consider for frequency
    private int multiplierThreshold = 5; 

    private Queue<Vector3> positionQueue = new();
    private List<GameObject> lastPointList = new();

    // private Vector3 tempValue;
    private Vector3 currentPos;
    private Vector3 outterCirclePosition;
    private Vector3 outterCircleScale;
    private Vector3 tangentCircleScale;
    private const float PiHalf = Mathf.PI / 2;
    private const float PIDoubled = 2 * Mathf.PI;

    
    private void Start() {
        previousDegree = scO.degree;
        lastUpdateTime = Time.time;
        previousDelta = 0f;
        outterCircleScale = new Vector3(_outterCircle.w, _outterCircle.w, _outterCircle.w) * 2;
        tangentCircleScale = new Vector3(_tangentCircleRadius, _tangentCircleRadius, _tangentCircleRadius) * 2;
        StartCoroutine(SavePositionCoroutine());
    }

    private void OnDestroy() {
        lastPointList.Clear();
        positionQueue.Clear();
    }
    

    private void Update() {
        raycastPoint.TryGetCurrent3DRaycastHit(out var hit);
        scO.currentPos = hit.point;
        
        outterCirclePosition = new Vector3(scO.currentPos.x, scO.currentPos.y, scO.currentPos.z - 0.1f);
        var quadrantRadiant = CalculateQuadrantLogicForRadiant();
        GetRadiantPointReflection(quadrantRadiant, _outterCircle.w);
        CalculateTremor();
        // Decrease tremorIntensity over time
        tremorIntensity -= tremorDecayRate * Time.deltaTime;
        tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);

        // Store tremorIntensity in the ScriptableObject for access from the editor window
        scO.tremorIntensity = tremorIntensity;
        // Debug.Log("Radiant: " + scO.degree);
    }

    private void CalculateTremor() {
        var currentTime = Time.time;
        var deltaTime = currentTime - lastUpdateTime;

        
        var deltaDegree = scO.degree - previousDegree;
        if (deltaDegree > 180f) deltaDegree -= 360f;
        if (deltaDegree < -180f) deltaDegree += 360f;

        
        var speed = Mathf.Abs(deltaDegree / deltaTime);

        
        if (speed > speedThreshold) {
            // Debug.Log($"Degree is changing too fast! Speed: {speed} degrees/second");
            detector.GetComponent<Renderer>().material.color = Color.blue;
            // Debug.Log("Tremor");
        }

        
        oscillationDelta = previousDelta + deltaDegree;
        if (Mathf.Abs(oscillationDelta) > oscillationThreshold) {
            // Debug.Log($"Degree is oscillating! Total oscillation: {oscillationDelta} degrees");
            detector.GetComponent<Renderer>().material.color = Color.red;
            // Debug.Log(oscillationDelta);
            oscillationDelta = 0; // Reset oscillation detection.
            IncrementTremorIntensityValue();
        }
        
        previousDegree = scO.degree;
        previousDelta = deltaDegree;
        lastUpdateTime = currentTime;
    }

    private IEnumerator SavePositionCoroutine() {
        while (true) {
            EnqueuePosition(scO.currentPos);
            // var prefab = Instantiate(lastPointPrefab, GetLastPosition(), Quaternion.identity);
            // lastPointList.Add(prefab);
            // Destroy(lastPointList[0].gameObject);
            // Wait for the specified interval before saving the position again
            yield return new WaitForSeconds(saveInterval);
        }
    }

    void EnqueuePosition(Vector3 position) {
        positionQueue.Enqueue(position);

        if (positionQueue.Count >= _queueCapacity) {
            positionQueue.Dequeue();
        }
    }
    
    private Vector3 GetRadiantPointReflection(float radiant, float scale) {
        var sin = scale * (float)Math.Sin(radiant);
        var cos = scale * (float)Math.Cos(radiant);
        return new Vector3(sin, -cos, 0);
    }

    private Vector3 GetLastPosition() {
        return positionQueue.Count > 0 ? positionQueue.ToArray()[positionQueue.Count - 1] : Vector3.zero;
    }

    private float CalculateHypotenuseToLastPosition() {
        var lastPosition = GetLastPosition();
        var distance = Vector3.Distance(lastPosition, scO.currentPos);
        return distance;
    }

    private float CalculateQuadrantLogicForRadiant() {
        var quadrantBasedRadiant = 0f;
        var deltaX = GetLastPosition().x - scO.currentPos.x;
        var deltaY = GetLastPosition().y - scO.currentPos.y;
        var thetaOfCos = Mathf.Acos(deltaX / CalculateHypotenuseToLastPosition());

        switch (deltaX) {
            // top left Quadrant > < | 0 - 90
            case > 0 when deltaY < 0:
                quadrantBasedRadiant = PiHalf - thetaOfCos;
                break;
            // bottom left Quadrant > > | 90 - 180
            case > 0 when deltaY > 0:
                quadrantBasedRadiant = PiHalf + thetaOfCos;
                break;
            // bottom right Quadrant < > | 180 - 270
            case < 0 when deltaY > 0:
            // top right Quadrant < <  | 270 - 360
            case < 0 when deltaY < 0:
                quadrantBasedRadiant = PIDoubled + PiHalf - thetaOfCos;
                break;
        }

        scO.degree = quadrantBasedRadiant * Mathf.Rad2Deg;
        return quadrantBasedRadiant;
    }
    
    private void IncrementTremorIntensityValue()
    {
        var incrementAmount = 1f; // Base amount to increment

        // Record the time of the tremor event
        tremorEventTimes.Add(Time.time);

        // Remove old events outside the time window
        tremorEventTimes.RemoveAll(t => t < Time.time - timeWindow);

        // Count events within the time window
        int eventCount = tremorEventTimes.Count;

        // Apply a multiplier based on the number of events
        if (eventCount >= multiplierThreshold)
        {
            var multiplier = eventCount / (float)multiplierThreshold;
            incrementAmount *= multiplier;
        }

        // Increase tremorIntensity
        tremorIntensity += incrementAmount;

        // Clamp tremorIntensity between 0 and 10
        tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
    }
}