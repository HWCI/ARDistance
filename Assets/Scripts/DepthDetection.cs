using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DepthDetection
{ 
    private int depthWidth;
    private int depthHeight;
    private AROcclusionManager occlusionManager;
    private byte[] depthBuffer = new byte[0];
    public short[] depthArray = new short[0];
    private Matrix4x4 depthDisplayMatrix = Matrix4x4.identity;


    public DepthDetection(AROcclusionManager aROM)
    {
        occlusionManager = aROM;
    }
    
    //Get the newest depth image and copy to buffer
    public void UpdateDepthImage(ARCameraFrameEventArgs args)
    {
        depthDisplayMatrix = args.displayMatrix.GetValueOrDefault();
        if (occlusionManager && occlusionManager.TryAcquireEnvironmentDepthCpuImage(out XRCpuImage image))
        {
            using (image)
            {
                depthWidth = image.width;
                depthHeight = image.height;
                int numPixels = image.width * image.height;
                int numBytes = numPixels * image.GetPlane(0).pixelStride;

                if (depthBuffer.Length != numBytes)
                {
                    depthBuffer = new byte[numBytes];
                }

                image.GetPlane(0).data.CopyTo(depthBuffer);
                image.Dispose();
            }
        }
    }
    
    //Send buffer to array
    public void UpdateDepthArray()
    {
        
        if (depthBuffer == null) return;
        int bufferLength = depthWidth * depthHeight;
        if (depthArray.Length != bufferLength)
        {
            depthArray = new short[bufferLength];
        }
        Buffer.BlockCopy(depthBuffer, 0, depthArray, 0, depthBuffer.Length);
    }

    //Get depth data from x,y value on the depth array
    public float GetDepthFromXY(int x, int y)
    {
        UpdateDepthArray();
        if (x >= depthWidth || x < 0 || y >= depthHeight || y < 0) return 0;
        var depthIndex = (y * depthWidth) + x;
        var depthInShort = depthArray[depthIndex];
        var depthInMeters = depthInShort * 10;
        return depthInMeters;
    }
    
    //Translate screen point to depth image x,y
    public float GetDepthFromScreenPoint(int x, int y)
    {
        Vector2 uv = new Vector2(x / (float)(Screen.width - 1), y / (float)(Screen.height - 1));
        uv = ScreenToDepthUV(uv);
        x = (int)(uv.x * (depthWidth - 1));
        y = (int)(uv.y * (depthHeight - 1));
        return GetDepthFromXY(x, y);
    }
    
    public Vector2 ScreenToDepthUV(Vector2 uv)
    {
        Vector4 transformed;
#if UNITY_ANDROID
        transformed = depthDisplayMatrix * new Vector4(uv.x, 1 - uv.y, 1f, 0f); //Reorient the depth image to screen
#endif
#if UNITY_IOS
        return uv;
#endif
        return new Vector2(transformed.x, transformed.y);

    }

}