using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour {

	public void SignOutAction()
    {
        SceneManager.LoadScene(0);
    }

}
