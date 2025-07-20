using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class UIPanel : MonoBehaviour
{
    private static readonly Vector2 STANDARD_RESOLUTION = new Vector2(640, 360);

    private void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("UI");

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = STANDARD_RESOLUTION;
        scaler.matchWidthOrHeight = 0.5f;
    }
}