using DG.Tweening;
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
    public RectMask2D FocusEffectMask;

    private bool _isFocused;
    private Vector2 _focusCoords;
    private float _clickRadius = .15f;

    private float _distanceToCamera;

    private Tween _lineColorTween;

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

    public void ResetLine(bool puzzleComplete)
    {
        IsDrawingLine = false;

        _lineColorTween?.Kill();

        _lineColorTween = Line.material
            .DOColor(puzzleComplete 
                ? new Color(0, 0, 0, 0) 
                : new Color(1, 1, 1, 0), 
                puzzleComplete ? 1 : .5f)
            .OnComplete(() =>
            {
                Line.enabled = false;

                Line.sharedMaterial.color = Color.white;
                _lineColorTween = null;
            });
    }

    public void Update()
    {
        if (IsDrawingLine)
        {
            var p1 = transform.position;
            var p2 = RaycastController.GetPosInCenterOfView(_distanceToCamera);
            
            if ((p1 - p2).magnitude + _clickRadius < PuzzleLogic.MaxLineDistance)
                Line.SetPositions(new[] { p1, p2 });
            else
                PuzzleLogic.ResetPuzzle();
        }

        if (_isFocused && !IsMarked)
        {
            UpdateFocusCoords();

            // [A, B] --> [a, b]
            // (val - A) * (b - a) / (B - A) + a

            // distance to center
            // [0.5                clickRadius                    0]
            // alpha
            // [0                       1                         1]
            // scale
            // [clickRadius + .5  clickRadius * 2   clickRadius * 2]

            // dtc
            // [.5     .45]
            // alpha (v2)
            // [0        1]

            var distanceToCenter = _focusCoords.magnitude;

            var color = FocusHalo.color;
            // color.a = (distanceToCenter - .5f) / (_clickRadius - .5f);
            color.a = (distanceToCenter - .5f) / -.05f; // v2
            FocusHalo.color = color;

            // var scale = Mathf.Max(distanceToCenter + _clickRadius, _clickRadius * 2);
            var scale = Mathf.Max(distanceToCenter * 2, _clickRadius * 2);
            FocusHalo.transform.localScale = new Vector3(scale, scale, 1);

            FocusEffectMask.padding = new Vector4
            (
                  _focusCoords.x + .1f,
                  _focusCoords.y + .1f,
                - _focusCoords.x + .1f,
                - _focusCoords.y + .1f
            );
        }
    }

    private void UpdateFocusCoords()
    {
        _focusCoords = transform.InverseTransformPoint(RaycastController.HitInfo.point);
    }

    public void OnFocus()
    {
        _isFocused = true;
        FocusHalo.enabled = true;
    }

    public void OnFocusExit()
    {
        _isFocused = false;
        FocusHalo.enabled = false;
    }

    public void OnClick()
    {
        if (_focusCoords.magnitude < _clickRadius)
        {
            PuzzleLogic.OnClickNode(this);
            FocusHalo.DOColor(new Color(1, 1, 1, 0), .25f);
            FocusHalo.transform.DOScale(Vector3.zero, .25f);
        }
    }
}
