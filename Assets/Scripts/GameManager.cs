using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Define the different states of the game
    public enum GameStates
    {
        Gameplay,
        Paused,
        GameOver,
        Levelup
    }

    // Stores the current state of the game
    public GameStates CurrentState;
    // Stores the previous state of the game 
    public GameStates previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject levelUpScreen;

    //current Stst display
    [Header("Current Stats Displays")]
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Result Screen Display")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public Text timesurvivedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    [Header("StopWatch")]
    public float timeLimit; // The time limit in seconds
    float stopwatchTime;// The current time elasped since the stopwatch started
    public Text stopwatchDisplay;

    // Flag to check if the game is over
    public bool isGameOver = false;

    // Flag to check if the player is choosing their upgrades
    public bool choosingUpgrade;

    //Refference to the player object   
    public GameObject playerObject;
    void Awake()
    {
        //waring check to see if there is another singleton of this kind in this game
        if(instance == null)
        {
            instance = this; 
        }
        else
        {
            Debug.LogWarning("EXTRA" + this + "DELETED");
        }

        DisableScreen();    
    }

    void Update()
    {
        // Defining the Behaviour of each state
        switch (CurrentState)
        {
            case GameStates.Gameplay:
                // Code for the gameplay state
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameStates.Paused:
                //code for the Paused state
                CheckForPauseAndResume(); 
                break;

            case GameStates.GameOver:
                // Code for the GameOver state
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; // Stops the game entirely
                    Debug.Log("Game Over!");
                    DisplayResults();
                }
                break;

            case GameStates.Levelup:
                // Code for the Levelup state
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; // Pauses the game while choosing upgrades
                    Debug.Log("Choose your upgrades!");
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    // A method to change the GameState instead of using the assignment operator
    public void ChangeState(GameStates newState)
    {
        CurrentState = newState;
    }

    public void PauseGame()
    {
        if(CurrentState != GameStates.Paused)
        {
            previousState = CurrentState;
            ChangeState(GameStates.Paused);
            Time.timeScale = 0f; // Stops the time in game (Pausing the Game)
            pauseScreen.SetActive(true);
            Debug.Log("Game Paused");
        }

    }

    public void ResumeGame()
    {
        if(CurrentState == GameStates.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Resume the game
            pauseScreen.SetActive(false);  
            Debug.Log("Game resumed"); 
        }
    }

    // Method to check for pause and resume input 
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(CurrentState == GameStates.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timesurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameStates.GameOver);
        resultScreen.SetActive(false);
    }

    void DisplayResults()
    {
        resultScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.Name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString(); 
    }

    public void AssignChosenWeaponAndPassiveItemUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemData)
    {
        if (chosenWeaponData.Count != chosenWeaponUI.Count || chosenPassiveItemData.Count != chosenPassiveItemUI.Count)
        {
            Debug.Log("Chosen weapons and passive items data do not match the UI slots");
            return;
        }

        // Assign chosen weapons data to chosenWeaponUI
        for (int i = 0; i < chosenWeaponUI.Count; i++)
        {
            //check that the sprite of the corresponding element in chosenWeaponData is not null
            if (chosenWeaponData[i].sprite)
            {
                //Enable the corresponding element in chosenWeaponsUI and set its sprite to the corresponding sprite in chosenWeaponData
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                // If the sprite is nukll, disable the corresponding element in chosenWeaponUI
                chosenWeaponUI[i].enabled = false;
            }

        }
        // Assign chosen weapons data to chosenPassiveItemUI
        for (int i = 0; i < chosenPassiveItemUI.Count; i++)
        {
            //check that the sprite of the corresponding element in chosenPassiveItemData is not null
            if (chosenWeaponData[i].sprite)
            {
                //Enable the corresponding element in chosenWPassiveItemUI and set its sprite to the corresponding sprite in chosenPassiveItemData
                chosenPassiveItemUI[i].enabled = true;
                chosenPassiveItemUI[i].sprite = chosenPassiveItemData[i].sprite;
            }
            else
            {
                // If the sprite is nukll, disable the corresponding element in chosenPassiveItemUI
                chosenPassiveItemUI[i].enabled = false;
            }

        }
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpadateStopwatchDisplay();

        if (stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpadateStopwatchDisplay()
    {
        // Claculate the number of minuties and seconds that have elaspsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60f);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60f);

        // Update the stopwatch text to display the elaspsed time
        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelup()
    {
        ChangeState(GameStates.Levelup);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelup()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f; // Resume the game
        levelUpScreen.SetActive(false);
        ChangeState(GameStates.Gameplay);
    }
}
