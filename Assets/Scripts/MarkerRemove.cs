using System;
using UnityEngine;

namespace HoloToolkit.Unity.InputModule
{
    public class MarkerRemove : MonoBehaviour, IFocusable, IInputHandler
    {

        public Transform HostTransform;
        public bool IsPlacementEnabled = false;
        private bool isGazed;

        private IInputSource currentInputSource;
        private uint currentInputSourceId;
        public void OnFocusEnter()
        {
            throw new NotImplementedException();
        }

        public void OnFocusExit()
        {
            throw new NotImplementedException();
        }

        public void OnInputDown(InputEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnInputUp(InputEventData eventData)
        {
            throw new NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {
            if (HostTransform == null)
            {
                HostTransform = transform;
            }
        }

        // Update is called once per frame
        
    }
}