using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private Collider _previouslyFocusedCollider = new Collider();
    private Collider _focusedCollider;

    private static Camera _mainCamera;

    public static RaycastHit HitInfo;

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
    }

    private void OnFocusExit()
    {
        var previouslyFocusedNode = _previouslyFocusedCollider.GetComponent<PuzzleNode>();
        if (previouslyFocusedNode != null)
            previouslyFocusedNode.OnFocusExit();
    }

    public void Click()
    {
        if (_focusedCollider != null)
        {
            var focusedNode = _focusedCollider.GetComponent<PuzzleNode>();
            if (focusedNode != null)
                focusedNode.OnClick();
        }
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
}
