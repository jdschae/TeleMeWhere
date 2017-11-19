using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;


public class EditInfo : MonoBehaviour {

    // Canvases used in this
    public Canvas editInfoCanvas;
    public Canvas passmatch;

    //On start, passmatch is not visible
    private void Awake()
    {
        editInfoCanvas.enabled = false;
        passmatch.enabled = false;
    }

    // When the "Save Changes" button is pressed
    public void SaveChanges() {
        string json = "";
        string api = "";
        Transform panel = editInfoCanvas.transform.GetChild(0);
        string firstname = panel.GetChild(3).GetComponent<KeyboardInputField>().text;
        string lastname = panel.GetChild(4).GetComponent<KeyboardInputField>().text;
        string password1 = panel.GetChild(5).GetComponent<KeyboardInputField>().text;
        string password2 = panel.GetChild(6).GetComponent<KeyboardInputField>().text;
        string email = panel.GetChild(7).GetComponent<KeyboardInputField>().text;
        if (password1 != password2)
        {
            passmatch.enabled = true;
            return;
        }

        json = "{\"username\":\"" + NetworkUtility.LoginUsername + "\",\"password\":\"" + password1 + "\",\"firstname\":\""
                + firstname + "\",\"lastname\":\"" + lastname + "\",\"email\":\"" + email + "\"}";
        api = "/api/user/edit";

        // Save the new json string for the user.
        WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
        //StartCoroutine(ProcessSaveChangesRequest(www));
    }

    /*// Called when process response from Save Changes request
    private IEnumerator ProcessSaveChangesRequest(WWW www)
    {
        yield return www;
        // save the 
    }*/

}
