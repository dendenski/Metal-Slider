using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraAdjust : MonoBehaviour
{
    float baseScreenResolution = 9f/18f;
    float widthCamera;
    float heightCamera;

    public Canvas canvas;
    
    Camera thisCamera;
    // Start is called before the first frame update
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        AdjustCamera();
    }
    // Update is called once per frame
    void Update()
    {
        AdjustCamera();
    }
    private void AdjustCamera(){
        RectTransform rect = canvas.GetComponent<RectTransform>();
        widthCamera = Screen.width;
        heightCamera = Screen.height;
        baseScreenResolution = widthCamera/heightCamera;
        if(baseScreenResolution <= .5f){
            thisCamera.orthographicSize = 5f / baseScreenResolution;
            rect.sizeDelta = new Vector2(10f, 10f/baseScreenResolution);
        }else{
            thisCamera.orthographicSize = 10f;
            rect.sizeDelta = new Vector2(20f*baseScreenResolution,20f);
        }
    }
}
