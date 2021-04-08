using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private PuzzleNode _focusedNode;

    private static Camera _mainCamera;

    public void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    public void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo))
            _focusedNode = hitInfo.collider.gameObject.GetComponent<PuzzleNode>();
    }

    public void OnClick()
    {
        if (_focusedNode != null)
            _focusedNode.OnClick();
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
