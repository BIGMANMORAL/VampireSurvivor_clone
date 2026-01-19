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
        GameOver
    }

    // Stores the current state of the game
    public GameStates CurrentState;
    // Stores the previous state of the game 
    public GameStates previousState;

    [Header("UI")]
    public GameObject pauseScreen;

    //current Stst display
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

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
                break;

            case GameStates.Paused:
                //code for the Paused state
                CheckForPauseAndResume(); 
                break;

            case GameStates.GameOver:
                // Code for the GameOver state 
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
    }
}
