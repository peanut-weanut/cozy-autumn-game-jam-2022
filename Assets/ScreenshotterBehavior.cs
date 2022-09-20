using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenshotterBehavior : MonoBehaviour
{
    public Camera[] screenshottingCameras;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Texture2D RenderPipelineManager_endCameraRendering(ScriptableRenderContext cxt,Camera cam ){
        int height = cam.pixelHeight;
        int width = cam.pixelWidth;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();
        return screenshotTexture;
    }

        

}
