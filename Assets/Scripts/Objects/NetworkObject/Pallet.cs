using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRCase
{
    public class Pallet : NetworkBehaviour
    {
        [Networked, HideInInspector] public BoxType CurrentBoxType { get; private set; }
        [Networked, HideInInspector] public uint CurrentBoxId { get; private set; }
        [field: SerializeField] public List<Pallet> NeighbourPallets { get; private set; }

        [SerializeField] private Outline _outline;

        private BoxType _oldBoxType;

        public UnityEvent<Pallet, BoxType> OnPalletBoxTypeChangedEvent;

        public void Update()
        {
            if (Object != null)
            {
                if (_oldBoxType != CurrentBoxType)
                {
                    _oldBoxType = CurrentBoxType;
                    OnPalletBoxTypeChangedEvent?.Invoke(this, CurrentBoxType);//On changed Current Box Type this event must call Invoke. 
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (CurrentBoxType == BoxType.None && // The box can enter to the pallet while current box has none.
                other.TryGetComponent<Box>(out Box box))
            {

                CurrentBoxType = box.BoxType;
                CurrentBoxId = box.NetworkObject.Id.Raw;
                OnPalletBoxTypeChangedEvent?.Invoke(this, box.BoxType);

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Box>(out Box box) &&
                CurrentBoxId == box.NetworkObject.Id.Raw)// if the exit box is current box can exit.
            {
                CurrentBoxType = BoxType.None;
                OnPalletBoxTypeChangedEvent?.Invoke(this, CurrentBoxType);

                if (box.IsGrabbing)
                {
                    EnableOutline(true);
                }
            }
        }

        /// <summary>
        /// Validates the current pallet's box type against the box types of its neighboring pallets according to game play rules.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current box type is valid with respect to all neighboring pallets' box types; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPalletRuleValidate()
        {
            foreach (Pallet pallet in NeighbourPallets)
            {
                if (!GamePlayRuleService.Instance.IsValidate(CurrentBoxType, pallet.CurrentBoxType))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Enables or disables the outline for this pallet based on the specified flag.
        /// </summary>
        /// <param name="enabled">A boolean flag indicating whether the outline should be enabled or disabled.</param>
        public void EnableOutline(bool enabled)
        {
            if (enabled && CurrentBoxType != BoxType.None)// This pallet is already using
                return;

            _outline.enabled = enabled;

        }
    }
}
