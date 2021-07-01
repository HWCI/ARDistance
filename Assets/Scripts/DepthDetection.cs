using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DepthDetection
{
    private Texture2D depthTexture;
    private short[] depthArray;
    private int depthWidth;
    private int depthHeight;
    private AROcclusionManager occlusionManager;

    public DepthDetection(AROcclusionManager aROM)
    {
        occlusionManager = aROM;
    }
    
    public void UpdateDepthImage()
    {
        if (occlusionManager.TryAcquireEnvironmentDepthCpuImage(out XRCpuImage image))
        {
            using (image)
            {
                UpdateTexture(image);
                depthWidth = image.width;
                depthHeight = image.height;
            }
        }
    }


    public float GetDepthFromXY(int x, int y) // Obtain the depth value in meters at the specified x, y location.
    {
       if (x >= depthWidth || x < 0 || y >= depthHeight || y < 0)
       {
           return 0;
       }
        int depthIndex = (y * depthWidth) + x;
        short shortDepth = depthArray[depthIndex];
        int depthInCm = shortDepth * 10;
        return depthInCm;
    }
    
    public void UpdateTexture(XRCpuImage cpuImage) //Formats the DepthImage to be processed
    {
        if (depthTexture == null || depthTexture.width != cpuImage.width || depthTexture.height != cpuImage.height)
        {
            depthTexture = new Texture2D(cpuImage.width, cpuImage.height, cpuImage.format.AsTextureFormat(), false);
            
            //Necessary ?
            var conversionParams = new XRCpuImage.ConversionParams(cpuImage, cpuImage.format.AsTextureFormat(),
                XRCpuImage.Transformation.MirrorY);

            // Get the Texture2D's underlying pixel buffer.
            var rawTextureData = depthTexture.GetRawTextureData<byte>();

            // Perform the conversion.
            cpuImage.Convert(conversionParams, rawTextureData);

            // "Apply" the new pixel data to the Texture2D.
            depthTexture.Apply();
            
            byte[] byteBuffer = depthTexture.GetRawTextureData();
            Buffer.BlockCopy(byteBuffer, 0, depthArray, 0, byteBuffer.Length);
        }

    }

}