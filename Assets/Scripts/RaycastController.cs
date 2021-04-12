using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private static Collider _previouslyFocusedCollider = new Collider();
    private static Collider _focusedCollider;

    private static Camera _mainCamera;

    public static RaycastHit HitInfo;
    private Interactive _clickedInteractive;

    public void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    public void FixedUpdate()
    {
        _previouslyFocusedCollider = _focusedCollider;

        _focusedCollider = 
            Physics.Raycast(transform.position, transform.forward, out HitInfo) 
            ? HitInfo.collider : null;

        if (_previouslyFocusedCollider != _focusedCollider)
        {
            if (_previouslyFocusedCollider != null)
                FocusExit();
            
            if (_focusedCollider != null)
                Focus();
        }
    }

    private void Focus()
    {
        var focusedInteractive = _focusedCollider.GetComponent<Interactive>();
        focusedInteractive?.OnFocus();
    }

    private void FocusExit()
    {
        var previouslyFocusedInteractive = _previouslyFocusedCollider.GetComponent<Interactive>();
        previouslyFocusedInteractive?.OnFocusExit();
    }

    public void Click()
    {
        if (_focusedCollider != null)
        {
            var focusedInteractive = _focusedCollider.GetComponent<Interactive>();
            if (focusedInteractive != null)
            {
                _clickedInteractive = focusedInteractive;
                _clickedInteractive.OnClick();
            }
        }
    }

    public void ClickUp()
    {
        _clickedInteractive?.OnClickUp();
    }

    public static Vector3 GetPosInCenterOfView(float distance)
    {
        var cameraTransform = _mainCamera.gameObject.transform;

        return cameraTransform.position + cameraTransform.forward * distance;
    }

    public static float GetDistanceToCamera(Vector3 pos)
    {
        return (pos - _mainCamera.transform.position).magnitude;
    }

    public static bool IsFocusingNode(out float distance)
    {
        distance = 0;

        if (_focusedCollider != null)
        {
            var focusedNode = _focusedCollider.GetComponent<PuzzleNode>();
            if (focusedNode != null)
            {
                distance = GetDistanceToCamera(HitInfo.point);
                return true;
            }
        }

        return false;
    }
}
