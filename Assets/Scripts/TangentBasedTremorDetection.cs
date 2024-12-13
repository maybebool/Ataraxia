using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TangentBasedTremorDetection : MonoBehaviour {
    public DataContainer scO;
    public XRRayInteractor raycastPoint;
    public GameObject detector;
    public Vector4 _outterCircle;
    public float _tangentCircleRadius;
    public float speedThreshold = 50f; 
    public float oscillationThreshold = 140f;
    
    private bool isCollectingData = false;
    private float previousDegree;
    private float previousDelta;
    private float oscillationDelta;
    private float lastUpdateTime;
    private float tremorIntensity = 0f;
    private float tremorDecayRate = 5f; 
    private List<float> tremorEventTimes = new();
    private float timeWindow = 3f; 
    private float multiplierThreshold = 3f; 
    private Vector3 previousPosition;
    private bool hasPreviousPosition = false;
    private Vector3 currentPos;
    private Vector3 outterCirclePosition;
    private Vector3 outterCircleScale;
    private Vector3 tangentCircleScale;
    private Coroutine dataCollectionCoroutine;

    private const float PiHalf = Mathf.PI / 2;
    private const float PIDoubled = 2 * Mathf.PI;
    
    private void Start() {
        previousDegree = scO.degree;
        lastUpdateTime = Time.time;
        previousDelta = 0f;
        outterCircleScale = new Vector3(_outterCircle.w, _outterCircle.w, _outterCircle.w) * 2;
        tangentCircleScale = new Vector3(_tangentCircleRadius, _tangentCircleRadius, _tangentCircleRadius) * 2;
    }
    
    private void OnEnable()
    {
        // Subscribe to events from EventManager
        if (MtsEventManager.Instance != null)
        {
            MtsEventManager.Instance.OnButtonPressed += StartDataCollection;
            MtsEventManager.Instance.OnButtonReleased += StopDataCollection;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        if (MtsEventManager.Instance != null)
        {
            MtsEventManager.Instance.OnButtonPressed -= StartDataCollection;
            MtsEventManager.Instance.OnButtonReleased -= StopDataCollection;
        }
    }
    

    private void Update() {
        if (isCollectingData) return;
        // Decrease tremorIntensity over time when not collecting data
        tremorIntensity -= tremorDecayRate * Time.deltaTime;
        tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
        scO.tremorIntensity = tremorIntensity;
    }
    
    
    private void StartDataCollection() {
        isCollectingData = true;
        scO.isCollectingData = true; // Set the flag in the ScriptableObject
        ResetTremorDetectionVariables();
        dataCollectionCoroutine = StartCoroutine(DataCollectionRoutine());
    }

    private void StopDataCollection() {
        isCollectingData = false;
        scO.isCollectingData = false; // Reset the flag in the ScriptableObject
        if (dataCollectionCoroutine != null) {
            StopCoroutine(dataCollectionCoroutine);
            dataCollectionCoroutine = null;
        }
        hasPreviousPosition = false; // Reset previous position
    }
    
    private IEnumerator DataCollectionRoutine() {
        while (isCollectingData) {
            
            raycastPoint.TryGetCurrent3DRaycastHit(out var hit);
            scO.currentPos = hit.point;

            if (hasPreviousPosition) {
                var deltaX = previousPosition.x - scO.currentPos.x;
                var deltaY = previousPosition.y - scO.currentPos.y;
                var hypotenuse = CalculateHypotenuse(previousPosition, scO.currentPos);

                var quadrantRadiant = CalculateQuadrantLogicForRadiant(deltaX, deltaY, hypotenuse);
                scO.degree = quadrantRadiant * Mathf.Rad2Deg;

                GetRadiantPointReflection(quadrantRadiant, _outterCircle.w);
                CalculateTremor();
                
                tremorIntensity -= tremorDecayRate * Time.deltaTime;
                tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
                scO.tremorIntensity = tremorIntensity;
            }
            else {
                
                previousDegree = scO.degree;
                lastUpdateTime = Time.time;
                previousDelta = 0f;
            }
            
            previousPosition = scO.currentPos;
            hasPreviousPosition = true;
            
            yield return null;
        }
    }
    
    private void ResetTremorDetectionVariables()
    {
        previousDegree = scO.degree;
        previousDelta = 0f;
        oscillationDelta = 0f;
        tremorIntensity = 0f;
        tremorEventTimes.Clear();
    }

    private void CalculateTremor() {
        var currentTime = Time.time;
        var deltaTime = currentTime - lastUpdateTime;

        
        var deltaDegree = scO.degree - previousDegree;
        if (deltaDegree > 180f) deltaDegree -= 360f;
        if (deltaDegree < -180f) deltaDegree += 360f;

        
        var speed = Mathf.Abs(deltaDegree / deltaTime);

        
        if (speed > speedThreshold) {
            detector.GetComponent<Renderer>().material.color = Color.blue;
        }

        
        oscillationDelta = previousDelta + deltaDegree;
        if (Mathf.Abs(oscillationDelta) > oscillationThreshold) {
            detector.GetComponent<Renderer>().material.color = Color.red;
            oscillationDelta = 0;
            IncrementTremorIntensityValue();
        }
        
        previousDegree = scO.degree;
        previousDelta = deltaDegree;
        lastUpdateTime = currentTime;
    }
    
    private Vector3 GetRadiantPointReflection(float radiant, float scale) {
        var sin = scale * (float)Math.Sin(radiant);
        var cos = scale * (float)Math.Cos(radiant);
        return new Vector3(sin, -cos, 0);
    }
    
    private float CalculateHypotenuse(Vector3 pointA, Vector3 pointB) {
        return Vector3.Distance(pointA, pointB);
    }

    private float CalculateQuadrantLogicForRadiant(float deltaX, float deltaY, float hypotenuse) {
        if (hypotenuse == 0) {
            return 0f;
        }
        var quadrantBasedRadiant = 0f;
        var thetaOfCos = Mathf.Acos(deltaX / hypotenuse);

        quadrantBasedRadiant = deltaX switch {
            // top left Quadrant > < | 0 - 90
            > 0 when deltaY < 0 => PiHalf - thetaOfCos,
            // bottom left Quadrant > > | 90 - 180
            > 0 when deltaY > 0 => PiHalf + thetaOfCos,
            // bottom right Quadrant < > | 180 - 270
            < 0 when deltaY > 0 => PIDoubled + PiHalf - thetaOfCos,
            // top right Quadrant < <  | 270 - 360
            < 0 when deltaY < 0 => PIDoubled + PiHalf - thetaOfCos,
            _ => quadrantBasedRadiant
        };
        scO.degree = quadrantBasedRadiant * Mathf.Rad2Deg;
        return quadrantBasedRadiant;
    }
    
    private void IncrementTremorIntensityValue() {
        var incrementAmount = 0.3f; 
        tremorEventTimes.Add(Time.time);
        tremorEventTimes.RemoveAll(t => t < Time.time - timeWindow);
        var eventCount = tremorEventTimes.Count;

        if (eventCount >= multiplierThreshold)
        {
            var multiplier = 1f + (eventCount - multiplierThreshold) * 0.01f; 
            incrementAmount *= multiplier;
        }
        
        tremorIntensity += incrementAmount;
        tremorIntensity = Mathf.Clamp(tremorIntensity, 0f, 10f);
    }
}