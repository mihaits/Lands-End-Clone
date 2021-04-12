using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum NodeType { Middle, Start, Finish }

public class PuzzleNode : MonoBehaviour, Interactive
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
            if (RaycastController.IsFocusingNode(out var distance))
                _distanceToCamera = distance;

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

            var distanceToCenter = _focusCoords.magnitude;

            
            var color = distanceToCenter < _clickRadius
                ? Color.black : Color.white;
            color.a = (distanceToCenter - .5f) / -.05f;
            FocusHalo.color = color;

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

    public void OnClickUp() {}
}
