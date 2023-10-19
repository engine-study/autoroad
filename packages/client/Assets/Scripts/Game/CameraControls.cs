using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
 
    public static CameraControls I;
    [Header("Controls")] 
    [SerializeField] float scrollSensitivity = 1f;
    [SerializeField] float rotateSensitivity = 360f;
    [SerializeField] float rotateRound = 45f;
    [SerializeField] float scrollRot, scrollLock;
    [SerializeField] bool canScroll = true; 
    [SerializeField] bool inCinematic = false; 

    void Awake() {
        I = this;
    }
    
    void OnDestroy() {
        I = null;
    }

    void Update()
    {
        UpdateInput();
    }
    
    public static void ToggleScroll(bool toggle) {
        I.canScroll = toggle;
        if(toggle) {} 
        else {I.scrollRot = 0;}
    }

    void UpdateInput() {

        if(SPUIBase.CanInput) {
            if(Input.GetKeyDown(KeyCode.Minus)) {
                SPCamera.SetFOVGlobal(1f);
            } else if(Input.GetKeyDown(KeyCode.Equals)) {
                SPCamera.SetFOVGlobal(-1f); 
            }
                
            if(Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl)) && Application.isEditor) {
                SPCamera.Screenshot();
            }
            
        }    

        if(SPUIBase.IsMouseOnScreen && canScroll) {

            if(Input.GetKey(KeyCode.LeftControl)) {
                
                // scrollRot += Input.mouseScrollDelta.y * rotateSensitivity * Time.deltaTime;
                // scrollLock = Mathf.Round(scrollRot / rotateRound) * rotateRound;

            } else if(SPInput.ModifierKey == false) {
                SPCamera.SetFOVGlobal(SPCamera.I.FOV + Input.mouseScrollDelta.y * -scrollSensitivity);
            }
        }
        
    }
}
