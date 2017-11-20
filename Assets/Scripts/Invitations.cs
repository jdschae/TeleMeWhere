using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;

public class Invitations : MonoBehaviour {

    public GameObject InviteToggleGroup;
    public GameObject ToggleTemplate;
    public Canvas inviteUser;
    public Vector3 FirstPosition;
    public float Spacing;

    private Vector3 CurrPosition;
    private ArrayList InviteLog;

	// Use this for initialization
	void Awake () {
        InviteLog = new ArrayList();
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
                Destroy((GameObject) InviteLog[i]);
            }
            InviteLog.Clear();

            string[] usernames = www.text.Split(';');
            for (int i = 0; i < usernames.Length - 1; ++i)
            {
                GameObject newToggle = Instantiate(ToggleTemplate, InviteToggleGroup.transform);
                newToggle.GetComponentInChildren<Text>().text = usernames[i];
                newToggle.transform.position = InviteToggleGroup.transform.TransformPoint(CurrPosition);

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
        int i;
        for (i = 1; i < InviteToggleGroup.transform.childCount; ++i)
        {
            if (InviteToggleGroup.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                break;
            }
        }

        NetworkUtility.InviteeUsername = InviteToggleGroup.transform.GetChild(i).GetChild(1).GetComponent<Text>().text;
        SceneManager.LoadScene(2);
    }

    void OnEnable () {
        Refresh();
	}

    public void Refresh()
    {
        string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\"}";
        string api = "/api/invite/view";

        StartCoroutine(ProcessGetInvitations(json, api));
    }

    public void SendInvite(GameObject inputField)
    {
        Text MessageField = inputField.transform.GetChild(2).GetComponent<Text>();
        string invitee = inputField.transform.GetChild(1).GetComponent<Text>().text;
        string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\", \"invitee\":\"" + invitee + "\"}";
        string api = "/api/invite/add";

        StartCoroutine(ProcessSendInvitation(MessageField, json, api));
        Transform panel = inviteUser.transform.GetChild(0);
        panel.GetChild(2).GetComponent<KeyboardInputField>().text = "";
    }

    private IEnumerator ProcessSendInvitation(Text MessageField, string json, string api)
    {
        WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
        yield return www;

        if (www.error == null)
        {
            MessageField.text = "Invitation Successful";
        }
        else
        {
            MessageField.text = "Invitation Unsuccessful";
        }
    }
}
