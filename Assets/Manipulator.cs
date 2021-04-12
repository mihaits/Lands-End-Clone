using UnityEngine;

public class Manipulator : MonoBehaviour, Interactive
{
    public Rigidbody Rigidbody;

    private float _distance;
    private Vector3 _grabLocalPos;

    private bool _isManipulating;
    private bool _isFocused;

    public float ForceMultiplier   = 50; // spring  
    public float DampingMultiplier = 10; // damper

    public void OnFocus()
    {
        _isFocused = true;
        UIController.FocusManipulatable();
    }

    public void OnFocusExit()
    {
        _isFocused = false;
        UIController.FocusExitManipulatable();
    }

    public void OnClick()
    {
        PuzzleLogic.ResetPuzzle();

        _distance = RaycastController.GetDistanceToCamera(RaycastController.HitInfo.point);
        _grabLocalPos = transform.InverseTransformPoint(RaycastController.HitInfo.point);
        _isManipulating = true;

        UIController.ClickManipulatable();
    }

    public void OnClickUp()
    {
        _isManipulating = false;

        UIController.ClickUpManipulatable();
        if (_isFocused)
            UIController.FocusManipulatable();
    }

    public void FixedUpdate()
    {
        if (_isManipulating)
        {
            var target = RaycastController.GetPosInCenterOfView(_distance);
            var grabPoint = transform.TransformPoint(_grabLocalPos);

            Rigidbody.AddForceAtPosition((target - grabPoint) * ForceMultiplier, grabPoint);
            Rigidbody.AddForceAtPosition(- Rigidbody.GetPointVelocity(grabPoint) * DampingMultiplier, grabPoint);
        }
    }
}
