using UnityEngine;

public enum NodeType { Middle, Start, Finish }

public class PuzzleNode : MonoBehaviour
{
    public NodeType Type = NodeType.Middle;
    public string PuzzleId;

    public bool IsMarked
    {
        get => Mark.enabled;
        set => Mark.enabled = value;
    }

    public MeshRenderer Mark;

    private Collider _collider;

    public void Start()
    {
        _collider = GetComponent<Collider>();
    }

    public void OnClick()
    {
        switch (Type)
        {
            case NodeType.Start:
                if (!PuzzleController.IsPuzzleStarted)
                {
                    PuzzleController.StartPuzzle(PuzzleId);
                    IsMarked = true;
                }
                else
                    Debug.Log("nope");
                break;

            case NodeType.Middle:
                if (PuzzleController.IsPuzzleStarted)
                    IsMarked = true;
                else
                    Debug.Log("nope");
                break;

            case NodeType.Finish:
                Debug.Log(
                    PuzzleController.IsPuzzleStarted && PuzzleController.AreAllNodesMarked()
                    ? "win" : "nope");

                PuzzleController.ResetPuzzle();
                break;
        }
    }
}
