using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundChangeColor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    //The Color to be assigned to the Renderer’s Material
    private bool updateCheck;
    // Start is called before the first frame update
    void Start()
    {
        updateCheck = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        updateCheck = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(updateCheck)
        {
            StartCoroutine(ChangeColorCo());
            updateCheck = false;
        }
    }
    Color RandomColor(){
        return new Color(Random.value, Random.value, Random.value);
    }
    public IEnumerator ChangeColorCo(){
        yield return new WaitForSeconds(0.50f);
        spriteRenderer.color = RandomColor();
        updateCheck = true;
    }
}
