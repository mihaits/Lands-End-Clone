using UnityEngine;

public class Manipulator : MonoBehaviour
{
    public Rigidbody Rigidbody;

    private float _distance;
    private Vector3 _grabLocalPos;

    private bool _isManipulating;

    public float _forceMultiplier   = 50; // spring  
    public float _dampingMultiplier = 10; // damper

    public void OnClick()
    {
        _distance = RaycastController.GetDistanceToCamera(RaycastController.HitInfo.point);
        _grabLocalPos = transform.InverseTransformPoint(RaycastController.HitInfo.point);
        _isManipulating = true;
    }

    public void OnClickUp()
    {
        _isManipulating = false;
    }

    public void FixedUpdate()
    {
        if (_isManipulating)
        {
            var target = RaycastController.GetPosInCenterOfView(_distance);
            var grabPoint = transform.position + transform.rotation * _grabLocalPos;

            Rigidbody.AddForceAtPosition((target - grabPoint) * _forceMultiplier, grabPoint);
            Rigidbody.AddForceAtPosition(- Rigidbody.GetPointVelocity(grabPoint) * _dampingMultiplier, grabPoint);
        }
    }
}
