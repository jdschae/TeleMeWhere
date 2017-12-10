using System.Collections;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;

namespace HoloToolkit.Unity.InputModule
{
    public class MarkerPlace : MonoBehaviour, IFocusable, IInputHandler//, ISourceStateHandler
    {
        public Transform HostTransform;
        public GameObject[] MarkerTemplateArray;
        public bool IsPlacementEnabled = false;
        private ArrayList MarkerLog;
        private bool isGazed;
        public GameObject markerMenu;
        public GameObject markerInfo;

        // Variables for place marker and place marker button
        Vector3 gazeHitPosition;
        Vector3 gazeNormal;
        Vector3 markerPosition;
        Vector3 surfaceNormal;
        Quaternion markerRotation;

        public Toggle toggleRed;
        public Toggle toggleBlue;
        public Toggle toggleGreen;
        public Toggle toggleCube;
        public Toggle toggleCapsule;
        public Toggle toggleCylinder;

        public Material[] material;

        public string color = "";
        public string shape = "";
        public int shapeIndex;

        private IInputSource currentInputSource;
        private uint currentInputSourceId;
        private string Username;
        private string Sex;


        // Use this for initialization
        void Start()
        {
            if (HostTransform == null)
            {
                HostTransform = transform;
            }
            MarkerLog = new ArrayList();

            if (NetworkUtility.InviteeUsername == "")
            {
                Username = NetworkUtility.LoginUsername;
            }
            else
            {
                Username = NetworkUtility.InviteeUsername;
            }

            string json = "{\"username\":\"" + Username + "\"}";
            string api = "/api/model/view";

            NetworkUtility.Instance.sync_flag = true;

            markerMenu.SetActive(false);
            markerInfo.SetActive(false);

            StartCoroutine(ProcessAllMarkerRequest(json, api));
        }

        private IEnumerator ProcessAllMarkerRequest(string json, string api)
        {
            WWW www = NetworkUtility.Instance.SendPostRequest(json, "/api/user/info");
            yield return www;

            if (www.error == null)
            {
                string[] info = www.text.Split(';');
                Sex = info[0];
                if (Sex == "F")
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
                    string[] markers = www.text.Split(';');
                    for (int i = 0; i < markers.Length - 1; ++i)
                    {
                        string[] Components = markers[i].Split(',');

                        int colorIndex = int.Parse(Components[4]);
                        shapeIndex = int.Parse(Components[5]);
                        int objIndex = colorIndex * 4 + shapeIndex;

                        Quaternion rotation = new Quaternion(float.Parse(Components[7]), float.Parse(Components[8]), float.Parse(Components[9]), float.Parse(Components[6]));

                        Vector3 worldPosition = HostTransform.TransformPoint(new Vector3(float.Parse(Components[1]), float.Parse(Components[2]), float.Parse(Components[3])));
                        GameObject tempMarker = GameObject.Instantiate(MarkerTemplateArray[objIndex], worldPosition, rotation, HostTransform);
                        tempMarker.name += Components[0];
                        tempMarker.GetComponent<Renderer>().material = material[colorIndex];
                        MarkerLog.Add(tempMarker);
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
            GameObject hit = GazeManager.Instance.HitObject;
            if (hit.name == "M3DMale" || hit.name == "M3DFemale")
            {
                gazeHitPosition = GazeManager.Instance.HitInfo.point;

                gazeNormal = GazeManager.Instance.HitInfo.normal;

                markerPosition = HostTransform.InverseTransformPoint(gazeHitPosition);
                surfaceNormal = HostTransform.InverseTransformDirection(gazeNormal);

                markerRotation = Quaternion.FromToRotation(HostTransform.up, surfaceNormal);
                markerMenu.SetActive(true);

            }
            else if (hit.name.Contains("MarkerTemp(Clone)"))
            {
                markerInfo.SetActive(true);
                string json = "{\"username\":\"" + Username + "\",\"markerid\":\"" + hit.name.Split(')')[1] + "\"}";
                string api = "/api/marker/view";
                WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
                StartCoroutine(ProcessMarkerView(www, json, api));
            }
        }

        private IEnumerator ProcessMarkerView(WWW www, string json, string api)
        {
            yield return www;
            if (www.error == null)
            {
                //int shapeIndex = int.Parse(www.text.Split(';')[0]);
                //switch (shapeIndex)
                //{
                //    case 0:
                //        toggleSphere.isOn = true;
                //        toggleCube.isOn = false;
                //        toggleCapsule.isOn = false;
                //        toggleCylinder.isOn = false;
                //        break;
                //    case 1:
                //        toggleSphere.isOn = false;
                //        toggleCube.isOn = true;
                //        toggleCapsule.isOn = false;
                //        toggleCylinder.isOn = false;
                //        break;
                //    case 2:
                //        toggleSphere.isOn = false;
                //        toggleCube.isOn = false;
                //        toggleCapsule.isOn = true;
                //        toggleCylinder.isOn = false;
                //        break;
                //    case 3:
                //        toggleSphere.isOn = false;
                //        toggleCube.isOn = false;
                //        toggleCapsule.isOn = false;
                //        toggleCylinder.isOn = true;
                //        break;
                //    default:
                //        print("error: invalid shape index");
                //        break;

                //}
                markerInfo.transform.GetChild(0).GetChild(1).GetComponent<KeyboardInputField>().text = www.text.Split(';')[1];
            }
            else
            {
                print("error: " + www.error);
            }

        }

        public void PlaceMarkerButton()
        {
            if ((Sex == "F" && HostTransform.name == "M3DMale") || (Sex == "M" && HostTransform.name == "M3DFemale")) {
                return;
            }

            ActiveToggle();
            Transform panel = markerMenu.transform.GetChild(0);
            string message = panel.GetChild(3).GetComponent<KeyboardInputField>().text;

            string json = "{\"username\":\"" + Username + "\",\"x\":\"" + markerPosition.x + "\",\"y\":\"" + markerPosition.y +
                        "\",\"z\":\"" + markerPosition.z + "\",\"message\":\"" + message + "\",\"color\":\"" + color + "\",\"shape\":\"" + shape +
                            "\",\"rw\":\"" + markerRotation.w + "\",\"rx\":\"" + markerRotation.x + "\",\"ry\":\"" + markerRotation.y
                            + "\",\"rz\":\"" + markerRotation.z + "\"}";
            string api = "/api/marker/add";

            WWW www = NetworkUtility.Instance.SendPostRequest(json, api);

            StartCoroutine(ProcessMarkerPlacementRequest(www, gazeHitPosition, Quaternion.FromToRotation(Vector3.up, gazeNormal)));
        }

        public void CancelButton()
        {
            Transform panel = markerMenu.transform.GetChild(0);
            panel.GetChild(3).GetComponent<KeyboardInputField>().text = "";
            markerInfo.SetActive(false);
            markerMenu.SetActive(false);
        }

        public void CloseButton()
        {
            markerInfo.SetActive(false);
            markerMenu.SetActive(false);
        }

        public void ActiveToggle()
        {
            if (toggleRed.isOn)
            {
                color = "0";
            }
            else if (toggleBlue.isOn)
            {
                color = "1";
            }
            else if (toggleGreen.isOn)
            {
                color = "2";
            }
            else if (toggleOrange.isOn)
            {
                color = "3";
            }
            else if (togglePurple.isOn)
            {
                color = "4";
            }
            else if (toggleYellow.isOn)
            {
                color = "5";
            }
            if (toggleSphere.isOn)
            {
                shape = "0";
            }
            else if(toggleCube.isOn)
            {
                shape = "1";
            }
            else if (toggleCapsule.isOn)
            {
                shape = "2";
            }
            else if (toggleCylinder.isOn)
            {
                shape = "3";
            }
        }

        private IEnumerator ProcessMarkerPlacementRequest(WWW www, Vector3 spawnPosition, Quaternion spawnRotation)
        {
            yield return www;
            // check for errors
            if (www.error == null)
            {
                ActiveToggle();
                int colorIndex = int.Parse(color);
                shapeIndex = int.Parse(shape);
                int objIndex = colorIndex * 4 + shapeIndex;

                MarkerTemplateArray[shapeIndex].GetComponent<MeshRenderer>().sharedMaterial = material[colorIndex];
                GameObject tempMarker = GameObject.Instantiate(MarkerTemplateArray[objIndex], spawnPosition, spawnRotation, HostTransform);
                //tempMarker.GetComponent<Renderer>().materials.Length = material[colorIndex];
                //tempMarker.transform.localScale /= 100;
                tempMarker.name += www.text;
                MarkerLog.Add(tempMarker);
                markerMenu.SetActive(false);
                markerInfo.SetActive(false);
            }
            else
            {
                print("error: " + www.error);
            }
        }
    }
}