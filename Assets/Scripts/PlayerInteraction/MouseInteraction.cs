using Commands;
using UnityEngine;

namespace PlayerInteraction {
    class MouseInteraction {
        const float DeltaPixels = 5;
    
        readonly RectTransform _selectionBox;
        readonly Vector2 _startMousePosition;
        bool _finished;

        public MouseInteraction(RectTransform selectionBox, Vector2 startMousePosition) {
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

        public void FinishSelection(bool addToSelection, Vector2 finalMousePosition, LayerMask interactionMask, Camera camera,
            SelectedList selectedObjects) {
            if (_finished) {
                return;
            }

            _finished = true;
            _selectionBox.gameObject.SetActive(false);
            if (!addToSelection) {
                selectedObjects.Clear();
            }

            if ((finalMousePosition - _startMousePosition).magnitude < DeltaPixels) {
                SelectOne(finalMousePosition, interactionMask, camera, selectedObjects);
            } else {
                SelectMany(interactionMask, camera, selectedObjects);
            }
        }

        void SelectMany(LayerMask interactionMask, Camera camera, SelectedList selectedObjects) {
            var (pos, size) = GetWorldValues(camera, _selectionBox.anchoredPosition, _selectionBox.sizeDelta);
            var selectionBounds = new Bounds(pos, size);

            var selected = Physics2D.BoxCastAll(pos, size, 0, Vector2.zero, 0, interactionMask);
            foreach (var hit2D in selected) {
                var obj = hit2D.transform;
                if (!selectionBounds.Contains(obj.position)) {
                    continue;
                }

                var res = TryGetComponent<Selectable>(obj, out var selectable);
                if (!res) {
                    continue;
                }
            
                selectedObjects.Add(selectable);
            }
        }

        static void SelectOne(Vector2 mousePosition, LayerMask interactionMask, Camera camera, SelectedList selectedObjects) {
            var (pos, size) = GetWorldValues(camera, mousePosition, new Vector2(DeltaPixels, DeltaPixels));
            var selectable = GetFirstWithComponent<Selectable>(pos, size, interactionMask);
            if (selectable != null) {
                selectedObjects.Add(selectable);
            }
        }

        public static CommandTargetable TargetOne(Vector2 mousePosition, LayerMask interactionMask, Camera camera) {
            var (pos, size) = GetWorldValues(camera, mousePosition, new Vector2(DeltaPixels, DeltaPixels));
            var targetable = GetFirstWithComponent<CommandTargetable>(pos, size, interactionMask);
            if (targetable != null) {
                return targetable;
            }

            var point = new GameObject();
            point.transform.position = pos;
            var pointTarget = point.AddComponent<PointTarget>();

            return pointTarget;
        }

        static TComponent GetFirstWithComponent<TComponent>(Vector2 position, Vector2 areaSize, LayerMask interactionMask) where TComponent : class {
            var selected = Physics2D.BoxCastAll(position, areaSize, 0, Vector2.zero, 0, interactionMask);
            foreach (var hit2D in selected) {
                var res = TryGetComponent<TComponent>(hit2D.transform, out var component);
                if (!res) {
                    continue;
                }
                
                return component;
            }

            return null;
        }

        static (Vector2, Vector2) GetWorldValues(Camera camera, Vector2 screenPosition, Vector2 screenFrameSize) {
            Vector2 pos = camera.ScreenToWorldPoint(screenPosition);
            var size = (Vector2) camera.ScreenToWorldPoint(screenPosition + screenFrameSize) - pos;
            return (pos, size);
        }

        static bool TryGetComponent<TComponent>(Transform obj, out TComponent component) where TComponent: class {
            if (obj.parent) {
                return obj.parent.TryGetComponent(out component);
            }

            component = null;
            return false;
        }
    }
}
