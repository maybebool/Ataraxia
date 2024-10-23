using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TangentBasedTremorDetection : MonoBehaviour {
    public DataContainer scO;
    public XRRayInteractor raycastPoint;
    public GameObject _circlePrefab;
    public GameObject _radiantPrefab;
    public GameObject detector;
    public Vector4 _outterCircle;
    public float _tangentCircleRadius;
    public GameObject lastPointPrefab;
    public float saveInterval = 2;
    public int _queueCapacity = 2;
    public int _pointsCapacity = 2;


    public float speedThreshold = 50f; // Threshold for how fast the degree can change before triggering a warning.
    public float oscillationThreshold = 140f; // Threshold for detecting oscillation.

    private float previousDegree;
    private float previousDelta;
    private float oscillationDelta;
    private float lastUpdateTime;

    private Queue<Vector3> positionQueue = new();
    private List<GameObject> lastPointList = new();

    // private Vector3 tempValue;
    private GameObject _innerCircleGO, _outterCircleGO, _tangentCircleGO;
    private Vector3 currentPos;
    private const float PiHalf = Mathf.PI / 2;
    private const float PiSquared = 2 * Mathf.PI;
    private const float PiQ3 = 3 * Mathf.PI / 2;

    // Start is called before the first frame update
    private void Start() {
        previousDegree = scO.degree;
        lastUpdateTime = Time.time;
        previousDelta = 0f;
        _outterCircleGO = Instantiate(_circlePrefab);
        _tangentCircleGO = Instantiate(_radiantPrefab);
        // StartCoroutine(SavePositionCoroutine());
    }

    private void OnDestroy() {
        lastPointList.Clear();
        positionQueue.Clear();
    }

    private void Update() {
        raycastPoint.TryGetCurrent3DRaycastHit(out var hit);
        scO.CurrentPos = hit.point;
        hit.normal = scO.CurrentPos;
            
        _outterCircleGO.transform.position = new Vector3(scO.CurrentPos.x, scO.CurrentPos.y, scO.CurrentPos.z - (float)0.1);
        _outterCircleGO.transform.localScale = new Vector3(_outterCircle.w, _outterCircle.w, _outterCircle.w) * 2;

        _tangentCircleGO.transform.position = GetRotatedTangent(CalculateQuadrantLogicForRadiant(), _outterCircle.w) + _outterCircleGO.transform.position;
        _tangentCircleGO.transform.localScale = new Vector3(_tangentCircleRadius, _tangentCircleRadius, _tangentCircleRadius) * 2;

        if (Input.GetKeyDown(KeyCode.Space)) {
            positionQueue.Enqueue(scO.CurrentPos);
            Instantiate(lastPointPrefab, GetLastPosition(), Quaternion.identity);
        }
        CalculateTremor();
        Debug.Log("Radiant: " + scO.degree);
    }

    private void CalculateTremor() {
        var currentTime = Time.time;
        var deltaTime = currentTime - lastUpdateTime;

        // Calculate the change in degree value, accounting for the wrap-around from 360 to 0.
        var deltaDegree = scO.degree - previousDegree;
        if (deltaDegree > 180f) deltaDegree -= 360f;
        if (deltaDegree < -180f) deltaDegree += 360f;

        // Calculate the speed of degree change (degree per second).
        var speed = Mathf.Abs(deltaDegree / deltaTime);

        // Check if the speed exceeds the threshold.
        if (speed > speedThreshold) {
            // Debug.Log($"Degree is changing too fast! Speed: {speed} degrees/second");
            detector.GetComponent<Renderer>().material.color = Color.blue;
        }

        // Check for oscillation. This is detected when the change in degree is significantly large and changes direction quickly.
        oscillationDelta = previousDelta + deltaDegree;
        if (Mathf.Abs(oscillationDelta) > oscillationThreshold) {
            // Debug.Log($"Degree is oscillating! Total oscillation: {oscillationDelta} degrees");
            detector.GetComponent<Renderer>().material.color = Color.red;
            oscillationDelta = 0; // Reset oscillation detection.
        }

        // Update the previous values for the next frame.
        previousDegree = scO.degree;
        previousDelta = deltaDegree;
        lastUpdateTime = currentTime;
    }

    private IEnumerator SavePositionCoroutine() {
        while (true) {
            // Save the current position to the queue
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


    private Vector3 GetRotatedTangent(float radiant, float scale) {
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
        var aOfCos = Mathf.Acos(deltaX / CalculateHypotenuseToLastPosition());

        switch (deltaX) {
            // bottom right Quadrant < >
            case < 0 when deltaY > 0:
            // bottom left Quadrant > >
            case > 0 when deltaY > 0:
                quadrantBasedRadiant = PiHalf + aOfCos;
                break;
            // top right Quadrant < < 
            case < 0 when deltaY < 0:
                quadrantBasedRadiant = PiSquared + PiHalf - aOfCos;
                break;
            // top left Quadrant > <
            case > 0 when deltaY < 0:
                quadrantBasedRadiant = PiHalf - aOfCos;
                break;
        }

        scO.degree = quadrantBasedRadiant * Mathf.Rad2Deg;
        return quadrantBasedRadiant;
    }
}