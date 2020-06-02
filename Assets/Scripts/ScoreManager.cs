using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{

    public int score;
    
    public int hiScore;
    public int finalscore;

    public GameManager gameManager;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI hiScoreTMP;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        gameManager = FindObjectOfType<GameManager>();
        hiScore = PlayerPrefs.GetInt ("hiScore");
        HiScoreHandler(hiScore);
    }

    // Update is called once per frame
    void Update()
    {
        ScoreHandler();
        if(score >= hiScore){
            HiScoreHandler(score);
        }
    }


    public void getScore(){

        score += (gameManager.totalScore*gameManager.numberOfBlocksDestroyed);
        if(gameManager.totalScore*gameManager.numberOfBlocksDestroyed > 0){
            scoreTMP.GetComponent<CameraShake>().shakeDuration = .7f;
            if(score > hiScore){
                hiScoreTMP.GetComponent<CameraShake>().shakeDuration = .7f;
            }
        }
    }

    private void ScoreHandler(){
        if(score < 10){
            scoreTMP.text = "00000" + score;
        }
        else if(score < 100){
            scoreTMP.text = "0000" + score;
        }
        else if(score < 1000){
            scoreTMP.text = "000" + score;
        }
        else if(score < 10000){
            scoreTMP.text = "00" + score;
        }
        else if(score < 100000){
            scoreTMP.text = "0" + score;
        }else{
            scoreTMP.text = "" + score;
        }
    }
        private void HiScoreHandler(int currentScore){
        if(currentScore < 10){
            hiScoreTMP.text = "00000" + currentScore;
        }
        else if(currentScore < 100){
            hiScoreTMP.text = "0000" + currentScore;
        }
        else if(currentScore < 1000){
            hiScoreTMP.text = "000" + currentScore;
        }
        else if(currentScore < 10000){
            hiScoreTMP.text = "00" + currentScore;
        }
        else if(currentScore < 100000){
            hiScoreTMP.text = "0" + currentScore;
        }else{
            hiScoreTMP.text = "" + currentScore;
        }
    }

    public void SetHighScore(){
        if(score > hiScore){
            PlayerPrefs.SetInt ("hiScore", score);
            PlayerPrefs.Save();
        }
    }
}
