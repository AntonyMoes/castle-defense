using System;
using UnityEngine;

namespace PlayerInteraction {
    public class Selectable : MonoBehaviour {
        [SerializeField] new Renderer renderer;
        public Action<bool> OnToggleSelection;
        public bool IsSelected { get; private set; }

        public void ToggleSelection(bool isSelected) {
            IsSelected = isSelected;
            renderer.enabled = IsSelected;
            OnToggleSelection?.Invoke(IsSelected);
        }
    }
}
