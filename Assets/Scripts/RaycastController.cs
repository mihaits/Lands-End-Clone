using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private static Collider _previouslyFocusedCollider = new Collider();
    private static Collider _focusedCollider;

    private static Camera _mainCamera;

    public static RaycastHit HitInfo;
    private PuzzleNode _clickedNode;
    private Manipulator _clickedManipulatable;

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
                OnFocusExit();
            
            if (_focusedCollider != null)
                OnFocus();
        }
    }

    private void OnFocus()
    {
        var focusedNode = _focusedCollider.GetComponent<PuzzleNode>();
        if (focusedNode != null)
            focusedNode.OnFocus();

        var focusedManipulatable = _focusedCollider.GetComponent<Manipulator>();
        if (focusedManipulatable != null)
            focusedManipulatable.OnFocus();
    }

    private void OnFocusExit()
    {
        var previouslyFocusedNode = _previouslyFocusedCollider.GetComponent<PuzzleNode>();
        if (previouslyFocusedNode != null)
            previouslyFocusedNode.OnFocusExit();

        var previouslyFocusedManipulatable = _previouslyFocusedCollider.GetComponent<Manipulator>();
        if (previouslyFocusedManipulatable != null)
            previouslyFocusedManipulatable.OnFocusExit();
    }

    public void Click()
    {
        if (_focusedCollider != null)
        {
            var focusedNode = _focusedCollider.GetComponent<PuzzleNode>();
            if (focusedNode != null)
            {
                _clickedNode = focusedNode;
                _clickedNode.OnClick();
            }

            var focusedManipulatable = _focusedCollider.GetComponent<Manipulator>();
            if (focusedManipulatable != null)
            {
                _clickedManipulatable = focusedManipulatable;
                _clickedManipulatable.OnClick();
            }
        }
    }

    public void ClickUp()
    {
        if (_clickedManipulatable != null)
            _clickedManipulatable.OnClickUp();
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
