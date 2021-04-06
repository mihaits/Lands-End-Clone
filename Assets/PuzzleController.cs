using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public static bool IsPuzzleStarted;

    public static IEnumerable<PuzzleNode> currentPuzzleNodes;

    public static void StartPuzzle(string puzzleId)
    {
        currentPuzzleNodes = FindObjectsOfType<PuzzleNode>()
            .Where(n => n.PuzzleId == puzzleId && n.Type != NodeType.Finish);

        IsPuzzleStarted = true;
    }

    public static void ResetPuzzle()
    {
        if (currentPuzzleNodes == null) return;

        foreach (var currentPuzzleNode in currentPuzzleNodes)
            currentPuzzleNode.IsMarked = false;

        IsPuzzleStarted = false;
    }

    public static bool AreAllNodesMarked()
    {
        if (currentPuzzleNodes == null) return false;

        return currentPuzzleNodes.All(n => n.IsMarked);
    }
}
