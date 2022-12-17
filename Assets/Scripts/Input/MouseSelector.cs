using UnityEngine;

public class MouseSelector : MonoBehaviour
{
    [SerializeField] private MouseInputControl mouseControl;
    [SerializeField] private Camera cursorCamera;
    private bool _isStartRayHit;

    private Vector3 _startDragPos, _endDragPos;

    private void Awake()
    {
        if (mouseControl == null)
        {
            var comp = GetComponent<MouseInputControl>();
            if (comp == null)
            {
                comp = FindObjectOfType<MouseInputControl>();
                if (comp == null) comp = gameObject.AddComponent<MouseInputControl>();
            }

            mouseControl = comp;
        }

        if (cursorCamera == null) cursorCamera = Camera.main ? Camera.main : Camera.current;
    }

    private void OnEnable()
    {
        mouseControl.OnMousePosition += OnMousePositionChanged;
        mouseControl.OnLeftButtonChanged += DrawAreaToWorld;
    }

    private void OnDisable()
    {
        mouseControl.OnMousePosition -= OnMousePositionChanged;
        mouseControl.OnLeftButtonChanged -= DrawAreaToWorld;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = new Color(0.8f, 0f, 0f, 0.5f);
        var x = Mathf.Max(_startDragPos.x, _endDragPos.x) - Mathf.Min(_startDragPos.x, _endDragPos.x);
        var y = Mathf.Max(_startDragPos.y, _endDragPos.y) - Mathf.Min(_startDragPos.y, _endDragPos.y);
        var z = Mathf.Max(_startDragPos.z, _endDragPos.z) - Mathf.Min(_startDragPos.z, _endDragPos.z);
        Gizmos.DrawCube(Vector3.Lerp(_startDragPos, _endDragPos, 0.5f), new Vector3(x, y, z));
    }

    private void OnMousePositionChanged(Vector2 mousePos)
    {
        if (!_isStartRayHit) return;

        var ray = cursorCamera.ScreenPointToRay(mouseControl.MousePosition);
        if (!Physics.Raycast(ray, out var hit)) return;
        _endDragPos = hit.point;
    }

    private void DrawAreaToWorld(bool state)
    {
        var ray = cursorCamera.ScreenPointToRay(mouseControl.MousePosition);

        if (state)
        {
            if (!Physics.Raycast(ray, out var hit)) return;
            _startDragPos = hit.point;
            _endDragPos = hit.point;

            _isStartRayHit = true;
        }
        else
        {
            if (!_isStartRayHit) return;

            if (Physics.Raycast(ray, out var hit))
            {
                _endDragPos = hit.point;
            }
            else
            {
                var pos = cursorCamera.ScreenToWorldPoint(mouseControl.MousePosition);
                _startDragPos = pos;
                _endDragPos = pos;
            }

            _isStartRayHit = false;
        }
    }
}