using UnityEngine;

public enum NodeType { Middle, Start, Finish }

public class PuzzleNode : MonoBehaviour
{
    public NodeType Type = NodeType.Middle;
    public string PuzzleId;

    private Collider _collider;

    public void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void Update()
    {
        
    }
}
