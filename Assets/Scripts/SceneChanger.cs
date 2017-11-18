using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public void OpenHomepage()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenModel()
    {
        SceneManager.LoadScene(2);
    }

}
