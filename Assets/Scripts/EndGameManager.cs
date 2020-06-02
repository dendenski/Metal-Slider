using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class EndGameManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameManager gameManager;
    public Camera gameCamera;

    public bool isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        isGameOver = true;
    }
    public IEnumerator GameOver(){
        //gameManager.blocksInScene.ForEach(c => c.GetComponent<SpriteRenderer>().color 
        //            = new Color(.5f, .5f, .5f));
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("GameScene");
    }


}
