using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ActionEnabler : MonoBehaviour {


    public void EnableMarkerPlacement(GameObject Model)
    {
        Model.GetComponent<MarkerPlace>().IsPlacementEnabled = true;
        Model.GetComponent<HandDraggable>().IsDraggingEnabled = false;
        Model.GetComponent<HandRotate>().IsRotatingEnabled = false;
    }

    public void EnableDragging(GameObject Model)
    {
        Model.GetComponent<MarkerPlace>().IsPlacementEnabled = false;
        Model.GetComponent<HandDraggable>().IsDraggingEnabled = true;
        Model.GetComponent<HandRotate>().IsRotatingEnabled = false;
    }

    public void EnableRotating(GameObject Model)
    {
        Model.GetComponent<MarkerPlace>().IsPlacementEnabled = false;
        Model.GetComponent<HandDraggable>().IsDraggingEnabled = false;
        Model.GetComponent<HandRotate>().IsRotatingEnabled = true;
    }
}
