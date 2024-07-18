using UnityEngine;
using UnityEngine.UI;

namespace UpgradeShop.ItemLevels
{
    public enum LevelState
    {
        NotUpgraded,
        Upgraded,
    }

    //Handles the UI and State of each upgrade level for an item
    public class ItemLevel : MonoBehaviour
    {
        private Image _myImage;
        [SerializeField]private LevelState itemLevelState;

        public LevelState ItemLevelState => itemLevelState;

        private void Awake()
        {
            _myImage = GetComponent<Image>();
        }

        public void DegradeSlot()
        {
            _myImage.color = Color.white;
            itemLevelState = LevelState.NotUpgraded;
        }

        public void UpgradeSlot()
        {
            _myImage.color = Color.black;
            itemLevelState = LevelState.Upgraded;
        }
    }
}