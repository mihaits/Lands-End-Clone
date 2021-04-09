using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
    public static bool IsPuzzleStarted;

    public static IEnumerable<PuzzleNode> currentPuzzleNodes;
    public static PuzzleNode LastNode;

    public static void StartPuzzle(string puzzleId)
    {
        currentPuzzleNodes = FindObjectsOfType<PuzzleNode>()
            .Where(n => n.PuzzleId == puzzleId);

        IsPuzzleStarted = true;
    }

    public static void ResetPuzzle()
    {
        if (currentPuzzleNodes == null) return;

        foreach (var puzzleNode in currentPuzzleNodes)
        {
            puzzleNode.IsMarked = false;
            puzzleNode.ResetLine();
        }

        IsPuzzleStarted = false;
    }

    public static void FinishPuzzle()
    {
        foreach (var puzzleNode in currentPuzzleNodes)
            puzzleNode.Collider.enabled = false;
    }

    public static bool AreAllNodesMarked()
    {
        if (currentPuzzleNodes == null) return false;

        return currentPuzzleNodes
            .All(n => n.IsMarked);
    }

    public static void OnClickNode(PuzzleNode node)
    {
        switch (node.Type)
        {
            case NodeType.Start:
                if (!IsPuzzleStarted)
                {
                    StartPuzzle(node.PuzzleId);
                    LastNode = node;
                    node.IsMarked = true;
                    node.StartLine();
                }
                else
                    ResetPuzzle();
                break;

            case NodeType.Middle:
                if (IsPuzzleStarted)
                {
                    LastNode.FinishLine(node.transform.position);
                    LastNode = node;
                    node.IsMarked = true;
                    node.StartLine();
                }
                else
                    ResetPuzzle();
                break;

            case NodeType.Finish:
                node.IsMarked = true;

                if (IsPuzzleStarted && AreAllNodesMarked())
                {
                    LastNode.FinishLine(node.transform.position);
                    FinishPuzzle();
                }
                else
                {
                    node.IsMarked = false;
                    ResetPuzzle();
                }

                break;
        }
    }
}
