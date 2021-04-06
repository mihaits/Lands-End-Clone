using UnityEngine;

public class RaycastController : MonoBehaviour
{
    public void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo))
        {
            var node = hitInfo.collider.gameObject.GetComponent<PuzzleNode>();
        
            if (node != null)
                Debug.Log($"node: {node.Type}");
        }
    }
}
