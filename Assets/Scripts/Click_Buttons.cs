using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;
using System.Net;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using Windows.Foundation;

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
        Input_Fields user_inf = signInCanvas.transform.GetChild(0).GetChild(3).GetComponent<Input_Fields>();
        Input_Fields pass_inf = signInCanvas.transform.GetChild(0).GetChild(4).GetComponent<Input_Fields>();
        /*
        ASCIIEncoding encoding = new ASCIIEncoding();
        string json = "{\"username\":\"Frank\", \"password\":\"Fapmaster\"}";

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        byte[] pData = Encoding.ASCII.GetBytes(json.ToCharArray());

        WWW www = new WWW("35.1.168.14:3000/api/user/login", pData, headers);

        StartCoroutine(request(www));
        */
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("35.1.168.14:3000/api/model/view/1");
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //Stream resStream = response.GetResponseStream();
        //StreamReader reader = new StreamReader(resStream);
        //string fin = reader.ReadToEnd();
        IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
        async (workItem) =>
        {

            string json = "{\"username\":\"Frank\", \"password\":\"Fapmaster\"}";
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json.ToCharArray());

            string url = "http://35.1.168.14:3000/api/user/login";
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.Headers["Content-Type"] = "application/json";

            Stream stream = await webRequest.GetRequestStreamAsync();
            stream.Write(jsonBytes, 0, jsonBytes.Length);

            WebResponse response = await webRequest.GetResponseAsync();
        }
        );

        //asyncAction.Completed = new AsyncActionCompletedHandler(PostDataAsyncCompleted);

        SceneManager.LoadScene(1);
    }

}
