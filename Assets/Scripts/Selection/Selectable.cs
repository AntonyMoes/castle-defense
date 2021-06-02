using UnityEngine;

public class Selectable : MonoBehaviour {
    [SerializeField] new Renderer renderer;

    public void ToggleSelection(bool isActive) {
        renderer.enabled = isActive;
    }
}
