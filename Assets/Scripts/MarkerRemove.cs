using System;
using System.Collections;
using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{
    public class MarkerRemove : MonoBehaviour, IFocusable, IInputHandler
    {

        public Transform HostTransform;
        public bool IsRemoveEnabled = false;
        private bool isGazed;

        private IInputSource currentInputSource;
        private uint currentInputSourceId;
        public void OnFocusEnter()
        {
            if (!IsRemoveEnabled)
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
            if (!IsRemoveEnabled)
            {
                return;
            }

            if (!isGazed)
            {
                return;
            }

            isGazed = false;
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (!eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
            {
                // The input source must provide positional data for this script to be usable
                return;
            }

            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
            RemoveMarker();
        }

        public void OnInputUp(InputEventData eventData)
        {
            
        }

        // Use this for initialization
        void Start()
        {
            if (HostTransform == null)
            {
                HostTransform = transform;
            }
        }

        private void RemoveMarker()
        {
            if (!isGazed || !IsRemoveEnabled)
            {
                return;
            }

            GameObject hit = GazeManager.Instance.HitObject;

            if (hit == null || hit.name != "Marker(Clone)")
            {
                return;
            }

            Vector3 markerPosition = hit.transform.position;

            string json = "{\"x\":\"" + markerPosition.x + "\",\"y\":\"" + markerPosition.y + 
                        "\",\"z\":\"" + markerPosition.z + "\"}";
            string api = "/api/marker/delete";

            WWW www = NetworkUtility.Instance.SendPostRequest(json, api);

            StartCoroutine(ProcessMarkerRemoveRequest(www, hit));

        }

        private IEnumerator ProcessMarkerRemoveRequest(WWW www, GameObject victim)
        {
            yield return www;
            // check for errors
            if (www.error == null)
            {
                Destroy(victim);
            }
            else
            {
                print("error: " + www.error);
            }
        }

    }

    
}