using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;

public class HomepageButtons : MonoBehaviour {

    //Homepage Canvas Screens
    public Canvas patientHomepage;
    public Canvas doctorHomepage;
    public Canvas invite;
    public Canvas checkInvites;
    public Canvas editInfo;
    public bool patient;
    public bool doctor;

    //On start, only Homepage is visible
    private void Awake()
    {
        patient = false;
        doctor = false;
        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;

        invite.enabled = false;
        checkInvites.enabled = false;
        editInfo.enabled = false;

        string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\"}";
        string api = "/api/user/info";
        StartCoroutine(LoadHomeProfile(json, api));
    }

    private IEnumerator LoadHomeProfile(string json, string api)
    { 
        WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
        yield return www; 
        string[] info = www.text.Split(';');
        if (info[1] == "D")
        {
            doctor = true;
            doctorHomepage.enabled = true;
        }
        else if (info[1] == "P")
        {
            patient = true;
            patientHomepage.enabled = true;
        }
    }

    //Used when clicking "Invite Patient/Doctor" from Homepage
    public void InviteOn()
    {
        invite.enabled = true;
        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;
        checkInvites.enabled = false;
        editInfo.enabled = false;
    }

    //Used when clicking "Check Invites" from Homepage
    public void CheckOn()
    {
        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;
        invite.enabled = false;
        checkInvites.enabled = true;
        editInfo.enabled = false;
    }

    //Used when clicking "Edit Info" from Homepage
    public void EditOn()
    {
        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;
        invite.enabled = false;
        checkInvites.enabled = false;
        editInfo.enabled = true;
    }

    //Used when clicking "Go Back" from Invite/Check/Edit
    public void GoBackHome()
    {
        if (editInfo.enabled)
        {
            Transform panel = editInfo.transform.GetChild(0);
            for (int i = 3; i < 7; ++i)
            {
                panel.GetChild(i).GetComponent<KeyboardInputField>().text = "";
            }
        }
        if (invite.enabled)
        {
            Transform panel = editInfo.transform.GetChild(0);
            panel.GetChild(2).GetComponent<KeyboardInputField>().text = "";
        }
        if (doctor)
        {
            doctorHomepage.enabled = true;
        }
        else if (patient)
        {
            patientHomepage.enabled = true;
        }
        invite.enabled = false;
        checkInvites.enabled = false;
        editInfo.enabled = false;
    }
}
