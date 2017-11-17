using System.Collections;
using UnityEngine;
using System.Threading;

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

            string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\"}";
            string api = "/api/model/view";

            NetworkUtility.Instance.sync_flag = true;

            while (NetworkUtility.Instance.sync_flag) {
                WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
                StartCoroutine(ProcessAllMarkerRequest(www));
            }
        }

        private IEnumerator ProcessAllMarkerRequest(WWW www)
        {
            yield return www;
            // check for errors
            if (www.error == null)
            {
                string[] Coordinates = www.text.Split(';');
                for (int i = 0; i < Coordinates.Length - 1; ++i)
                {
                    string[] Components = Coordinates[i].Split(',');
                    Vector3 worldPosition = HostTransform.TransformPoint(new Vector3(float.Parse(Components[1]), float.Parse(Components[2]), float.Parse(Components[3])));
                    GameObject marker = GameObject.Instantiate(MarkerTemplate, worldPosition, Quaternion.identity, HostTransform);
                    marker.name += Components[0];
                }
            }
            else
            {
                print("error: " + www.error);
            }
            yield return new WaitForSeconds(5);
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

        private void PlaceMarker()
        {
            if (!isGazed || !IsPlacementEnabled)
            {
                return;
            }

            Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;

            Vector3 markerPosition = HostTransform.InverseTransformPoint(gazeHitPosition);

            string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\",\"x\":\"" + markerPosition.x + "\",\"y\":\"" + markerPosition.y + 
                        "\",\"z\":\"" + markerPosition.z + "\",\"message\":\"" + "message "+"\"}";
            string api = "/api/marker/add";

            WWW www = NetworkUtility.Instance.SendPostRequest(json, api);

            StartCoroutine(ProcessMarkerPlacementRequest(www, gazeHitPosition));
        }

        private IEnumerator ProcessMarkerPlacementRequest(WWW www, Vector3 spawnPosition)
        {
            yield return www;
            // check for errors
            if (www.error == null)
            {
                GameObject marker = GameObject.Instantiate(MarkerTemplate, spawnPosition, Quaternion.identity, HostTransform);
                marker.name += www.text;
            }
            else
            {
                print("error: " + www.error);
            }
        }
    }
}