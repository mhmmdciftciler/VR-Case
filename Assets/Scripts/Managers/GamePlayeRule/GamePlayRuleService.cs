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
        /// <summary>
        /// Validates whether the given box types adhere to the defined rules.
        /// </summary>
        /// <param name="currentBoxType">The box type of the current item.</param>
        /// <param name="sideBoxType">The box type of the adjacent item.</param>
        /// <returns>
        /// <c>true</c> if the given box types are valid according to the rules; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method checks each rule to determine if there is a rule that invalidates the combination of the current box type and the side box type.
        /// - If any rule matches the given box types, the method returns <c>false</c>, indicating that the combination is not valid.
        /// - If no matching rule is found, the method returns <c>true</c>, indicating that the combination is valid.
        /// </remarks>
        public bool IsValidate(BoxType currentBoxType, BoxType sideBoxType)
        {
            foreach (Rule rule in Rules)
            {
                if (rule.CurrentBoxType == currentBoxType && rule.SideBoxType == sideBoxType)
                    return false;
            }
            return true;
        }

    }

}
