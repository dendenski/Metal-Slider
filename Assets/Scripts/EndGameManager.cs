using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class EndGameManager : MonoBehaviour
{
    public GameManager gameManager;
    public bool isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        isGameOver = true;
    }
    public IEnumerator GameOver(){
        yield return new WaitForSeconds(7f);
        FindObjectOfType<PausedEndGameManager>().GameOverPanel();
    }


}
