using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class UnityAnchor : MonoBehaviour
{
    private Anchor labelA;
    private Anchor labelB;
    private Raycaster raycaster;
    private DepthDetection depthDetector;
    private ARCameraManager arCameraManager;
    public Camera ARCamera;

    public void Awake()
    {
        
        raycaster = new Raycaster(GetComponent<ARRaycastManager>(), GetComponent<ARAnchorManager>(), GetComponent<ARPlaneManager>(), SetAnchor);
        depthDetector = new DepthDetection(GetComponent<AROcclusionManager>());
        arCameraManager = ARCamera.GetComponent<ARCameraManager>();
        arCameraManager.frameReceived += depthDetector.UpdateDepthImage;
        labelA = new Anchor(depthDetector, ARCamera, true);
        labelB = new Anchor(depthDetector, ARCamera, false);
        Debug.Log("Initialize successful");
    }

    public void Update()
    {
        raycaster.ProcessInput();
        labelA.UpdateLine();
        labelB.UpdateLine();
    }

    public void SetAnchor(ARAnchor anchor, int label)
    {
        if (label == 0)
        {
            labelA.SetAnchor(anchor);
        }
        else
        {
            labelB.SetAnchor(anchor);
        }
    }

    
}