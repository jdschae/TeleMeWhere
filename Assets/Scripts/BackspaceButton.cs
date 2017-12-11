using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.UI.Keyboard;

public class BackspaceButton : MonoBehaviour {

    public GameObject input;

    public void Backspace()
    {
        string inputstring = input.GetComponent<KeyboardInputField>().text;
        if (inputstring != null && inputstring.Length > 0)
        {
            input.GetComponent<KeyboardInputField>().text = inputstring.Substring(0, inputstring.Length - 1);
        }
    }
    
}
