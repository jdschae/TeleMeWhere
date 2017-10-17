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
        user_inf = createCanvas.transform.GetChild(0).GetChild(3).GetComponent<Input_Fields>();
        pass_inf = createCanvas.transform.GetChild(0).GetChild(4).GetComponent<Input_Fields>();

        ASCIIEncoding encoding = new ASCIIEncoding();
        string json = "{\"username\":\"" + user_inf.username + "," +
                          "\",\"password\":\"" + pass_inf.password +"\"}";
        byte[] data = encoding.GetBytes(json);

        WebRequest request = WebRequest.Create("35.1.168.14:3000/api/user/login");
        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        Stream stream = request.GetRequestStream();
        stream.Write(data, 0, data.Length);
        stream.Close();

        WebResponse response = request.GetResponse();
        stream = response.GetResponseStream();

        StreamReader sr99 = new StreamReader(stream);
        MessageBox.Show(sr99.ReadToEnd());

        sr99.Close();
        stream.Close();

        SceneManager.LoadScene(sceneName);
    }

}
