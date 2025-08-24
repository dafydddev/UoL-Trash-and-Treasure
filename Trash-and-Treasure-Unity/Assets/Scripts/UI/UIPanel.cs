using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIPanel : MonoBehaviour
    {
        // Target resolution for the game, 640x360
        private static readonly Vector2 StandardResolution = new(640, 360);
        // The first button on the UI, setting focus onto it, helps with keyboard navigation
        [SerializeField] private Button firstButton;

        // Called when adding the script or resetting it 
        private void Reset()
        {
            // Delegate to SetupCanvas to make sure UIPanel elements have consistent settings
            SetupCanvas();
        }

        private void SetupCanvas()
        {
            // Set the layer to UI
            gameObject.layer = LayerMask.NameToLayer("UI");
            // Get the canvas (required by RequireComponent)
            var canvas = GetComponent<Canvas>();
            // Set the render mode to screen space overlay
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // Get the CanvasScaler (required by RequireComponent)
            var scaler = GetComponent<CanvasScaler>();
            // Set the canvas scaler to scale with screen size
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            // Set the reference resolution to the standard resolution
            scaler.referenceResolution = StandardResolution; 
            // Set match width or height to 0.5f for balanced scaling between width and height
            scaler.matchWidthOrHeight = 0.5f;
        }
    
        public void SetActive(bool isActive)
        {
            // Set the active state of the game object
            gameObject.SetActive(isActive);
            // If the game object is active and the first button is not null, set focus onto it
            if (isActive && firstButton)
            {
                firstButton.Select();
            }
        }
    }
}