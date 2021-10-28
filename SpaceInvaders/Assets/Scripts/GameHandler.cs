using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [Header("Pause System")]
    //We create our variables for the pause menu and the bool to evaluate whether game is paused or not
    public GameObject pauseMenu;
    public bool isPaused;

    [Header("Timer System")]
    //We create our variables for global and round time as well as level count and round limit time
    public float globalTime = 0.0f;
    public float roundTime = 0.0f;
    public float roundTimeLimit = 240.0f;
    public int levelCount = 0;
    //We create a bool to track whether the round is active at any given time.
    public bool roundActive;
    //We create a timer to track time between rounds and a time limit between rounds
    public float nonRoundTime = 0.0f;
    public float nonRoundTimeLimit = 30.0f;

    private void Start()
    {
        //We ensure round one begins as soon as the game starts
        RoundStart();
    }


    private void FixedUpdate()
    {
        //We set globalTime equal to Time.fixedDeltaTime to track how long the player plays and survives
        globalTime += Time.fixedDeltaTime;

        //We check to see if a round is currently active. If it is, we ascribe roundtime to Time.fixedDeltaTime
        if (roundActive)
        {
            roundTime += Time.fixedDeltaTime;
        } else
        {
            //Here we ascribe nonRoundTime to Time.fixedDeltaTime to track the time between rounds and start the next when the limit is passed
            nonRoundTime += Time.fixedDeltaTime;
        }

        //We check to see if the roundTime exceeds the roundTimeLimit. If it does, we call the RoundEnd function
        if (roundTime >= roundTimeLimit)
        {
            RoundEnd();
        }

        //If nonRoundTime exceeds nonRoundTimeLimit the next round starts
        if (nonRoundTime >= nonRoundTimeLimit)
        {
            nonRoundTime = 0.0f;
            RoundStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause();
        }
    }

    //We create a function to evaluate whether the game is paused and determine the action thereof
    public void GamePause()
    {
        if (isPaused == false)
        {
            //We change isPaused to true, set pauseMenu to active and change our timescale to 0
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            //We change isPaused to false, deactivate our pause menu and return the timescale to 1
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    //We create a function to send us back to the Main Menu and set the game time back to 1
    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1f;
    }

    //We create a function to exit the game
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

    //We create a function to Restart the state of our game and set the game time back to 1
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }

    public void RoundStart()
    {
        //We increase the level count, ascribe roundTime to Time.fixedDeltaTime and turn our bool roundActive true
        levelCount++;
        roundTime += Time.fixedDeltaTime;
        roundActive = true;
    }

    public void RoundEnd()
    {
        //We reset our roundTime to 0.0f and set roundActive to false
        roundTime = 0.0f;
        roundActive = false;
    }
}
