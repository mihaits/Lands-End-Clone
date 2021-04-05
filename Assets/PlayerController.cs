using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float LookSpeed = 20;
    public float MoveSpeed = .01f;

    private float _forward;
    private float _right;

    public void Start()
    {
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(
            ClampYRotation(transform.eulerAngles.x - Input.GetAxis("Mouse Y") * LookSpeed),
            transform.eulerAngles.y + Input.GetAxis("Mouse X") * LookSpeed,
            transform.eulerAngles.z);

        UpdateMoveInput();

        var horizontalForward = transform.forward;
        horizontalForward.y = 0;
        horizontalForward.Normalize();

        transform.position = transform.position
                             + horizontalForward * _forward * MoveSpeed
                             + transform.right   * _right   * MoveSpeed; 
    }

    private void UpdateMoveInput()
    {
        _forward = 0;
        _right   = 0;

        _forward += Input.GetKey("w") ? 1 : 0;
        _forward -= Input.GetKey("s") ? 1 : 0;

        _right += Input.GetKey("d") ? 1 : 0;
        _right -= Input.GetKey("a") ? 1 : 0;
    }

    private float ClampYRotation(float angle)
    {
        angle = angle > 180 ? angle - 360 : angle; 

        return Mathf.Clamp(angle, -90, 90);
    }
}
