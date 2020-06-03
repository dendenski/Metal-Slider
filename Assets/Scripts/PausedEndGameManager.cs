using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PausedEndGameManager : MonoBehaviour
{
    public GameObject pausedGameOverPanel;
    private GameManager gameManager;
    private Transform resumeButton;
    private Transform displayText;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        resumeButton = pausedGameOverPanel.transform.Find("Resume Button");
        displayText = pausedGameOverPanel.transform.Find("Display Text");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(gameManager.gameState != GameState.gameOver && Time.timeScale != 0){
                resumeButton.gameObject.SetActive(true);
                displayText.gameObject.GetComponent<TextMeshProUGUI>().text = "PAUSED";
                Time.timeScale = 0;
                gameManager.tempState = gameManager.gameState;
                gameManager.gameState = GameState.paused;
                pausedGameOverPanel.SetActive(true);

            }
            else if(gameManager.gameState == GameState.paused){
                Resume();
            }
        }
    }
    public void Resume(){
        resumeButton.gameObject.SetActive(false);
        displayText.gameObject.GetComponent<TextMeshProUGUI>().text = "GAME OVER";
        Time.timeScale = 1;
        gameManager.gameState = gameManager.tempState;
        pausedGameOverPanel.SetActive(false);
    }
    public void Retry(){
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }
    public void GameOverPanel(){
        resumeButton.gameObject.SetActive(false);
        displayText.gameObject.GetComponent<TextMeshProUGUI>().text = "GAME OVER";
        pausedGameOverPanel.SetActive(true);
    }
    public void Quit(){
        Application.Quit();
    }
}
