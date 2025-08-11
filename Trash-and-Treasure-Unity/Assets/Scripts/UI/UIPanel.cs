using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIPanel : MonoBehaviour
    {
        private static readonly Vector2 StandardResolution = new Vector2(640, 360);

        [SerializeField] private Button firstButton;

        private void Reset()
        {
            SetupCanvas();
        }

        private void SetupCanvas()
        {
            gameObject.layer = LayerMask.NameToLayer("UI");
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = StandardResolution;
            scaler.matchWidthOrHeight = 0.5f;

        }
    
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}