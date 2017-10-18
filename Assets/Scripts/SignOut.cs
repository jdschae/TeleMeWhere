using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour {

	public void SignOutAction()
    {	
    	string json = "";
    	string api = "/api/user/logout";
        WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
        StartCoroutine(ProcessSignOut(www));
    }

    private IEnumerator ProcessSignOut(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            print("error: " + www.error);
        }
    }

}
