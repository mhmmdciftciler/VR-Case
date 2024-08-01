using Fusion;
using Fusion.XR.Shared.Grabbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCase
{
    public class Box : MonoBehaviour
    {
        [SerializeField] private BoxType boxType;
        public BoxType BoxType => boxType;

        public bool IsGrabbing { get; private set; }
        public NetworkObject NetworkObject { get; private set; }
        private Grabbable _grabbable;
        private void Start()
        {
            _grabbable = GetComponent<Grabbable>();
            NetworkObject = GetComponent<NetworkObject>();
            _grabbable.onGrab.AddListener(OnGrab);
            _grabbable.onUngrab.AddListener(OnUnGrab);
        }

        private void OnUnGrab()
        {
            IsGrabbing = false;
        }

        private void OnGrab()
        {
            IsGrabbing = true;
        }
    }
    public enum BoxType
    {
        None,
        A,
        B,
        C
    }
}
