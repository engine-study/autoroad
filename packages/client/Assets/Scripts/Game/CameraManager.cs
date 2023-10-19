using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Variables

    private Vector2 _delta;

    private bool _isMoving;
    private bool _isRotating;
    private bool _isBusy;

    private float _xRotation;
    private Vector2 currentPos;
    private Vector2 lastPos;

    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float rotationSpeed = 0.5f;

    Quaternion rot;

    #endregion

    private void Awake()
    {
        _xRotation = transform.rotation.eulerAngles.x;
    }

    private void LateUpdate()
    {

        lastPos = currentPos;
        currentPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        _delta = currentPos - lastPos;

        _isMoving = Input.GetMouseButton(0);
        _isRotating = Input.GetMouseButton(1);

        if (_isMoving) {

            var position = transform.right * (_delta.x * -movementSpeed);
            position += transform.forward * (_delta.y * -movementSpeed);
            position = transform.position + position * Time.deltaTime;

            position.x = Mathf.Clamp(position.x, BoundsComponent.Left, BoundsComponent.Right);
            position.z = Mathf.Clamp(position.z, BoundsComponent.Down, BoundsComponent.Up);

            SPCamera.SetTarget(position);

        }

        if (_isRotating) {
            rot = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + -_delta.x * rotationSpeed, transform.eulerAngles.z);
            // rot = Quaternion.Euler(_xRotation, transform.rotation.eulerAngles.y, 0.0f);
            SPCamera.SetTarget(rot);

        }
    }

}
