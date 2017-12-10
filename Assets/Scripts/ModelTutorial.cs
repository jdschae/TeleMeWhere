using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ModelTutorial : MonoBehaviour {

    public Canvas selectAction;
    public Canvas tutorial;

	public void Awake() {
        selectAction.enabled = true;
        tutorial.enabled = false;
	}

    public void TutorialOn()
    {
        selectAction.enabled = false;
        tutorial.enabled = true;
    }

    public void GoBack()
    {
        selectAction.enabled = true;
        tutorial.enabled = false;
    }
	
}
