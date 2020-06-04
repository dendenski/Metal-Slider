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
    public Button pauseButtton;
    public Button soundButtton;
    public Sprite soundIcon;
    public Sprite soundIconX;

    public int soundState;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        resumeButton = pausedGameOverPanel.transform.Find("Resume Button");
        displayText = pausedGameOverPanel.transform.Find("Display Text");
        soundState = PlayerPrefs.GetInt ("soundState");
        if(soundState == 0){
            gameManager.GetComponent<AudioSource>().volume = 0f;
            soundButtton.GetComponent<Image>().sprite = soundIconX;
            for(int i = 0; i < gameManager.verticalPos.Length; i++){
                gameManager.verticalPos[i].GetComponent<AudioSource>().volume = 0f;
            }
            
        }else{
            gameManager.GetComponent<AudioSource>().volume = 0.5f;
            soundButtton.GetComponent<Image>().sprite = soundIcon;
            for(int i = 0; i < gameManager.verticalPos.Length; i++){
                gameManager.verticalPos[i].GetComponent<AudioSource>().volume = 0.5f;
            }
        }
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
                soundButtton.interactable = false;
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
        pauseButtton.interactable = true;
        soundButtton.interactable = true;
    }

    public void Paused(){
        if(gameManager.gameState != GameState.gameOver && Time.timeScale != 0){
                resumeButton.gameObject.SetActive(true);
                displayText.gameObject.GetComponent<TextMeshProUGUI>().text = "PAUSED";
                Time.timeScale = 0;
                gameManager.tempState = gameManager.gameState;
                gameManager.gameState = GameState.paused;
                pausedGameOverPanel.SetActive(true);
                pauseButtton.interactable = false;
                soundButtton.interactable = false;
            }
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

    
    public void SoundButton(){
        if(soundState != 0){
            gameManager.GetComponent<AudioSource>().volume = 0f;
            soundButtton.GetComponent<Image>().sprite = soundIconX;
            for(int i = 0; i < gameManager.verticalPos.Length; i++){
                gameManager.verticalPos[i].GetComponent<AudioSource>().volume = 0f;
            }
            soundState = 0;
            PlayerPrefs.SetInt ("soundState", soundState);
            PlayerPrefs.Save();
        }else{
            gameManager.GetComponent<AudioSource>().volume = 0.5f;
            soundButtton.GetComponent<Image>().sprite = soundIcon;
            for(int i = 0; i < gameManager.verticalPos.Length; i++){
                gameManager.verticalPos[i].GetComponent<AudioSource>().volume = 0.5f;
            }
            soundState = 1;
            PlayerPrefs.SetInt ("soundState", soundState);
            PlayerPrefs.Save();
        }

    }
}
