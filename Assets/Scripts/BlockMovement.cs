using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BlockState{
    moveBlock,
    releaseBlock,
    endTurn
}
public class BlockMovement : MonoBehaviour
{

    public BlockState currentState;
    private Collider2D boxCollider;
    private Rigidbody2D myRigidBody;
    public GameManager gameManager;
    public Vector2 tempTouchPosition;
    public float currentPositionX;
    public int blockSize;
    public bool isClicked;
    // Start is called before the first frame update
    void Start()
    {
        isClicked = false;
        currentState = BlockState.moveBlock;
        gameManager = FindObjectOfType<GameManager>();
        myRigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameState == GameState.play){
            switch(currentState){
                case BlockState.moveBlock:
                    //TouchMove();
                    mouseMove();
                    break;
                case BlockState.releaseBlock:
                    gameManager.blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().collisionDetectionMode
                            = CollisionDetectionMode2D.Discrete);
                    gameManager.gameState = GameState.blocksFall;
                    currentState = BlockState.endTurn;
                    break;
                case BlockState.endTurn:
                    break;

                default:
                    break;
            }
        }
    }
    private void mouseMove(){
        Vector2 touchPosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
        if(Input.GetMouseButtonDown(0) && boxCollider == touchedCollider){
            ClickBlock();
        }
        if(Input.GetMouseButton(0) && isClicked){
            DragBlock(touchPosition);
        }
        if(Input.GetMouseButtonUp(0) && isClicked){
            ReleaseBlock();
        }
    }

    private void TouchMove(){
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition =  Camera.main.ScreenToWorldPoint(touch.position);
            Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
            if(touch.phase == TouchPhase.Began && boxCollider == touchedCollider){
                ClickBlock();
            }
            if(touch.phase == TouchPhase.Moved && isClicked){
                DragBlock(touchPosition);
            }
            if(touch.phase == TouchPhase.Ended && isClicked){
                ReleaseBlock();
            }
        }
    }

    private void ClickBlock(){
        gameManager.blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().constraints 
            = RigidbodyConstraints2D.FreezeAll);
        currentPositionX = transform.position.x;
        isClicked = true;
        Vector2 sizeY = GetComponent<BoxCollider2D>().size;
        sizeY.y = 0.5f;
        GetComponent<BoxCollider2D>().size = sizeY;
        myRigidBody.constraints  &= ~RigidbodyConstraints2D.FreezePositionX;
    }
    private void DragBlock(Vector2 touchPosition){
        tempTouchPosition = touchPosition;
        tempTouchPosition.y = transform.position.y;
        myRigidBody.MovePosition(tempTouchPosition);
    }
    private void ReleaseBlock(){
        Vector2 sizeY = GetComponent<BoxCollider2D>().size;
        sizeY.y = 1f;
        GetComponent<BoxCollider2D>().size = sizeY;
        isClicked = false;
        SnapToGrid();
        if(Mathf.Abs(currentPositionX - transform.position.x) >= 0.01f){
            currentState = BlockState.releaseBlock;
        }
    }

    public void SnapToGrid(){
        float offset = 0f;
        if(blockSize == 2){
            offset = -0.5f;
        }else if(blockSize == 3){
            offset = -1f;
        }else if(blockSize == 4){
            offset = -1.5f;
        }
        float distance = Mathf.Abs(transform.position.x - (gameManager.spawnPoints[0].position.x - offset));
        float distanceToCompare;
        float finalPosition = gameManager.spawnPoints[0].position.x - offset;
        for(int i = 1; i < gameManager.spawnPoints.Length; i++){
            distanceToCompare = Mathf.Abs(transform.position.x - (gameManager.spawnPoints[i].position.x - offset));
            if(distance > distanceToCompare){
                distance = distanceToCompare;
                finalPosition = (gameManager.spawnPoints[i].position.x - offset);
            }
        }
        tempTouchPosition.x = finalPosition;
        transform.position = tempTouchPosition;
        gameManager.blocksInScene.ForEach(c => c.GetComponent<Rigidbody2D>().constraints 
                    = RigidbodyConstraints2D.FreezeAll);
    }
}
