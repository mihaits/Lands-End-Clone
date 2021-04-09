using UnityEngine; 

public enum NodeType { Middle, Start, Finish }

public class PuzzleNode : MonoBehaviour
{
    public NodeType Type = NodeType.Middle;
    public string PuzzleId;

    public LineRenderer Line;
    public MeshRenderer Mark;

    public bool IsDrawingLine;

    public bool IsMarked
    {
        get => Mark.enabled;
        set => Mark.enabled = value;
    }

    private bool _isFocused;
    private Vector3 _focusPoint;

    public Collider Collider;

    private float _distanceToCamera;

    public void Start()
    {
        Collider = GetComponent<Collider>();
    }

    public void StartLine()
    {
        _distanceToCamera = RaycastController.GetDistanceToCamera(transform.position);
        Line.enabled = true;
        IsDrawingLine = true;
    }

    public void FinishLine(Vector3 end)
    {
        Line.SetPositions(new[] {transform.position, end});
        IsDrawingLine = false;
    }

    public void ResetLine()
    {
        Line.enabled = false;
        IsDrawingLine = false;
    }

    public void Update()
    {
        if (IsDrawingLine)
            Line.SetPositions(new[]
            {
                transform.position, RaycastController.GetPosInCenterOfView(_distanceToCamera)
            });

        if (_isFocused)
        {
            // todo: focus visuals
        }
    }

    public void OnFocus()
    {
        _isFocused = true;
    }

    public void UpdateFocusHit(Vector3 point)
    {
        _focusPoint = point;
    }

    public void OnFocusExit()
    {
        _isFocused = false;
    }

    public void OnClick()
    {
        PuzzleController.OnClickNode(this);
    }
}
