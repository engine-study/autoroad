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
    [SerializeField] float movementSpeed = 100.0f;
    [SerializeField] Vector2 fovMultiply = new Vector2(1f,10f);
    [SerializeField] Vector2 clippingClose = new Vector2(1f,10f);
    [SerializeField] Vector2 clippingFar = new Vector2(1f,10f);

    [Header("Rotate")]
    public bool canRotate = true;
    [SerializeField] float rotationSpeed = 50f;

    [Header("Misc")]
    [SerializeField] bool inCinematic = false; 

    [Header("Debug")]
    [SerializeField] Vector2 _delta;
    private bool _isMoving;
    private bool _isRotating;

    private float _xRotation;
    private Vector2 currentPos;
    private Vector2 lastPos;
    private Vector2 startPos;
    private Quaternion startRot;
    Quaternion rot;

    void Awake() {
        I = this;
        _xRotation = transform.rotation.eulerAngles.x;
    }
    
    void Start() {
        UpdateClipping();
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
            UpdateClipping();
        } else if(Input.GetKeyDown(KeyCode.Equals)) {
            SPCamera.SetFOVGlobal(-1f); 
            UpdateClipping();
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
                UpdateClipping();
            }
        }
        
    }

    void UpdateClipping() {
        float fovLerp = Mathf.InverseLerp(SPCamera.I.FOVClamp.x, SPCamera.I.FOVClamp.y, SPCamera.I.FOV);
        SPCamera.Camera.nearClipPlane = Mathf.Lerp(clippingClose.x, clippingFar.x, fovLerp);
        SPCamera.Camera.farClipPlane = Mathf.Lerp(clippingClose.y, clippingFar.y, fovLerp);
    }

    bool hasStarted, hasMovedEnough;

    void UpdateDrag()
    {

        lastPos = currentPos;
        currentPos = new Vector2(Input.mousePosition.x / (float)Screen.width, Input.mousePosition.y / (float)Screen.height);

        _delta = currentPos - lastPos;

        _isMoving = Input.GetMouseButton(0) && !SPInput.ModifierKey;
        _isRotating = Input.GetMouseButton(1) || Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift);
        bool _isMovingDown = Input.GetMouseButtonDown(0);
        bool _isRotatingDown = Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift));

        if(SPUIBase.IsPointerOverUIElement == false && (_isMovingDown || _isRotatingDown)) {
            startPos = currentPos;
            hasStarted = true;
            hasMovedEnough = false;
        }

        bool input = _isMoving || _isRotating;

        if(input && (SPPlayer.LocalPlayer == null || SPPlayer.LocalPlayer?.Actor?.ActionState == ActionState.Idle)) {
            if(!hasMovedEnough) {
                hasMovedEnough = Vector2.Distance(currentPos, startPos) > .01f;
                if(hasMovedEnough) {
                    startPos = currentPos;
                }
            }
        } else {
            hasStarted = false;
            hasMovedEnough = false;
        }

        if(!input || !hasStarted || !hasMovedEnough) {return;}

        if (_isMoving && canPan) {

            SPCamera.SetFollow(null);

            var position = transform.right * (_delta.x * -movementSpeed);
            position += transform.forward * (_delta.y * -movementSpeed);

            float fovLerp = Mathf.InverseLerp(SPCamera.I.FOVClamp.x, SPCamera.I.FOVClamp.y, SPCamera.I.FOV);
            float distanceMultiplier =  Mathf.Lerp(fovMultiply.x, fovMultiply.y, fovLerp);

            position *= distanceMultiplier;

            position = transform.position + position;

            position.x = Mathf.Clamp(position.x, BoundsComponent.Left * 2f, BoundsComponent.Right * 2f);
            position.z = Mathf.Clamp(position.z, -20f, BoundsComponent.Up + 10f);

            SPCamera.SetTarget(position);


        } else if (_isRotating && canRotate) {

            rot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + _delta.x * rotationSpeed, transform.eulerAngles.z);
            // rot = Quaternion.Euler(_xRotation, transform.rotation.eulerAngles.y, 0.0f);
            SPCamera.SetTarget(rot);

        }
    }
    
}
