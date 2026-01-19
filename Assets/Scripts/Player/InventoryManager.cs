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

    public void Addweapons(int slotIndex, WeaponController weapon) // Add a weapon to a specific spot
    {
        weaponSlots[slotIndex] = weapon;
        weaponsLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;     //Enable the component when it is being used
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveitem) // Add a passive item to a specific spot
    {
        passiveItemsSlots[slotIndex] = passiveitem;
        passiveItemsLevels[slotIndex] = passiveitem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;     // Enables the component when it is being used
        passiveItemUISlots[slotIndex].sprite = passiveitem.passiveItemData.Icon;
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
        }
    }
}
