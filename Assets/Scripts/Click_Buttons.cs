using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;
using UnityEngine.SceneManagement;

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

    //Used when clicking "Sign In" or "Create Account" to start session
    public void ChangeScene(string sceneName)
    {
        var httpWebRequest = (HttpWebRequest)WebRequest.Create("localhost:3000/api/user/login");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null))
        {
            Input_Fields user_inf = createCanvas.transform.GetChild(0).GetChild(5).GetComponent<Input_Fields>();
            Input_Fields pass_inf = createCanvas.transform.GetChild(0).GetChild(4).GetComponent<Input_Fields>();
            string json = "{\"username\":\"" +user_inf.username + "," +
                          "\",\"password\":\"" + pass_inf.password +"\"}";

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Dispose();
        }


        HttpWebResponse httpResponse = await webrequest.GetResponseAsync();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();
        }

        SceneManager.LoadScene(sceneName);
    }

}
