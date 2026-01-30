using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    //Current Stats
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            //checks if the value of health has changed
            if(currentHealth != value)
            {
                currentHealth = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth; 
                }
                // Add any additional logic here that needs to be executed when the vale changes
            }
        }
    }
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            //checks if the value of health has changed
            if(currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
                // Add any additional logic here that needs to be executed when the vale changes
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //checks if the value of health has changed
            if(currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "MoveSpeed: " + currentMoveSpeed;
                }
                // Add any additional logic here that needs to be executed when the vale changes
            }
        }
    }
    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            //checks if the value of health has changed
            if(currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
                // Add any additional logic here that needs to be executed when the vale changes
            }
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            //checks if the value of health has changed
            if(currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "ProjectileSpeed: " + currentProjectileSpeed;
                }
                // Add any additional logic here that needs to be executed when the vale changes
            }
        }
    }
    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            //checks if the value of health has changed
            if(currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
                // Add any additional logic here that needs to be executed when the vale changes
            }
        }
    }
    #endregion

    //Exprience and level of the player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Class for defining a level range and the corresponding experince cap increase for that range
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject secondWeaponTest;
    public GameObject firstpassiveItemTest, secondpassiveItemTest;


    void Awake()
    {
        // Use the static instance to access GetData()
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        //Initialize current stats from character data
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        //spawn the starting weapon
        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstpassiveItemTest);
        SpawnPassiveItem(secondpassiveItemTest);
    }

    void Start()
    {
        //Initilize the experience cap as the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        //set the current stats display
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "MoveSpeed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "ProjectileSpeed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignChosenCharacterUI(characterData); 
    }

    void Update()
    {
        //Handle invincibility timer countdown
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;

        }
        // if the invincibility timer has reached 0, set the invicibility flag to false
        else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach(LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            GameManager.instance.StartLevelup();
        }
    }

    public void TakeDamage(float dmg)
    {
        //If the player is not currently invincible, reduce health and start invincibility timer
        if (!isInvincible)
        {
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }
        
    }

    public void Kill()
    {
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignChosenWeaponAndPassiveItemUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth(float amount)
    {
        //Only heal when the player if their current health is below max health
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;

            //makes sure the current health does not exceed max health
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if(CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += currentRecovery * Time.deltaTime;

            //makes sure the current health does not exceed max health
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            } 
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        // Checking if the slots are full, and returning if it is
        if(weaponIndex >= inventory.weaponSlots.Count - 1) //Must be -1 because list starts from 0
        {
            Debug.LogError("Inventory slots are alreadyfull");
            return;
        }

        //spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform); //set the weapon to be a child of the player
        inventory.Addweapons(weaponIndex, spawnedWeapon.GetComponent<WeaponController>()); // Add the weapon to it's inventory slot

        weaponIndex++;
    }
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        // Checking if the slots are full, and returning if it is
        if(passiveItemIndex >= inventory.passiveItemsSlots.Count - 1) //Must be -1 because list starts from 0
        {
            Debug.LogError("Inventory slots are alreadyfull");
            return;
        }

        //spawn the starting passiveItem
        GameObject spawnedpassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedpassiveItem.transform.SetParent(transform); //set the passiveitem to be a child of the player
        inventory.AddPassiveItem(passiveItemIndex, spawnedpassiveItem.GetComponent<PassiveItem>()); // Add the passiveItem to it's inventory slot

        passiveItemIndex++;
    }
}
