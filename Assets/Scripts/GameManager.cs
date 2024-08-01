using Fusion.XR.Shared.Grabbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCase
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] private List<Grabbable> _grabables;
        [SerializeField] private List<Pallet> _pallets;
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
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
