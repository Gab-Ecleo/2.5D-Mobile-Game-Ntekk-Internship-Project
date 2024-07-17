using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Upgrade Item Identifier", menuName = "Upgrade Shop/Upgrade Item Identifier", order = 0)]
    public class UpgradeItemIdentifier : ScriptableObject
    {
        public string identifier;
    }
}