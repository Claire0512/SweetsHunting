using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public static bool isRestarting = false; // Add this static variable
    //public static MenuManager Instance; // Singleton instance

    public GameObject menuPanel;
    public GameObject scorelifePanel;
    public GameObject gameOverPanel; // Add this GameOver Panel reference
    //public PlayerController player;

    public PlayerController player => FindObjectOfType<PlayerController>();

    // ... [other variables]

    private void Awake()
    {
        Instance = this;

        if (!isRestarting) // Only show the menuPanel if not restarting
            menuPanel.SetActive(true);
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        scorelifePanel.SetActive(true);
        player.controlEnabled = true;  // Ensure the player's control is enabled.
        player.transform.position = new Vector3(3.48f, -0.5396699f, 1);
        //Debug.Log($"StartGame() : {player.transform.position}");
        player.jumpState = PlayerController.JumpState.Grounded;
        player.isDying = false;
    }


    void Start()
    {
        if (!isRestarting)
        {
            menuPanel.SetActive(true);
            scorelifePanel.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            scorelifePanel.SetActive(true);
            player.controlEnabled = true;  // Ensure the player's control is enabled.
            player.transform.position = new Vector3(3.48f, -0.5396699f, 1);
            scorelifePanel.SetActive(true);
            player.isDying = false;
        }
        gameOverPanel.SetActive(false);
        player.transform.position = new Vector3(3.48f, -0.5396699f, 1);
        //Debug.Log($"Start() : {player.transform.position}");

    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        isRestarting = true; // Set the flag
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        StartGame();  // Ensure the game starts correctly after restarting.
    }


    public void ReturnToMenu()
    {
        isRestarting = false; // Reset the flag
        gameOverPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        menuPanel.SetActive(true);
        scorelifePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
