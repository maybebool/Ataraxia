using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Exercises.ExerciseOne {
    public class GazeRaycastIndicator : MonoBehaviour {

        
        [SerializeField] private XRGazeInteractor gazeInteractor;
        [SerializeField] private GameObject objectToSpawn;

        private Vector3 _rayEndPoint;
        private Vector3 _objectPosition;
        private Quaternion _objectRotation;
        private void Start() {
            _rayEndPoint = gazeInteractor.rayEndPoint; 
            _objectPosition = _rayEndPoint;
            _objectRotation = Quaternion.identity;
            Instantiate(objectToSpawn, _objectPosition, _objectRotation);
        }

        private void Update() {
            _rayEndPoint = gazeInteractor.rayEndPoint;
            objectToSpawn.transform.position = _rayEndPoint;
        }
    }
}