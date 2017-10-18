using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour {

	public void SignOutAction()
    {	
    	string json = "";
    	string api = "/api/user/logout";
        WWW www = NetworkUtility.SendPostRequest(json, api);
        // parse databefore signout
        SceneManager.LoadScene(0);
    }

}
