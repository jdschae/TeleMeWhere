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

public class Click_Buttons : MonoBehaviour
{

    //Three main Canvas Screens before logging in
    public Canvas mainCanvas;
    public Canvas signInCanvas;
    public Canvas createCanvas;

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
    }

    private IEnumerator request(WWW www)
    {
        yield return www;
    }

    //Used when clicking "Sign In" or "Create Account" to start session
    public void ChangeScene()
    {
        if (createCanvas.enabled == true && signInCanvas.enabled == false){
            
        }
        else if (signInCanvas.enabled == true && createCanvas.enabled == false) {
            Input_Fields user_inf = signInCanvas.transform.GetChild(0).GetChild(1).GetComponent<Input_Fields>();
            Input_Fields pass_inf = signInCanvas.transform.GetChild(0).GetChild(2).GetComponent<Input_Fields>();
            
            string json = "{\"username\":\"" + user_inf.username + "\",\"password\":\"" + pass_inf.password +"\"}";
            string url = "http://35.1.168.14:3000/api/user/login";

            WWW www = SendPostRequest(json,url)
            StartCoroutine(ProcessLogIn(www));
        }
    }

    // Called when process response from LogIn request
    private IEnumerator ProcessLogIn(WWW www) 
    {
        yield return www;
        // check for errors
        if (www.error == null) {
            print (www.text);
        } else {
            print ("error: " + www.error);
        }
        SceneManager.LoadScene(1);
    }    

    // Util method for sending post request
    public WWW SendPostRequest(string json, string url)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] pData = Encoding.ASCII.GetBytes(json.ToCharArray());

        WWW www = new WWW(url, pData, headers);
    }
}
