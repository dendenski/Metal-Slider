using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoveUp : MonoBehaviour
{
    public enum blockState{
        stop,
        move
    }
    private bool hasMoved;
    public blockState currentState;
    private BlockMovement blockMovement;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        blockMovement =  GetComponent<BlockMovement>();
        hasMoved = false;
        currentState = blockState.stop;
    }
    private void OnEnable(){
        hasMoved = false;
        currentState = blockState.stop;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == blockState.stop){
            hasMoved = false;
        }
        if(currentState == blockState.move){
            if(hasMoved == false){
                transform.position = new Vector2(transform.position.x, transform.position.y + 1.18f);
                currentState = blockState.stop;
                hasMoved = true;
            }
        }
    }
}
