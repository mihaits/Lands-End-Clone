using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float LookSpeed = 5;
    public float MoveSpeed = 5;

    public RaycastController RaycastController;

    private float _lookUp;
    private float _lookRight;

    private float _moveForward;
    private float _moveRight;

    private bool _mouseDown = false;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && !_mouseDown)
        {
            _mouseDown = true;
            RaycastController.OnClick();
        }
        else if (!Input.GetMouseButton(0) && _mouseDown)
            _mouseDown = false;
    }

    public void Update()
    {
        UpdateInput();


        var xAngle = ClampYAngle(transform.eulerAngles.x - _lookUp    * LookSpeed);
        var yAngle =             transform.eulerAngles.y + _lookRight * LookSpeed;

        transform.rotation = Quaternion.Euler(xAngle, yAngle, transform.eulerAngles.z);


        var horizontalForward = transform.forward;
        horizontalForward.y = 0;
        horizontalForward.Normalize();

        transform.position = transform.position 
            + horizontalForward * _moveForward * MoveSpeed * Time.deltaTime
            + transform.right   * _moveRight   * MoveSpeed * Time.deltaTime;
    }

    private void UpdateInput()
    {
        _moveForward = 0;
        _moveRight   = 0;

        _moveForward += Input.GetKey("w") ? 1 : 0;
        _moveForward -= Input.GetKey("s") ? 1 : 0;

        _moveRight   += Input.GetKey("d") ? 1 : 0;
        _moveRight   -= Input.GetKey("a") ? 1 : 0;

        _lookUp    = Input.GetAxis("Mouse Y");
        _lookRight = Input.GetAxis("Mouse X");
    }

    private static float ClampYAngle(float angle)
    {
        angle = angle > 180 ? angle - 360 : angle; 

        return Mathf.Clamp(angle, -90, 90);
    }
}
