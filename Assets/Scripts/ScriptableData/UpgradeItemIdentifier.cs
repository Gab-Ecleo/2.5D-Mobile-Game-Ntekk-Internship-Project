using UnityEngine;

namespace ScriptableData
{
    // A scriptable object that provides an ID for the upgradable items
    [CreateAssetMenu(fileName = "Upgrade Item Identifier", menuName = "Upgrade Shop/Upgrade Item Identifier", order = 0)]
    public class UpgradeItemIdentifier : ScriptableObject
    {
        public string identifier;
    }
}