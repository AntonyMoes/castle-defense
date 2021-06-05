using System.Linq;
using Commands;
using UnityEngine;

namespace PlayerInteraction {
    public class PlayerMouseInteraction : MonoBehaviour {
        [SerializeField] new Camera camera;
        [SerializeField] RectTransform selectionBox;
        [SerializeField] LayerMask interactionMask;
        MouseInteraction _currentSelection;
        readonly SelectedList _selectedObjects = new SelectedList();
        Commandable[] _commandables = {};

        void Update() {
            // TODO: check for ui elements
            
            if (Input.GetButtonDown("Mouse Left")) {
                _currentSelection = new MouseInteraction(selectionBox, Input.mousePosition);
            }
            if (Input.GetButton("Mouse Left")) {
                _currentSelection.UpdateSelection(Input.mousePosition);
            }
            if (Input.GetButtonUp("Mouse Left")) {
                var addToSelection = Input.GetKey(KeyCode.LeftControl);
                _currentSelection.FinishSelection(addToSelection, Input.mousePosition, interactionMask, camera, _selectedObjects);
                _commandables = _selectedObjects
                    .Select(selectable => selectable.GetComponent<Commandable>())
                    .Where(commandable => commandable)
                    .ToArray();
            }
            
            if (Input.GetButtonDown("Mouse Right") && _commandables.Length != 0) {
                var target = MouseInteraction.TargetOne(Input.mousePosition, interactionMask, camera);
                foreach (var commandable in _commandables) {
                    commandable.AcquireCommand(target);
                }

                if (target is PointTarget pointTarget) {
                    pointTarget.Usages++;
                    pointTarget.Usages--;
                }
            }
        }
    }
}
