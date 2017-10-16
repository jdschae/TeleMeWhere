using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Input_Fields : MonoBehaviour
{

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
        username = _username_.text;
    }

    public void PassInput()
    {
        password = _password_.text;
    }

    public void Pass2Input()
    {
        password2 = _password2_.text;
    }

    public void EmailInput()
    {
        email = _email_.text;
    }
}
