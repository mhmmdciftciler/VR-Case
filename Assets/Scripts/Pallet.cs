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

        public UnityEvent<Pallet, BoxType> OnPalletBoxTypeChangedEvent;

        private BoxType _oldBoxType;
        public override void Render()
        {
            base.Render();
            if(Object != null)
            {
                if (_oldBoxType != CurrentBoxType)
                {
                    _oldBoxType = CurrentBoxType;
                    OnPalletBoxTypeChangedEvent?.Invoke(this, CurrentBoxType);
                }
            }

        }
        private void OnTriggerEnter(Collider other)
        {
            if (Runner.IsSharedModeMasterClient &&
                CurrentBoxType == BoxType.None &&
                other.TryGetComponent<Box>(out Box box))
            {

                CurrentBoxType = box.BoxType;
                CurrentBoxId = box.NetworkObject.Id.Raw;
                OnPalletBoxTypeChangedEvent?.Invoke(this, box.BoxType);

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (Runner.IsSharedModeMasterClient &&
                other.TryGetComponent<Box>(out Box box) &&
                CurrentBoxId == box.NetworkObject.Id.Raw)
            {
                CurrentBoxType = BoxType.None;
                OnPalletBoxTypeChangedEvent?.Invoke(this, CurrentBoxType);

                if (box.IsGrabbing)
                {
                    EnableOutline(true);
                }
            }
        }
        public bool IsPalletRuleValidate()
        {
            foreach (Pallet pallet in NeighbourPallets)
            {
                if (!GamePlayRuleService.Instance.IsValidate(CurrentBoxType, pallet.CurrentBoxType))
                    return false;
            }
            return true;
        }

        public void EnableOutline(bool enabled)
        {
            if (enabled && CurrentBoxType != BoxType.None)// This pallet is already using
                return;

            _outline.enabled = enabled;

        }
    }
}
