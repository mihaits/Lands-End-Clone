using UnityEngine;

public class RaycastController : MonoBehaviour
{
    private PuzzleNode _focusedNode;

    public void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo))
            _focusedNode = hitInfo.collider.gameObject.GetComponent<PuzzleNode>();
    }

    public void OnClick()
    {
        if (_focusedNode != null)
        {
            _focusedNode.OnClick();
        }
    }
}
