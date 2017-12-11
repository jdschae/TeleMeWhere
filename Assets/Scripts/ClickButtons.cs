using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public class ClickButtons : MonoBehaviour
{

    //Four main Canvas Screens before logging in
    public Canvas mainCanvas;
    public Canvas signInCanvas;
    public Canvas createCanvas;
    public Canvas tutorialCanvas;

    //For Invalid Username/Password/Toggle/Email
    public Canvas invalid;
    public Canvas passmatch;

    public static string ipAddress;

    //Male/Female Doc/Pat toggle buttons
    public Toggle isMale;
    public Toggle isFemale;
    public Toggle isPat;
    public Toggle isDoc;
    public string sex;
    public string type;

    //On start, only Main Menu is visible
    private void Awake()
    {
        mainCanvas.enabled = true;
        signInCanvas.enabled = false;
        createCanvas.enabled = false;
        invalid.enabled = false;
        passmatch.enabled = false;
        tutorialCanvas.enabled = false;
    }

    //Used when clicking "Sign In" from Main Menu
    public void SignInOn()
    {
        signInCanvas.enabled = true;
        mainCanvas.enabled = false;
        createCanvas.enabled = false;
        invalid.enabled = false;
        tutorialCanvas.enabled = false;
    }

    //Used when clicking "Create Account" from Main Menu
    public void CreateOn()
    {
        signInCanvas.enabled = false;
        mainCanvas.enabled = false;
        createCanvas.enabled = true;
        passmatch.enabled = false;
        tutorialCanvas.enabled = false;
    }

    //Used when clicking "Tutorial" from Main Menu
    public void TutorialOn()
    {
        signInCanvas.enabled = false;
        mainCanvas.enabled = false;
        createCanvas.enabled = false;
        passmatch.enabled = false;
        tutorialCanvas.enabled = true;
    }

    public void BackspaceUser()
    {
        Transform panel = signInCanvas.transform.GetChild(0);
        string username = panel.GetChild(3).GetComponent<KeyboardInputField>().text;
        panel.GetChild(3).GetComponent<KeyboardInputField>().text = username.Substring(0, username.Length - 1);
    }

    //Regex code snippet
    /*
    public static void emas(string text)
    {
        const string MatchEmailPattern =
       @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
       + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
         + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
        Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // Find matches.
        MatchCollection matches = rx.Matches(text);
        // Report the number of matches found.
        int noOfMatches = matches.Count;
        // Report on each match.
        foreach (Match match in matches)
        {
            Console.WriteLine(match.Value.ToString());
        }
    }*/

    //Used when clikcing "Main Menu" from either Sign In screen or Create Account
    public void GoBack()
    {
        if (createCanvas.enabled)
        {
            Transform panel = createCanvas.transform.GetChild(0);
            for (int i = 3; i < 8; ++i)
            {
                panel.GetChild(i).GetComponent<KeyboardInputField>().text = "";
            }
        }
        else if (signInCanvas.enabled)
        {
            Transform panel = signInCanvas.transform.GetChild(0);
            string username = panel.GetChild(3).GetComponent<KeyboardInputField>().text = "";
            string password = panel.GetChild(4).GetComponent<KeyboardInputField>().text = "";
        }
        createCanvas.enabled = false;
        signInCanvas.enabled = false;
        mainCanvas.enabled = true;
        invalid.enabled = false;
        passmatch.enabled = false;
        tutorialCanvas.enabled = false;
    }

    public void ActiveToggle()
    {
        if (isMale.isOn)
        {
            sex = "M";
        }
        else if (isFemale.isOn)
        {
            sex = "F";
        }
        if (isDoc.isOn)
        {
            type = "D";
        }
        else if (isPat.isOn)
        {
            type = "P";
        }
    }

    private IEnumerator request(WWW www)
    {
        yield return www;
    }

    //Used when clicking "Sign In" or "Create Account" to start session
    public void ChangeScene()
    {
        string json = "";
        string api = "";
        if (createCanvas.enabled == true && signInCanvas.enabled == false){
            ActiveToggle();
            Transform panel = createCanvas.transform.GetChild(0);
            string firstname = panel.GetChild(3).GetComponent<KeyboardInputField>().text;
            string lastname = panel.GetChild(4).GetComponent<KeyboardInputField>().text;
            string username = panel.GetChild(5).GetComponent<KeyboardInputField>().text;
            string password1 = panel.GetChild(6).GetComponent<KeyboardInputField>().text;
            string password2 = panel.GetChild(7).GetComponent<KeyboardInputField>().text;
            string email = panel.GetChild(8).GetComponent<KeyboardInputField>().text;
            if (password1 != password2) {
                passmatch.enabled = true;
                return;
            }
            json = "{\"username\":\"" + username + "\",\"password\":\"" + password1 + "\",\"firstname\":\"" 
                    + firstname + "\",\"lastname\":\""  + lastname + "\",\"email\":\"" + email + "\",\"sex\":\""
                    + sex + "\",\"type\":\"" + type + "\"}";
            api = "/api/user/create";
        }
        else if (signInCanvas.enabled == true && createCanvas.enabled == false) {
            Transform panel = signInCanvas.transform.GetChild(0);
            string username = panel.GetChild(3).GetComponent<KeyboardInputField>().text;
            string password = panel.GetChild(4).GetComponent<KeyboardInputField>().text;

            json = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
            api = "/api/user/login";
        }
        WWW www = NetworkUtility.Instance.SendPostRequest(json, api);
        StartCoroutine(ProcessMainMenuRequest(www));
    }

    // Called when process response from LogIn request
    private IEnumerator ProcessMainMenuRequest(WWW www) 
    {
        yield return www;
        // check for errors
        if (www.error == null) {
            //succesful log in or account creation
            NetworkUtility.LoginUsername = www.text.Split('\"')[3];
            invalid.enabled = false;
            SceneManager.LoadScene(1);
        }
        else {
            print ("error: " + www.error);
            invalid.enabled = true;
        }
    }

}
