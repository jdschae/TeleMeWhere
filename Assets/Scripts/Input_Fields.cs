using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Input_Fields : MonoBehaviour
{
    //Keyboard Functionality
    UnityEngine.TouchScreenKeyboard keyboard;
    public static string keyboardText = "";

    //Input fields on the Create Account and Sign in Screens
    [SerializeField] public InputField _username_;
    [SerializeField] public InputField _password_;
    [SerializeField] public InputField _password2_;
    [SerializeField] public InputField _email_;
   
    public string username;
    public string password;
    public string password2;
    public string email;

    public void UserInput()
    {
        KeyLoop();
        //username = _username_.text;
        username = keyboardText;
    }

    public void PassInput()
    {
        KeyLoop();
        //password = _password_.text;
        password = keyboardText;
    }

    public void Pass2Input()
    {
        KeyLoop();
        //password2 = _password2_.text;
        password2 = keyboardText;
    }

    public void EmailInput()
    {
        KeyLoop();
        //email = _email_.text;
        email = keyboardText;
    }

    public void KeyLoop()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        if (TouchScreenKeyboard.visible == false && keyboard != null)
        {
            if (keyboard.done == true)
            {
                keyboardText = keyboard.text;
                keyboard = null;
            }
        }
    }
}
