using System.Collections;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

namespace HoloToolkit.Unity.InputModule
{
    public class MarkerPlace : MonoBehaviour, IFocusable, IInputHandler//, ISourceStateHandler
    {
        public Transform HostTransform;
        public GameObject MarkerTemplate;
        public bool IsPlacementEnabled = false;
        private ArrayList MarkerLog;
        private bool isGazed;

        public Toggle toggleRed;
        public Toggle toggleBlue;
        public Toggle toggleGreen;
        public Toggle toggleOrange;
        public Toggle togglePurple;
        public Toggle toggleYellow;
        public Toggle toggleSphere;
        public Toggle toggleCube;
        public Toggle toggleCapsule;
        public Toggle toggleCylinder;

        public string color = "";
        public string shape = "";

        private IInputSource currentInputSource;
        private uint currentInputSourceId;


        // Use this for initialization
        void Start()
        {
            if (HostTransform == null)
            {
                HostTransform = transform;
            }
            MarkerLog = new ArrayList();

            string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\"}";
            string api = "/api/model/view";

            NetworkUtility.Instance.sync_flag = true;



            StartCoroutine(ProcessAllMarkerRequest(json, api));
        }

        private IEnumerator ProcessAllMarkerRequest(string json, string api)
        {
            WWW www = NetworkUtility.Instance.SendPostRequest(json, "/api/user/info");
            yield return www;

            if (www.error == null)
            {
                string[] info = www.text.Split(';');
                if (info[0] == "F")
                {
                    if (HostTransform.name == "M3DMale")
                    {
                        HostTransform.gameObject.SetActive(false);
                        yield break;
                    }
                }
                else
                {
                    if (HostTransform.name == "M3DFemale")
                    {
                        HostTransform.gameObject.SetActive(false);
                        yield break;
                 
                    }
                }
            }
            else
            {
                print("error: " + www.error);
            }

            while (NetworkUtility.Instance.sync_flag) {
                www = NetworkUtility.Instance.SendPostRequest(json, api);
                yield return www;
                // check for errors
                if (www.error == null)
                {
                    for (int i = 0; i < MarkerLog.Count; ++i)
                    {
                        GameObject.Destroy((GameObject) MarkerLog[i]);
                    }
                    MarkerLog.Clear();
                    string[] Coordinates = www.text.Split(';');
                    for (int i = 0; i < Coordinates.Length - 1; ++i)
                    {
                        string[] Components = Coordinates[i].Split(',');
                        Vector3 worldPosition = HostTransform.TransformPoint(new Vector3(float.Parse(Components[1]), float.Parse(Components[2]), float.Parse(Components[3])));
                        MarkerLog.Add(GameObject.Instantiate(MarkerTemplate, worldPosition, Quaternion.identity, HostTransform));
                        ((GameObject) MarkerLog[MarkerLog.Count - 1]).name += Components[0];
                    }
                }
                else
                {
                    print("error: " + www.error);
                }
                yield return new WaitForSeconds(5);
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

        private void PlaceMarker()
        {
            if (!isGazed || !IsPlacementEnabled)
            {
                return;
            }

            Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;

            Vector3 markerPosition = HostTransform.InverseTransformPoint(gazeHitPosition);

            ActiveToggle();

            //color and shape need to be added
            string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\",\"x\":\"" + markerPosition.x + "\",\"y\":\"" + markerPosition.y + 
                        "\",\"z\":\"" + markerPosition.z + "\",\"message\":\"" + "message "+"\"}";
            string api = "/api/marker/add";

            WWW www = NetworkUtility.Instance.SendPostRequest(json, api);

            StartCoroutine(ProcessMarkerPlacementRequest(www, gazeHitPosition));
        }

        public void ActiveToggle()
        {
            if (toggleRed)
            {
                color = "red";
            }
            else if (toggleBlue)
            {
                color = "blue";
            }
            else if (toggleGreen)
            {
                color = "green";
            }
            else if (toggleOrange)
            {
                color = "Orange";
            }
            else if (togglePurple)
            {
                color = "purple";
            }
            else if (toggleYellow)
            {
                color = "yellow";
            }
            if (toggleSphere)
            {
                shape = "sphere";
            }
            else if(toggleCube)
            {
                shape = "cube";
            }
            else if (toggleCapsule)
            {
                shape = "capsule";
            }
            else if (toggleCylinder)
            {
                shape = "cylinder";
            }
        }

        private IEnumerator ProcessMarkerPlacementRequest(WWW www, Vector3 spawnPosition)
        {
            yield return www;
            // check for errors
            if (www.error == null)
            {
                MarkerLog.Add(GameObject.Instantiate(MarkerTemplate, spawnPosition, Quaternion.identity, HostTransform));
                ((GameObject) MarkerLog[MarkerLog.Count - 1]).name += www.text;
            }
            else
            {
                print("error: " + www.error);
            }
        }
    }
}