using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleLogic : MonoBehaviour
{
    public static bool IsPuzzleStarted;

    public static IEnumerable<PuzzleNode> currentPuzzleNodes;
    public static PuzzleNode LastNode;

    public const float MaxLineDistance = 2;

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
            puzzleNode.ResetLine(false);
        }

        IsPuzzleStarted = false;
    }

    public static void FinishPuzzle()
    {
        foreach (var puzzleNode in currentPuzzleNodes)
        {
            puzzleNode.Collider.enabled = false;
            puzzleNode.ResetLine(true);
        }

        currentPuzzleNodes = null;
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
                if (IsPuzzleStarted && NodeDistance(LastNode, node) < MaxLineDistance && !node.IsMarked)
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

                if (IsPuzzleStarted && AreAllNodesMarked() && NodeDistance(LastNode, node) < MaxLineDistance)
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

    private static float NodeDistance(PuzzleNode n1, PuzzleNode n2)
    {
        return (n1.transform.position - n2.transform.position).magnitude;
    }
}
