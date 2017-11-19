using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invitations : MonoBehaviour {

    public GameObject InviteToggleGroup;
    public GameObject ToggleTemplate;
    public Vector3 FirstPosition;
    public float Spacing;

    private Vector3 CurrPosition;
    private ArrayList InviteLog;

	// Use this for initialization
	void Start () {
        Refresh();
    }

    private IEnumerator ProcessGetInvitations(string json, string api)
    {
        WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
        yield return www;

        if (www.error == null)
        {
            CurrPosition = FirstPosition;
            for (int i = 0; i < InviteLog.Count; ++i)
            {
                GameObject.Destroy((GameObject) InviteLog[i]);
            }
            InviteLog.Clear();

            string[] usernames = www.text.Split(';');
            for (int i = 0; i < usernames.Length - 1; ++i)
            {
                GameObject newToggle = Instantiate(ToggleTemplate, InviteToggleGroup.transform);
                newToggle.GetComponentInChildren<UnityEngine.UI.Text>().text = usernames[i];
                newToggle.transform.position = CurrPosition;

                CurrPosition += new Vector3(0, -Spacing, 0);

                InviteLog.Add(newToggle);
            }

        }
        else
        {
            print("error: " + www.error);
        }


    }

    //TODO
    public void AcceptInvite()
    {

    }

    void OnEnable () {
        Refresh();
	}

    public void Refresh()
    {
        string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\"}";
        string api = "/api/getinvitations";

        StartCoroutine(ProcessGetInvitations(json, api));
    }

    public void SendInvite(GameObject inputField)
    {
        string username = inputField.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text;

        //TODO: Implement api call
    }
}
