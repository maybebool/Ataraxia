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
    // public InputActionReference inputAction; 
    // private Coroutine savePositionCoroutine;

    private float previousDegree;
    private float previousDelta;
    private float oscillationDelta;
    private float lastUpdateTime;

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
    
    // private void OnEnable() {
    //     inputAction.action.Enable(); // Enable the input action
    //     inputAction.action.performed += OnActionPerformed; // Subscribe to the performed event
    //     inputAction.action.canceled += OnActionCanceled;   // Subscribe to the canceled event
    // }
    // private void OnDisable() {
    //     inputAction.action.performed -= OnActionPerformed; // Unsubscribe from the performed event
    //     inputAction.action.canceled -= OnActionCanceled;   // Unsubscribe from the canceled event
    //     inputAction.action.Disable(); // Disable the input action
    // }
    //
    // private void OnActionPerformed(InputAction.CallbackContext context) {
    //     if (savePositionCoroutine == null) {
    //         savePositionCoroutine = StartCoroutine(SavePositionCoroutine()); // Start the coroutine
    //     }
    // }
    //
    // private void OnActionCanceled(InputAction.CallbackContext context) {
    //     if (savePositionCoroutine != null) {
    //         StopCoroutine(savePositionCoroutine); // Stop the coroutine
    //         savePositionCoroutine = null;
    //     }
    // }

    private void Update() {
        raycastPoint.TryGetCurrent3DRaycastHit(out var hit);
        scO.CurrentPos = hit.point;
        
        outterCirclePosition = new Vector3(scO.CurrentPos.x, scO.CurrentPos.y, scO.CurrentPos.z - 0.1f);
        var quadrantRadiant = CalculateQuadrantLogicForRadiant();
        GetRadiantPointReflection(quadrantRadiant, _outterCircle.w);
        CalculateTremor();
        Debug.Log("Radiant: " + scO.degree);
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
            Debug.Log("Tremor");
        }

        
        oscillationDelta = previousDelta + deltaDegree;
        if (Mathf.Abs(oscillationDelta) > oscillationThreshold) {
            // Debug.Log($"Degree is oscillating! Total oscillation: {oscillationDelta} degrees");
            detector.GetComponent<Renderer>().material.color = Color.red;
            oscillationDelta = 0; // Reset oscillation detection.
        }

        
        previousDegree = scO.degree;
        previousDelta = deltaDegree;
        lastUpdateTime = currentTime;
    }

    private IEnumerator SavePositionCoroutine() {
        while (true) {
            EnqueuePosition(scO.CurrentPos);
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
        var distance = Vector3.Distance(lastPosition, scO.CurrentPos);
        return distance;
    }

    private float CalculateQuadrantLogicForRadiant() {
        var quadrantBasedRadiant = 0f;
        var deltaX = GetLastPosition().x - scO.CurrentPos.x;
        var deltaY = GetLastPosition().y - scO.CurrentPos.y;
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
}