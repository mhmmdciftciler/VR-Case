using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCase
{
    public class GamePlayRuleService : MonoBehaviour
    {
        public static GamePlayRuleService Instance;
        [field:SerializeField] public List<Rule> Rules { get; private set; }
        private void Start()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public bool IsValidate(BoxType currentBoxType, BoxType sideBoxType)
        {
            foreach (Rule rule in Rules)
            {
                if(rule.CurrentBoxType == currentBoxType && rule.SideBoxType == sideBoxType)
                    return false;
            }
            return true;
        }
    }
    [System.Serializable]
    public struct Rule
    {
        public BoxType CurrentBoxType;
        public BoxType SideBoxType;
    }
}
