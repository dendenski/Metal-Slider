using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowChecker : MonoBehaviour
{
    public int blockDetected;
    public GameManager gameManager;
    public Camera gameCamera;
    public EndGameManager endGameManager;
    public List<GameObject> others;
    // Start is called before the first frame update
    void Start()
    {
        
        endGameManager = FindObjectOfType<EndGameManager>();
        gameCamera = FindObjectOfType<Camera>();
        gameManager = FindObjectOfType<GameManager>();
        blockDetected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        SetMass(other);
        if(other.gameObject.tag == "BlockSize1"){
            blockDetected += 1;
            others.Add(other.gameObject);
        }
        if(other.gameObject.tag == "BlockSize2"){
            blockDetected += 2;
            others.Add(other.gameObject);
        }
        if(other.gameObject.tag == "BlockSize3"){
            blockDetected += 3;
            others.Add(other.gameObject);
        }
        if(other.gameObject.tag == "BlockSize4"){
            blockDetected += 4;
            others.Add(other.gameObject);
        }
        if(blockDetected == 8 && gameManager.gameState != GameState.gameOver){
            gameManager.blocksDestroyed = true;
            StartCoroutine(DestroyBlockCo());
        }else if(gameManager.gameState == GameState.gameOver){
            StartCoroutine(BlockChangeColorGameOverCo());
            StartCoroutine(DestroyBlockGameOverCo());
        }
    }
    private IEnumerator DestroyBlockCo(){
        int score = 0;
        for(int i = 0;i < others.Count;i++){
            Animator animator;
            animator = others[i].GetComponent<Animator>();
            score += (int)Mathf.Pow(2,others[i].GetComponent<BlockMovement>().blockSize); 
            if(animator == null){
                continue;
            }
            gameCamera.GetComponent<CameraShake>().shakeDuration = .7f;
            animator.SetBool("isDestroyed", true);

        }
        this.GetComponent<AudioSource>().Play(0);
        yield return new WaitForSeconds(.4f);
        for(int i = 0;i < others.Count;i++){
                Animator animator;
                animator = others[i].GetComponent<Animator>();
                others[i].SetActive(false);
                animator.SetBool("isDestroyed", false);
                gameManager.blocksInScene.Remove(others[i]);
            }
        gameManager.totalScore = score;
        gameManager.numberOfBlocksDestroyed++;
    }
    public IEnumerator BlockChangeColorGameOverCo(){
        float delayTimer = 0f;
        float offset = -6.5f;
        delayTimer = (13f - (transform.position.y - offset))/2f;
        yield return new WaitForSeconds(delayTimer - .5f);
        for(int i = 0;i < others.Count;i++){
            others[i].GetComponent<SpriteRenderer>().color 
                    = new Color(.5f, .5f, .5f);
        }
    }
    private IEnumerator DestroyBlockGameOverCo(){
        float delayTimer = 0f;
        float offset = -6.5f;
        delayTimer = (13f - (transform.position.y - offset))/2f;
        yield return new WaitForSeconds(delayTimer);
        for(int i = 0;i < others.Count;i++){
            Animator animator;
            animator = others[i].GetComponent<Animator>();
            if(animator == null){
                continue;
            }
            gameCamera.GetComponent<CameraShake>().shakeDuration = .7f;
            animator.SetBool("isDestroyed", true);

        }
        this.GetComponent<AudioSource>().Play(0);
        yield return new WaitForSeconds(.4f);
        for(int i = 0;i < others.Count;i++){
                Animator animator;
                animator = others[i].GetComponent<Animator>();
                others[i].SetActive(false);
                animator.SetBool("isDestroyed", false);
                gameManager.blocksInScene.Remove(others[i]);
            }
    }

    private void SetMass(Collider2D other){
        float diffVal = -6.5f;
        other.GetComponent<Rigidbody2D>().mass = 50000 / Mathf.Pow(5f, (this.transform.position.y - diffVal));
    }
}
