using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuFlow : MonoBehaviour
{
    [SerializeField] private GameObject ShopTab, PlayerTab, UpgradesTab;

    private bool isShopOpen, isPlayerOpen, isUpgradeOpen;

    private void Awake()
    {
        if (ShopTab != null && PlayerTab != null && UpgradesTab != null)
        {
            isShopOpen = false;
            isPlayerOpen = true;
            isUpgradeOpen = false;
        }
        else
        {
            Debug.LogError("Something is wrong with referencing");
            return;
        }
    }
    private void Start()
    {
        MainMenuButtons("Player");
    }
    public void MainMenuButtons(string uiPanel)
    {
        switch (uiPanel)
        {
            case "Player":
                isPlayerOpen = true;
                isShopOpen = false;
                isUpgradeOpen = false;
                break;
            case "Shop":
                isPlayerOpen = false;
                isShopOpen = true;
                isUpgradeOpen = false;
                break;
            case "Upgrades":
                isPlayerOpen = false;
                isShopOpen = false;
                isUpgradeOpen = true;
                break;

        }

        ShopTab.SetActive(isShopOpen);
        UpgradesTab.SetActive(isUpgradeOpen);
        PlayerTab.SetActive(isPlayerOpen); 
    }
}
