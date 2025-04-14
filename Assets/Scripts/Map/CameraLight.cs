using UnityEngine;
using UnityEngine.Rendering;

public class CameraLight : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera; // Minimap camera reference
    [SerializeField] private Light minimapLight;  // Light to be used only for the minimap

    void OnEnable()
    {
        // Subscribe to rendering events
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDisable()
    {
        // Unsubscribe when object is disabled
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    void OnBeginCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        // Enable the light only when the minimap camera is rendering
        if (cam == minimapCamera)
        {
            minimapLight.enabled = true;
        }
    }

    void OnEndCameraRendering(ScriptableRenderContext context, Camera cam)
    {
        // Disable the light after the minimap camera finishes rendering
        if (cam == minimapCamera)
        {
            minimapLight.enabled = false;
        }
    }
}
