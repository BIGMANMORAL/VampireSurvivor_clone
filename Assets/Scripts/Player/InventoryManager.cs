using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponsLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemsSlots = new List<PassiveItem>(6);
    public int[] passiveItemsLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);

    [System.Serializable]
    public class WeaponUpgrade
    {
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>(); // List of ugrade options for weapons
    public List<PassiveItemUpgrade> PassiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // List of upgrade options for passive items
    public List<UpgradeUI> UpgradeUIOptions = new List<UpgradeUI>();   // list of ui for upgrade options present in the scene

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void Addweapons(int slotIndex, WeaponController weapon) // Add a weapon to a specific spot
    {
        weaponSlots[slotIndex] = weapon;
        weaponsLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;     //Enable the component when it is being used
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelup();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveitem) // Add a passive item to a specific spot
    {
        passiveItemsSlots[slotIndex] = passiveitem;
        passiveItemsLevels[slotIndex] = passiveitem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;     // Enables the component when it is being used
        passiveItemUISlots[slotIndex].sprite = passiveitem.passiveItemData.Icon;

        if(GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelup();
        }
    }

    public void LevelupWeapon(int slotIndex)
    {
        if(weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab) // Checks if there is a next level for the current weapon
            {
                Debug.LogError("NO NEXT LEVEL FOR" + weapon.name);
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform); // set the weapon to be a child of the player 
            Addweapons(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponsLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level; // To make sure we have the correct weapon level

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelup();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex)
    {
        if(passiveItemsSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemsSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab) // Checks if there is a next level for the current passive Item
            {
                Debug.LogError("NO NEXT LEVEL FOR" + passiveItem.name);
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform); // set the weapon to be a child of the player 
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemsLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level; // To make sure we have the correct passiveItem level

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelup();
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        foreach(var upgradeOption in UpgradeUIOptions)
        {
            int upgradeType = Random.Range(1, 3);  // Choose between weapon and passive items
            if(upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[Random.Range(0, weaponUpgradeOptions.Count)];

                bool newWeapon = false;
                if (chosenWeaponUpgrade != null)
                {
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelupWeapon(i)); // apply button functionality
                                //Set the description and name to be that of the next level 
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon)); // apply button functionality
                        //Apply initial descriiption and name
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if(upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = PassiveItemUpgradeOptions[Random.Range(0, PassiveItemUpgradeOptions.Count)];

                if(chosenPassiveItemUpgrade != null)
                {
                    bool newPassiveItem = false;
                    for(int i = 0; i < passiveItemsSlots.Count; i++)
                    {
                        if(passiveItemsSlots[i] != null && passiveItemsSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false;

                            if (!newPassiveItem)
                            {
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i)); // apply button functionality
                                //Set the description and name to be that of the next level
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem)); // apply button functionality
                        //Apply initial descriiption and name
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach(var upgradeOption in UpgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
        }
    }

    public void RemoveAndApplyUpgrade()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }
}