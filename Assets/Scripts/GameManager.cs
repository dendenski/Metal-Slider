using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState{
    play,
    blocksFall,
    blocksCheck,
    nextTurn,
    gameOver
}
public class GameManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] verticalPos;
    public GameObject blockSize1;
    public GameObject blockSize2;
    public GameObject blockSize3;
    public GameObject blockSize4;
    public List<GameObject> blocksInScene;
    private ObjectPool objectPool;
    public GameState gameState;
    public bool blocksDestroyed;
    public bool blockPlaced;
    public Text levelText;
    public int numberOfBlocksDestroyed;
    
    public int totalScore;
    public ScoreManager scoreManager;
    public EndGameManager endGameManager;
    public int level;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        totalScore = 0;
        numberOfBlocksDestroyed = 0;
        levelText.gameObject.SetActive(false);
        level = 1;
        blockPlaced = false;
        blocksDestroyed = false;
        gameState = GameState.play;
        int randomRange = 5;
        endGameManager = FindObjectOfType<EndGameManager>();
        objectPool = FindObjectOfType<ObjectPool>();
        scoreManager = FindObjectOfType<ScoreManager>();
        bool blankAdded = false;
        for(int i = 0; i < spawnPoints.Length;){
            int blocksCreated = Random.Range(0,randomRange);
            if(i >= 4  && !blankAdded){
                i+= 1;
                blankAdded = true;
            }
            else if(blocksCreated == 1){
                blocksInScene.Add(Instantiate(blockSize1, spawnPoints[i].position, Quaternion.identity));
                i += 1;
            }
            else if(blocksCreated == 2){
                Vector2 tempPos = spawnPoints[i].position;
                tempPos.x -= -0.5f;
                blocksInScene.Add(Instantiate(blockSize2, tempPos, Quaternion.identity));
                i += 2;
            }
            else if(blocksCreated == 3){
                Vector2 tempPos = spawnPoints[i].position;
                tempPos.x -= -1f;
                blocksInScene.Add(Instantiate(blockSize3, tempPos, Quaternion.identity));
                i += 3;
            }
            else if(blocksCreated == 4){
                Vector2 tempPos = spawnPoints[i].position;
                tempPos.x -= -1.5f;
                blocksInScene.Add(Instantiate(blockSize4, tempPos, Quaternion.identity));
                i += 4;
            }else{
                i+= 1;
                blankAdded = true;
            }
            randomRange = SpawnRange(i);
        }
    }
    public void PlaceBlocks(){
        int randomRange = 5;
        bool blankNotAdded = true;
        blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().gravityScale 
                        = 0f);
        blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().constraints 
                        |= RigidbodyConstraints2D.FreezePositionX);
        for(int i = 0; i < spawnPoints.Length;){
            randomRange = SpawnRange(i);
            int blocksCreated = Random.Range(0,randomRange);
            if(blocksCreated == 1){
                if(i == 7 && blankNotAdded){
                    i+= 1;
                    blankNotAdded = false;
                    continue;
                }
                GameObject block = objectPool.GetPooledObject("BlockSize1");
                AddBlocks(block, spawnPoints[i].transform);
                i += 1;
            }
            else if(blocksCreated == 2){
                if(i == 6 && blankNotAdded){
                    i+= 1;
                    blankNotAdded = false;
                    continue;
                }
                GameObject block = objectPool.GetPooledObject("BlockSize2");
                Transform  tempTrans = spawnPoints[i].transform;
                Vector2 tempPos = tempTrans.position;
                tempPos.x -= -0.5f;
                tempTrans.position = tempPos;
                AddBlocks(block, tempTrans);
                tempPos.x += -0.5f;
                tempTrans.position = tempPos;
                i += 2;
            }
            else if(blocksCreated == 3){
                if(i == 5 && blankNotAdded){
                    i+= 1;
                    blankNotAdded = false;
                    continue;
                }
                GameObject block = objectPool.GetPooledObject("BlockSize3");
                Transform  tempTrans = spawnPoints[i].transform;
                Vector2 tempPos = tempTrans.position;
                tempPos.x -= -1f;
                tempTrans.position = tempPos;
                AddBlocks(block, spawnPoints[i].transform);
                tempPos.x += -1f;
                tempTrans.position = tempPos;
                i += 3;
            }
            else if(blocksCreated == 4){
                if(i == 4 && blankNotAdded){
                    i+= 1;
                    blankNotAdded = false;
                    continue;
                }
                GameObject block = objectPool.GetPooledObject("BlockSize4");
                Transform  tempTrans = spawnPoints[i].transform;
                Vector2 tempPos = tempTrans.position;
                tempPos.x -= -1.5f;
                tempTrans.position = tempPos;
                AddBlocks(block, spawnPoints[i].transform);
                tempPos.x += -1.5f;
                tempTrans.position = tempPos;
                i += 4;
            }else{
                i+= 1;
                blankNotAdded = false;
            }
            
        }
        blockPlaced = true;
    }

    void AddBlocks(GameObject block,Transform pos){
        blocksInScene.Add(block);
        if(block != null){
            block.transform.position = pos.position;
            block.transform.rotation = Quaternion.identity;
            block.SetActive(true);
        }
    }
    public int SpawnRange(int i){
        int randomRange = 5;
        if(i >= 7){
            randomRange = 2;
        }
        else if(i >= 6){
            randomRange = 3;
        }
        else if(i >= 5){
            randomRange = 4;
        }
        return randomRange;
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState){
            case GameState.play:
                break;
            case GameState.blocksFall:
                StartCoroutine(BlocksWillFallCo());
                gameState = GameState.blocksCheck;
                break;
            case GameState.blocksCheck:
                ComboDisplay();
                break;
            case GameState.nextTurn:
                StartCoroutine(BlocksIncrease());
                gameState = GameState.blocksCheck;           
                break;
            case GameState.gameOver:
                break;
            default:
                break;
        }
        if(gameState != GameState.play){
            //checkVelocity();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            //Application.Quit();
            if(isPaused){
                Time.timeScale = 1;
                isPaused = false;
            }else if(!isPaused){
                Time.timeScale = 0;
                isPaused = true;
            }
        }
    }


    private IEnumerator BlocksWillFallCo(){
        DisableRowChecker();
        EnableGravity();
        StartCoroutine(checkVelocity());
        yield return new WaitForSeconds(.8f);
        DisableGravity();
        EnableRowChecker();
        yield return new WaitForSeconds(.5f);
        DisableRowChecker();
        if(blocksDestroyed){
            blocksDestroyed = false;
            gameState = GameState.blocksFall;
        }else{
            gameState = GameState.nextTurn;
        }
    }

    private IEnumerator BlocksIncrease(){
        float waitTime = .5f;
        if(!blockPlaced || ((level >= 20) && (blocksInScene.Count < 14)) || blocksInScene.Count == 0){
            MoveBlockUp();
            if(level % 5 == 0){
                //levelText.gameObject.SetActive(true);
                levelText.text = "Level " + level;
            }
            yield return new WaitForSeconds(.4f);
            PlaceBlocks();
        }
        EnableGravity();
        StartCoroutine(checkVelocity());
        yield return new WaitForSeconds(waitTime);
        DisableGravity();
        if(endGameManager.isGameOver){
            gameState = GameState.gameOver;
            levelText.gameObject.SetActive(false);
            scoreManager.SetHighScore();
            EnableRowChecker();
            StartCoroutine(endGameManager.GameOver());
        }else{
            EnableRowChecker();
            if(blocksDestroyed){
                waitTime = 1f;
            }
            StartCoroutine(checkVelocity());
            yield return new WaitForSeconds(waitTime);
            DisableRowChecker();
            if(blocksDestroyed){
                blocksDestroyed = false;
                gameState = GameState.blocksFall;
            }else{
                scoreManager.getScore();
                levelText.gameObject.SetActive(false);
                totalScore = 0;
                numberOfBlocksDestroyed = 0;
                //Debug.Log("level: "+ level + "blocksInScene.Count: "+ blocksInScene.Count);
                level++;
                blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().collisionDetectionMode
                                = CollisionDetectionMode2D.Continuous);
                gameState = GameState.play;
                blockPlaced = false;
                blocksInScene.ForEach(c => c.GetComponent<BlockMovement>().currentState = BlockState.moveBlock);
            }
        }

    }

    private void MoveBlockUp(){
        blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().constraints 
                    &= ~RigidbodyConstraints2D.FreezePositionY);
        blocksInScene.ForEach(c => c.GetComponent<BlockMoveUp>().currentState 
                                    = BlockMoveUp.blockState.move);
    }

    public Vector2 SnapToYGrid(Vector2 pos){
        float distance = Mathf.Abs(pos.y - verticalPos[0].transform.position.y);
        float distanceToCompare;
        float finalPosition = verticalPos[0].transform.position.y;
        for(int i = 1; i < verticalPos.Length; i++){
            distanceToCompare = Mathf.Abs(pos.y - verticalPos[i].transform.position.y);
            if(distance > distanceToCompare){
                distance = distanceToCompare;
                finalPosition = verticalPos[i].transform.position.y;
            }
        }
        pos.y = finalPosition;
        return pos;
    }

    private void EnableGravity(){
        blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().gravityScale = 1f);
        blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().constraints 
                    &= ~RigidbodyConstraints2D.FreezePositionY);
    }

    private void DisableGravity(){
        blocksInScene.ForEach(c => c.transform.position = SnapToYGrid(c.transform.position));
        blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().constraints 
                    |= RigidbodyConstraints2D.FreezePositionY);
        blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().gravityScale = 0.0001f);
    }
    private void DisableRowChecker(){
        for(int i = 0;i < verticalPos.Length;i++){
            verticalPos[i].GetComponent<RowChecker>().blockDetected = 0;
            verticalPos[i].GetComponent<RowChecker>().others.Clear(); 
            verticalPos[i].SetActive(false);
        }
    }
    public void EnableRowChecker(){
        for(int i = 0;i < verticalPos.Length;i++){
            verticalPos[i].SetActive(true);
        }
    }
    private IEnumerator checkVelocity(){
        yield return new WaitForSeconds(.05f);
        for(int i = 0; i < blocksInScene.Count; i++)
        {
            if(blocksInScene[i].GetComponent<Rigidbody2D>().velocity == Vector2.zero){
                blocksInScene[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                blocksInScene[i].GetComponent<Rigidbody2D>().gravityScale = 0f;
                blocksInScene[i].transform.position = SnapToYGrid(blocksInScene[i].transform.position);
            };
        }
    }
    private void ComboDisplay(){
        if(numberOfBlocksDestroyed >= 2){
            levelText.text = "" + numberOfBlocksDestroyed + " Combo!!! ";
            levelText.gameObject.SetActive(true);
        }
    }
}
