using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;
using System.Net;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;

public class ClickButtons : MonoBehaviour
{

    //Three main Canvas Screens before logging in
    public Canvas mainCanvas;
    public Canvas signInCanvas;
    public Canvas createCanvas;

    //For Invalid Username/Password
    //public Text invalid;

    //Input fields on the Create Account and Sign in Screens
    public InputField firstname_inf;
    public InputField lastname_inf;
    public InputField username_inf;
    public InputField pass_inf;
    public InputField pass2_inf;
    public InputField email_inf;

    public static string ipAddress;

    //On start, only Main Menu is visible
    private void Awake()
    {
        mainCanvas.enabled = true;
        signInCanvas.enabled = false;
        createCanvas.enabled = false;
    }

    //Used when clicking "Sign In" from Main Menu
    public void SignInOn()
    {
        signInCanvas.enabled = true;
        mainCanvas.enabled = false;
        createCanvas.enabled = false;
        //invalid.enabled = false;
    }

    //Used when clicking "Create Account" from Main Menu
    public void CreateOn()
    {
        signInCanvas.enabled = false;
        mainCanvas.enabled = false;
        createCanvas.enabled = true;
    }

    //Used when clikcing "Main Menu" from either Sign In screen or Create Account
    public void GoBack()
    {
        createCanvas.enabled = false;
        signInCanvas.enabled = false;
        mainCanvas.enabled = true;
        //invalid.enabled = false;
    }

    private IEnumerator request(WWW www)
    {
        yield return www;
    }

    //Used when clicking "Sign In" or "Create Account" to start session
    public void ChangeScene()
    {
        string json = "";
        string api = "";
        if (createCanvas.enabled == true && signInCanvas.enabled == false){
            //json = "{\"firstname\":\"" + firstname_inf.text + "\",\"lastname\":\"" + lastname_inf.text + "\",\"username\":\"" + username_inf.text + "\",\"password\":\"" + pass_inf.text + "\",\"password2\":\"" + pass2_inf.text + "\",\"email\":\"" + email_inf.text + "\"}";
        }
        else if (signInCanvas.enabled == true && createCanvas.enabled == false) {
            KeyboardInputField user_inf = signInCanvas.transform.GetChild(0).GetChild(3).GetComponent<KeyboardInputField>();
            KeyboardInputField pswrd_inf = signInCanvas.transform.GetChild(0).GetChild(4).GetComponent<KeyboardInputField>();

            json = "{\"username\":\"" + user_inf.text + "\",\"password\":\"" + pswrd_inf.text + "\"}";
            api = "/api/user/login";

            WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
            StartCoroutine(ProcessLogIn(www));
        }
    }

    // Called when process response from LogIn request
    private IEnumerator ProcessLogIn(WWW www) 
    {
        yield return www;
        // check for errors
        if (www.error == null) {
            SceneManager.LoadScene(1);
        }
        else {
            print ("error: " + www.error);
        }
    }    

    
}
