using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;

public class HomepageButtons : MonoBehaviour {

    //Homepage Canvas Screens
    public Canvas patientHomepage;
    public Canvas doctorHomepage;
    public Canvas invitePatient;
    public Canvas inviteDoc;
    public Canvas checkInvites;
    public Canvas editInfo;

    //On start, only Homepage is visible
    private void Awake()
    {
        //IF STATEMENT #FIXME
        string json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\"}";
        string api = "/api/user/info";
        StartCoroutine(LoadHomeProfile(json, api));

        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;
        invitePatient.enabled = false;
        inviteDoc.enabled = false;
        checkInvites.enabled = false;
        editInfo.enabled = false;

    }

    private IEnumerator LoadHomeProfile(string json, string api)
    { 
        WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
        yield return www; 
        string[] info = www.text.Split(';');
        if (info[1] == "d"){
            doctorHomepage.enabled = true;
        }
        else if (info[1] == "p"){
            patientHomepage.enabled = true;
        }
    }

    //Used when clicking "Invite Patient/Doctor" from Homepage
    public void InviteOn()
    {
        //IF STATEMENT #FIXME
        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;
        invitePatient.enabled = true;
        inviteDoc.enabled = true;
        checkInvites.enabled = false;
        editInfo.enabled = false;
    }

    //Used when clicking "Check Invites" from Homepage
    public void CheckOn()
    {
        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;
        invitePatient.enabled = false;
        inviteDoc.enabled = false;
        checkInvites.enabled = true;
        editInfo.enabled = false;
    }

    //Used when clicking "Edit Info" from Homepage
    public void EditOn()
    {
        patientHomepage.enabled = false;
        doctorHomepage.enabled = false;
        invitePatient.enabled = false;
        inviteDoc.enabled = false;
        checkInvites.enabled = false;
        editInfo.enabled = true;
    }

    //Used when clicking "Go Back" from Invite/Check/Edit
    public void GoBackHome()
    {
        //IF STATEMENT #FIXME
        patientHomepage.enabled = true;
        doctorHomepage.enabled = true;
        invitePatient.enabled = false;
        inviteDoc.enabled = false;
        checkInvites.enabled = false;
        editInfo.enabled = false;
    }
}
