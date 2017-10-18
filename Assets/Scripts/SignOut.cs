using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour {

	public void SignOutAction()
    {	
    	string json = "";
    	string url = "http://35.1.109.92:3000/api/user/logout";
        WWW www = Click_Buttons.SendPostRequest(json, url);
        // parse databefore signout
        SceneManager.LoadScene(0);
    }

}
