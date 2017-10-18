using System;
using UnityEngine;


namespace HoloToolkit.Unity.InputModule
{
    public class MarkerPlace : MonoBehaviour, IFocusable, IInputHandler//, ISourceStateHandler
    {
        public Transform HostTransform;
        public GameObject MarkerTemplate;
        public bool IsPlacementEnabled = false;
        private bool isGazed;

        private IInputSource currentInputSource;
        private uint currentInputSourceId;


        // Use this for initialization
        void Start()
        {
            if (HostTransform == null)
            {
                HostTransform = transform;
            }
        }

        

        public void OnFocusEnter()
        {
            if (!IsPlacementEnabled)
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
            if (!IsPlacementEnabled)
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
            //if (currentInputSource != null &&
            //    eventData.SourceId == currentInputSourceId)
            //{
            //    PlaceMarker();
            //}
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
            PlaceMarker();
        }

        //public void onsourcedetected(sourcestateeventdata eventdata)
        //{
        //    // nothing to do
        //}

        //public void onsourcelost(sourcestateeventdata eventdata)
        //{
        //    if (currentinputsource != null && eventdata.sourceid == currentinputsourceid)
        //    {
        //        stopdragging();
        //    }
        //}

        private void PlaceMarker()
        {
            if (!isGazed || !IsPlacementEnabled)
            {
                return;
            }

            Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;

            Vector3 markerPosition = HostTransform.InverseTransformPoint(gazeHitPosition);

            string json = "{\"x\":\"" + markerPosition.x + "\",\"y\":\"" + markerPosition.y + 
                        "\",\"z\":\"" + markerPosition.z + "\",\"message\":\"" + "message "+"\"}";
            string api = "/api/marker/add";

            WWW www = NetworkUtility.SendPostRequest(json, api);
            // TODO: Parse response 
            GameObject.Instantiate(MarkerTemplate, gazeHitPosition, Quaternion.identity, HostTransform);

            //In model space
            
        }
    }
}