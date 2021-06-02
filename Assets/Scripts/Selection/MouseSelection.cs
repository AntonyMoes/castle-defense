using UnityEngine;

class MouseSelection {
    const float DeltaPixels = 5;
    
    readonly RectTransform _selectionBox;
    readonly Vector2 _startMousePosition;
    bool _finished;

    public MouseSelection(RectTransform selectionBox, Vector2 startMousePosition) {
        _selectionBox = selectionBox;
        _startMousePosition = startMousePosition;
        _selectionBox.gameObject.SetActive(true);
            
        UpdateSelection(_startMousePosition);
    }

    public void UpdateSelection(Vector2 mousePosition) {
        if (_finished) {
            return;
        }

        var delta = mousePosition - _startMousePosition;
        _selectionBox.sizeDelta = new Vector2(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        _selectionBox.anchoredPosition = _startMousePosition + delta / 2;
    }

    public void FinishSelection(Vector2 finalMousePosition, LayerMask selectionMask, Camera camera, ref SelectedList selectedObjects) {
        if (_finished) {
            return;
        }
            
        _finished = true;
        _selectionBox.gameObject.SetActive(false);
        selectedObjects.Clear();

        if ((finalMousePosition - _startMousePosition).magnitude < DeltaPixels) {
            SelectOne(finalMousePosition, selectionMask, camera, ref selectedObjects);
        } else {
            SelectMany(selectionMask, camera, ref selectedObjects);
        }
    }

    void SelectMany(LayerMask selectionMask, Camera camera, ref SelectedList selectedObjects) {
        var (pos, size) = GetWorldValues(camera, _selectionBox.anchoredPosition, _selectionBox.sizeDelta);
        var selectionBounds = new Bounds(pos, size);

        var selected = Physics2D.BoxCastAll(pos, size, 0, Vector2.zero, 0, selectionMask);
        foreach (var hit2D in selected) {
            var obj = hit2D.transform;
            if (!selectionBounds.Contains(obj.position)) {
                continue;
            }

            var res = TryGetSelectable(obj, out var selectable);
            if (!res) {
                continue;
            }
            
            selectedObjects.Add(selectable);
        }
    }

    static void SelectOne(Vector2 mousePosition, LayerMask selectionMask, Camera camera, ref SelectedList selectedObjects) {
        var (pos, size) = GetWorldValues(camera, mousePosition, new Vector2(DeltaPixels, DeltaPixels));
        
        var selected = Physics2D.BoxCastAll(pos, size, 0, Vector2.zero, 0, selectionMask);
        foreach (var hit2D in selected) {
            var res = TryGetSelectable(hit2D.transform, out var selectable);
            if (!res) {
                continue;
            }
        
            selectedObjects.Add(selectable);
            return;
        }
    }

    static (Vector2, Vector2) GetWorldValues(Camera camera, Vector2 screenPosition, Vector2 screenFrameSize) {
        Vector2 pos = camera.ScreenToWorldPoint(screenPosition);
        var size = (Vector2) camera.ScreenToWorldPoint(screenPosition + screenFrameSize) - pos;
        return (pos, size);
    }

    static bool TryGetSelectable(Transform obj, out Selectable selectable) {
        if (obj.parent) {
            return obj.parent.TryGetComponent(out selectable);
        }

        selectable = null;
        return false;
    }
}
