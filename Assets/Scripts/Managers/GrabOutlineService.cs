using Fusion.XR.Shared.Grabbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCase
{
    /// <summary>
    /// Manages the visibility of outlines for pallets based on the grabbing and ungrabbing actions of grabbable objects.
    /// </summary>
    /// <remarks>
    /// - When a `Grabbable` object is grabbed, all pallets' outlines are enabled.
    /// - When a `Grabbable` object is ungrabbed, all pallets' outlines are disabled.
    /// </remarks>
    public class GrabOutlineService : MonoBehaviour
    {
        [SerializeField] private List<Grabbable> _grabables;
        [SerializeField] private List<Pallet> _pallets;
        private void Start()
        {
            foreach (Grabbable grabbable in _grabables)
            {
                grabbable.onGrab.AddListener(OnGrab);
                grabbable.onUngrab.AddListener(OnUnGrab);
            }
        }

        private void OnUnGrab()
        {
            foreach (Pallet pallet in _pallets)
            {
                pallet.EnableOutline(false);
            }
        }

        private void OnGrab()
        {
            foreach (Pallet pallet in _pallets)
            {
                pallet.EnableOutline(true);
            }
        }
    }
}
