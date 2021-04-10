using UnityEngine;
using UnityEngine.UI;

public enum NodeType { Middle, Start, Finish }

public class PuzzleNode : MonoBehaviour
{
    public NodeType Type = NodeType.Middle;
    public string PuzzleId;

    public LineRenderer Line;
    public MeshRenderer Mark;
    public Collider Collider;

    public bool IsDrawingLine;

    public bool IsMarked
    {
        get => Mark.enabled;
        set => Mark.enabled = value;
    }

    public Image FocusHalo;
    public Mask FocusImageMask;

    private bool _isFocused;
    private Vector2 _focusCoords;
    private float _clickRadius = .15f;

    private float _distanceToCamera;

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
            // [A, B] --> [a, b]
            // (val - A) * (b - a) / (B - A) + a

            // distance to center
            // [0.5                clickRadius                    0]
            // alpha
            // [0                       1                         1]
            // scale
            // [clickRadius + .5  clickRadius * 2   clickRadius * 2]


            var distanceToCenter = _focusCoords.magnitude;

            var color = FocusHalo.color;
            color.a = (distanceToCenter - .5f) / (_clickRadius - .5f);
            FocusHalo.color = color;

            var scale = Mathf.Max(distanceToCenter + _clickRadius, _clickRadius * 2);
            FocusHalo.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void OnFocus()
    {
        _isFocused = true;
        FocusHalo.enabled = true;
    }

    public void UpdateFocusHit(Vector3 point)
    {
        _focusCoords = transform.InverseTransformPoint(point);
    }

    public void OnFocusExit()
    {
        _isFocused = false;
        FocusHalo.enabled = false;
    }

    public void OnClick()
    {
        if (_focusCoords.magnitude < _clickRadius)
            PuzzleLogic.OnClickNode(this);
    }
}
