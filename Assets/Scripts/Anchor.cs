using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class Anchor
{
        private float curDistance = 0;
        private GameObject line;
        private LineRenderer lr;
        private ARAnchor anchorRef;
        private const float MAXDistance = 1000;
        private DepthDetection depthDetection;
        private Camera camera;

        public Anchor(DepthDetection dD, Camera cam)
        {
                line = new GameObject();
                line.AddComponent<LineRenderer>();
                lr = line.GetComponent<LineRenderer>();
                ShowLine(false);
                depthDetection = dD;
                Camera camera = cam;
        }

        public void UpdateLine() //Update positions and draw the line
        {
                if (lr.enabled)
                {
                        Vector3 anchorLocation = anchorRef.transform.position;
                        var cameraLocation = camera.transform.position;
                        curDistance = GetDistance();
                        line.transform.position = anchorLocation;
                        lr.startColor = GetLineColor();
                        lr.startWidth = 0.2f;
                        lr.SetPosition(0, anchorLocation);
                        lr.SetPosition(1, cameraLocation);
                }
        }

        public float GetDistance() //Get distance by using depth data
        {
                Vector3 screenPoint = camera.WorldToScreenPoint(anchorRef.transform.position);
                int screenX = (int)screenPoint.x;
                int screenY = (int)screenPoint.y;
                return depthDetection.GetDepthFromXY(screenX, screenY);
        }

        public void ShowLine(bool enable)
        {
                lr.enabled = enable;
        }

        public Color GetLineColor()
        {
                Color newColor = Color.Lerp(Color.red, Color.blue, MAXDistance/curDistance);
                return newColor;
        }

        public void SetAnchor(ARAnchor anchor)
        {
                anchorRef = anchor;
                ShowLine(true);
        }

        public float LineDistance
        {
                get => curDistance;
        }
}