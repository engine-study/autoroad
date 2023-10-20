using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
 
    public static CameraControls I;

    [Header("Scroll")]
    [SerializeField] bool canZoom = true; 
    [SerializeField] float scrollSensitivity = 1f;
    [SerializeField] float scrollRot;

    [Header("Pan")]
    public bool canPan = true;
    [SerializeField] private float movementSpeed = 10.0f;

    [Header("Rotate")]
    public bool canRotate = true;
    [SerializeField] private float rotationSpeed = 0.5f;

    [Header("Misc")]
    [SerializeField] bool inCinematic = false; 


    private Vector2 _delta;

    private bool _isMoving;
    private bool _isRotating;

    private float _xRotation;
    private Vector2 currentPos;
    private Vector2 lastPos;
    private Vector2 startPos;
    private Quaternion startRot;
    Quaternion rot;
    float timeDown = 0f;

    void Awake() {
        I = this;
        _xRotation = transform.rotation.eulerAngles.x;
    }
    
    void OnDestroy() {
        I = null;
    }

    void LateUpdate() {

        if(!SPUIBase.CanInput) {return;}

        UpdateInput();
        UpdateDrag();
    }
    
    public static void ToggleZoom(bool toggle) {
        I.canZoom = toggle;
        if(toggle) {} 
        else {I.scrollRot = 0;}
    }

    public static void TogglePan(bool toggle) {
        I.canPan = toggle;
    }

    public static void ToggleRotate(bool toggle) {
        I.canRotate = toggle;
    }

    void UpdateInput() {

        if(Input.GetKeyDown(KeyCode.Minus)) {
            SPCamera.SetFOVGlobal(1f);
        } else if(Input.GetKeyDown(KeyCode.Equals)) {
            SPCamera.SetFOVGlobal(-1f); 
        }
            
        if(Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl)) && Application.isEditor) {
            SPCamera.Screenshot();
        }
            
        if(SPUIBase.IsMouseOnScreen && canZoom) {

            if(Input.GetKey(KeyCode.LeftControl)) {
                
                // scrollRot += Input.mouseScrollDelta.y * rotateSensitivity * Time.deltaTime;
                // scrollLock = Mathf.Round(scrollRot / rotateRound) * rotateRound;

            } else if(SPInput.ModifierKey == false) {
                SPCamera.SetFOVGlobal(SPCamera.I.FOV + Input.mouseScrollDelta.y * -scrollSensitivity);
            }
        }
        
    }

    void UpdateDrag() {

        lastPos = currentPos;
        currentPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        _delta = currentPos - lastPos;

        _isMoving = Input.GetMouseButton(0);
        _isRotating = Input.GetMouseButton(1);

        if(Input.GetMouseButtonDown(0)) {
            startPos = currentPos;
        }

        bool input = _isMoving || _isRotating;

        timeDown = input ? timeDown + Time.deltaTime : 0f;
        if(Vector2.Distance(currentPos, startPos) < 10f) {return;}

        if (_isMoving && canPan) {

            var position = transform.right * (_delta.x * -movementSpeed);
            position += transform.forward * (_delta.y * -movementSpeed);
            position = transform.position + position * Time.deltaTime;

            position.x = Mathf.Clamp(position.x, BoundsComponent.Left, BoundsComponent.Right);
            position.z = Mathf.Clamp(position.z, BoundsComponent.Down, BoundsComponent.Up);

            SPCamera.SetTarget(position);

        } else if (_isRotating && canRotate) {

            rot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + _delta.x * rotationSpeed, transform.eulerAngles.z);
            // rot = Quaternion.Euler(_xRotation, transform.rotation.eulerAngles.y, 0.0f);
            SPCamera.SetTarget(rot);

        }
    }
    
}
