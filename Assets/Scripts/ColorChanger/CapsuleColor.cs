using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleColor : MonoBehaviour
{

    public Material[] material;

    public Toggle toggleRed;
    public Toggle toggleBlue;
    public Toggle toggleGreen;
    public Toggle toggleOrange;
    public Toggle togglePurple;
    public Toggle toggleYellow;

    public Renderer rend;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();//Gives functionality for the renderer
        rend.enabled = true;//Makes the rendered 3d object visable if enabled;
        rend.sharedMaterial = material[0];
    }

    public void ActiveToggle()
    {
        if (toggleRed.isOn)
        {
            rend.sharedMaterial = material[0];
        }
        else if (toggleBlue.isOn)
        {
            rend.sharedMaterial = material[1];
        }
        else if (toggleGreen.isOn)
        {
            rend.sharedMaterial = material[2];
        }
        else if (toggleOrange.isOn)
        {
            rend.sharedMaterial = material[3];
        }
        else if (togglePurple.isOn)
        {
            rend.sharedMaterial = material[4];
        }
        else if (toggleYellow.isOn)
        {
            rend.sharedMaterial = material[5];
        }
    }

}