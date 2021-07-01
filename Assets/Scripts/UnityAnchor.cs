using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class UnityAnchor : MonoBehaviour
{
    private Anchor labelA;
    private Anchor labelB;
    private LabelUI ui;
    private Raycaster raycaster;
    private DepthDetection depthDetector;
    public Camera ARCamera;

    public void Awake()
    {
        labelA = new Anchor(depthDetector, ARCamera);
        labelB = new Anchor(depthDetector, ARCamera);
        ui = new LabelUI();
        raycaster = new Raycaster(GetComponent<ARRaycastManager>(), GetComponent<ARAnchorManager>(), GetComponent<ARPlaneManager>(), SetAnchor);
        depthDetector = new DepthDetection(GetComponent<AROcclusionManager>());
    }

    public void Update()
    {
        depthDetector.UpdateDepthImage();
        raycaster.ProcessInput();
        labelA.UpdateLine();
        labelB.UpdateLine();
        ui.UpdateUI(labelA.LineDistance, labelB.LineDistance);
    }

    public void SetAnchor(ARAnchor anchor, int label)
    {
        if (label == 1)
        {
            labelA.SetAnchor(anchor);
        }
        else
        {
            labelB.SetAnchor(anchor);
        }
    }

    
}