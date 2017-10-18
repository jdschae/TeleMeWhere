﻿using System.Text;
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

public class Click_Buttons : MonoBehaviour
{

    //Three main Canvas Screens before logging in
    public Canvas mainCanvas;
    public Canvas signInCanvas;
    public Canvas createCanvas;

    //For Invalid Username/Password
    public Text invalid;

    //Input fields on the Create Account and Sign in Screens
    public InputField firstname_inf;
    public InputField lastname_inf;
    public InputField username_inf;
    public InputField pass_inf;
    public InputField pass2_inf;
    public InputField email_inf;

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
        invalid.enabled = false;
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
        invalid.enabled = false;
    }

    private IEnumerator request(WWW www)
    {
        yield return www;
    }

    //Used when clicking "Sign In" or "Create Account" to start session
    public void ChangeScene()
    {
        string json = "";
        string url = "";
        if (createCanvas.enabled == true && signInCanvas.enabled == false){
            //json = firstname_inf.text + lastname_inf.text + username_inf.text + pass_inf.text + pass2_inf.text + email_inf.text;
        }
        else if (signInCanvas.enabled == true && createCanvas.enabled == false) {
            //Input_Fields user_inf = signInCanvas.transform.GetChild(0).GetChild(1).GetComponent<Input_Fields>();
            //Input_Fields pass_inf = signInCanvas.transform.GetChild(0).GetChild(2).GetComponent<Input_Fields>();
            
            //json = "{\"username\":\"" + username_inf.text + "\",\"password\":\"" + pass_inf.text + "\"}";

            json = "{\"username\":\"" + "frank" + "\",\"password\":\"" + "eecs498" +"\"}";
            url = "http://35.1.109.14:3000/api/user/login";


            WWW www = SendPostRequest(json, url);
            StartCoroutine(ProcessLogIn(www));
        }
    }

    // Called when process response from LogIn request
    private IEnumerator ProcessLogIn(WWW www) 
    {
        yield return www;
        // check for errors
        if (www.error == null) {
            invalid.enabled = false;
            print (www.text);
        } else {
            invalid.enabled = true;
            print ("error: " + www.error);
        }
        SceneManager.LoadScene(1);
    }    

    // Util method for sending post request
    public static WWW SendPostRequest(string json, string url)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] pData = Encoding.ASCII.GetBytes(json.ToCharArray());

        return new WWW(url, pData, headers);
    }
}
