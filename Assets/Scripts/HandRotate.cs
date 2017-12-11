using System;
using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{
    public class HandRotate : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler
    {

        /// <summary>
        /// Event triggered when dragging starts.
        /// </summary>
        public event Action StartedRotating;

        /// <summary>
        /// Event triggered when dragging stops.
        /// </summary>
        public event Action StoppedRotating;

        [Tooltip("Transform that will be rotated. Defaults to the object of the component.")]
        public Transform HostTransform;

        [Tooltip("Controls the speed at which the object will interpolate toward the desired rotation")]
        //[Range(0.01f, 20.0f)]
        public float RotationLerpSpeed = 50.0f;

        [Tooltip("Scale by which hand movement in z is multiplied to move the dragged object.")]
        public float DistanceScale = 2f;

        public bool IsRotatingEnabled = false;

        private bool isRotating;
        private bool isGazed;
        private Vector3 objRefForward;
        private Vector3 objRefUp;
        private float objRefDistance;
        private Quaternion gazeAngularOffset;
        private float handRefDistance;
        private Vector3 objRefGrabPoint;
        private Vector3 initialHandRefPosition;

        private Vector3 rotatingPosition;
        private Quaternion rsotation;

        private IInputSource currentInputSource;
        private uint currentInputSourceId;

        // Use this for initialization
        private void Start()
        {
            if (HostTransform == null)
            {
                HostTransform = transform;
            }
        }

        private void OnDestroy()
        {
            if (isRotating)
            {
                StopRotating();
            }

            if (isGazed)
            {
                OnFocusExit();
            }
        }

        private void Update()
        {
            if (IsRotatingEnabled && isRotating)
            {
                UpdateRotating();
            }
        }

        /// <summary>
        /// Starts dragging the object.
        /// </summary>
        /// TODO
        public void StartRotating()
        {
            if (!IsRotatingEnabled)
            {
                return;
            }

            if (isRotating)
            {
                return;
            }

            // Add self as a modal input handler, to get all inputs during the manipulation
            InputManager.Instance.PushModalInputHandler(gameObject);

            isRotating = true;

            Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;
            Transform cameraTransform = CameraCache.Main.transform;
            Vector3 handPosition;
            currentInputSource.TryGetPosition(currentInputSourceId, out handPosition);

            Vector3 pivotPosition = GetHandPivotPosition(cameraTransform);
            initialHandRefPosition = handPosition - pivotPosition;
            handRefDistance = Vector3.Magnitude(handPosition - pivotPosition);
            objRefDistance = Vector3.Magnitude(gazeHitPosition - pivotPosition);

            Quaternion objRotation = HostTransform.localRotation;
            // Store where the object was grabbed from
            objRefGrabPoint = cameraTransform.InverseTransformDirection(HostTransform.position - gazeHitPosition);

            Vector3 objDirection = Vector3.Normalize(gazeHitPosition - pivotPosition);
            Vector3 handDirection = Vector3.Normalize(handPosition - pivotPosition);
            
            //objforward = cameratransform.inversetransformdirection(objforward);       // in camera space
            //objup = cameratransform.inversetransformdirection(objup);                 // in camera space
            objDirection = cameraTransform.InverseTransformDirection(objDirection);   // in camera space
            handDirection = cameraTransform.InverseTransformDirection(handDirection); // in camera space

            //objRefForward = objForward;
            //objRefUp = objUp;

            // Store the initial offset between the hand and the object, so that we can consider it when dragging
            gazeAngularOffset = Quaternion.FromToRotation(handDirection, objDirection);
            rotatingPosition = gazeHitPosition;

            StartedRotating.RaiseEvent();
        }

        /// <summary>
        /// Gets the pivot position for the hand, which is approximated to the base of the neck.
        /// </summary>
        /// <returns>Pivot position for the hand.</returns>
        private Vector3 GetHandPivotPosition(Transform cameraTransform)
        {
            Vector3 pivot = cameraTransform.position + new Vector3(0, -0.2f, 0) - cameraTransform.forward * 0.2f; // a bit lower and behind
            return pivot;
        }

        /// <summary>
        /// Enables or disables dragging.
        /// </summary>
        /// <param name="isEnabled">Indicates whether dragging should be enabled or disabled.</param>
        public void SetRotating(bool isEnabled)
        {
            if (IsRotatingEnabled == isEnabled)
            {
                return;
            }

            IsRotatingEnabled = isEnabled;

            if (isRotating)
            {
                StopRotating();
            }
        }

        /// <summary>
        /// Update the position of the object being dragged.
        /// </summary>
        /// TODO
        private void UpdateRotating()
        {
            Vector3 newHandPosition;
            Transform cameraTransform = CameraCache.Main.transform;
            currentInputSource.TryGetPosition(currentInputSourceId, out newHandPosition);

            Vector3 pivotPosition = GetHandPivotPosition(cameraTransform);

            Vector3 newHandDirection = Vector3.Normalize(newHandPosition - pivotPosition);

            newHandDirection = cameraTransform.InverseTransformDirection(newHandDirection); // in camera space
            Vector3 targetDirection = Vector3.Normalize(gazeAngularOffset * newHandDirection);
            targetDirection = cameraTransform.TransformDirection(targetDirection); // back to world space

            float currentHandDistance = Vector3.Magnitude(newHandPosition - pivotPosition);

            float distanceRatio = currentHandDistance / handRefDistance;
            float distanceOffset = distanceRatio > 0 ? (distanceRatio - 1f) * DistanceScale : 0;
            float targetDistance = objRefDistance + distanceOffset;

            rotatingPosition = pivotPosition + (targetDirection * targetDistance);

            Vector3 handOffset = newHandPosition - pivotPosition - Vector3.Project(newHandPosition - pivotPosition, initialHandRefPosition);

            //if (rotationmode == rotationmodeenum.orienttowarduser || rotationmode == rotationmodeenum.orienttowarduserandkeepupright)
            //{
            //    rotatingrotation = quaternion.lookrotation(hosttransform.position - pivotposition);
            //}
            //else if (rotationmode == rotationmodeenum.lockobjectrotation)
            //{
            //    rotatingrotation = hosttransform.rotation;
            //}
            //else // rotationmodeenum.default
            //{
            //    vector3 objforward = cameratransform.transformdirection(objrefforward); // in world space
            //    vector3 objup = cameratransform.transformdirection(objrefup);   // in world space
            //    rotatingrotation = quaternion.lookrotation(objforward, objup);
            //}

            // Apply Final Position
            //HostTransform.position = Vector3.Lerp(HostTransform.position, rotatingPosition + cameraTransform.TransformDirection(objRefGrabPoint), PositionLerpSpeed);
            // Apply Final Rotation
            //transform.Rotate(new Vector3(0, -RotationLerpSpeed * handOffset.x, 0));
            if (Math.Abs(handOffset.x) > Math.Abs(handOffset.y))
            {
                transform.RotateAround(transform.TransformPoint(new Vector3(0, 1.15f, 0)), Vector3.up, -RotationLerpSpeed * handOffset.x);
            }
            else
            {
                transform.RotateAround(transform.TransformPoint(new Vector3(0, 1.15f, 0)), Vector3.Cross(Vector3.up, targetDirection), RotationLerpSpeed * handOffset.y);
            }
            //if (RotationMode == RotationModeEnum.OrientTowardUserAndKeepUpright)
            //{
            //    Quaternion upRotation = Quaternion.FromToRotation(HostTransform.up, Vector3.up);
            //    HostTransform.rotation = upRotation * HostTransform.rotation;
            //}
        }

        /// <summary>
        /// Stops dragging the object.
        /// </summary>
        public void StopRotating()
        {
            if (!isRotating)
            {
                return;
            }

            // Remove self as a modal input handler
            InputManager.Instance.PopModalInputHandler();

            isRotating = false;
            currentInputSource = null;
            StoppedRotating.RaiseEvent();
        }

        public void OnFocusEnter()
        {
            if (!IsRotatingEnabled)
            {
                return;
            }

            if (isGazed)
            {
                return;
            }

            isGazed = true;
        }

        public void OnFocusExit()
        {
            if (!IsRotatingEnabled)
            {
                return;
            }

            if (!isGazed)
            {
                return;
            }

            isGazed = false;
        }

        public void OnInputUp(InputEventData eventData)
        {
            if (currentInputSource != null &&
                eventData.SourceId == currentInputSourceId)
            {
                StopRotating();
            }
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (isRotating)
            {
                // We're already handling drag input, so we can't start a new drag operation.
                return;
            }

            if (!eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
            {
                // The input source must provide positional data for this script to be usable
                return;
            }

            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
            StartRotating();
        }

        public void OnSourceDetected(SourceStateEventData eventData)
        {
            // Nothing to do
        }

        public void OnSourceLost(SourceStateEventData eventData)
        {
            if (currentInputSource != null && eventData.SourceId == currentInputSourceId)
            {
                StopRotating();
            }
        }
    }
}