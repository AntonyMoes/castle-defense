using UnityEngine;

public class PlayerSelection : MonoBehaviour {
    [SerializeField] new Camera camera;
    [SerializeField] RectTransform selectionBox;
    [SerializeField] LayerMask selectionMask;
    MouseSelection _currentSelection;
    SelectedList _selectedObjects = new SelectedList();

    void Update() {
        if (Input.GetButtonDown("Mouse Left")) {
            _currentSelection = new MouseSelection(selectionBox, Input.mousePosition);
        }
        if (Input.GetButton("Mouse Left")) {
            _currentSelection.UpdateSelection(Input.mousePosition);
        }
        if (Input.GetButtonUp("Mouse Left")) {
            _currentSelection.FinishSelection(Input.mousePosition, selectionMask, camera, ref _selectedObjects);
        }
    }
}
