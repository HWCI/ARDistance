using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class Anchor
{
        private float curDistance = 0;
        private GameObject anchorObj;
        private LineRenderer lr;
        private ARAnchor anchorRef;
        private const float MAXDistance = 30000;
        private DepthDetection depthDetection;
        private Camera camera;
        private TextMesh txtMesh;
        private bool isLabelA;
        private GameObject labelObject;
        private Animation labelAnim;
        
        //Setup anchor object with LineRenderer and Text
        public Anchor(DepthDetection dD, Camera cam, bool isA, GameObject lO) 
        {
                anchorObj = new GameObject();
                anchorObj.AddComponent<LineRenderer>();
                lr = anchorObj.GetComponent<LineRenderer>();
                ShowLine(false);
                depthDetection = dD;
                camera = cam;
                isLabelA = isA;
                labelObject = Object.Instantiate(lO, lr.transform, true);
                labelObject.SetActive(false);
                CreateLineRenderer();
                CreateTextMesh();
                
        }

        private void CreateLineRenderer()
        {
                lr.startWidth = 0.01f;
                lr.endWidth = 0.01f;
                lr.useWorldSpace = true;
                lr.alignment = LineAlignment.View;
                Material mat = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                lr.material = mat;
        }

        private void CreateTextMesh()
        {
                txtMesh = anchorObj.AddComponent<TextMesh>();
                txtMesh.characterSize = 0.075f;
                txtMesh.fontSize = 32;
                txtMesh.offsetZ = 1;
        }

        //Update positions and draw the line
        public void UpdateLine() 
        {
                if (lr.enabled)
                {
                        Vector3 anchorLocation = anchorRef.transform.position;
                        var cameraLocation = camera.transform.position;
                        Vector3 lineEndLocation = new Vector3(cameraLocation.x, cameraLocation.y - 0.3f, cameraLocation.z);
                        curDistance = GetDistance(); 
                        anchorObj.transform.position = anchorLocation;
                        var color = GetLineColor();
                        lr.startColor = color;
                        lr.endColor = color;
                        lr.SetPosition(0, anchorLocation);
                        lr.SetPosition(1, lineEndLocation);
                        txtMesh.transform.rotation = Quaternion.LookRotation(txtMesh.transform.position - cameraLocation);
                        if (isLabelA)
                        {
                                txtMesh.text = "A=" + (int)(curDistance/100) + "cm";
                        }
                        else
                        {
                                txtMesh.text = "B="+ (int)(curDistance/100) + "cm";
                        }
                }
        }

        //Get distance by using depth data
        public float GetDistance() 
        {
                Vector3 screenPoint = camera.WorldToScreenPoint(anchorRef.transform.position);
                return depthDetection.GetDepthFromScreenPoint((int)screenPoint.x, (int)screenPoint.y);
        }

        public void ShowLine(bool enable)
        {
                lr.enabled = enable;
        }

        public Color GetLineColor()
        {
                Color newColor = Color.Lerp(Color.red, Color.green, curDistance/MAXDistance);
                return newColor;
        }

        //Clear and set new anchor
        public void SetAnchor(ARAnchor anchor)
        {
                if (anchorRef != null)
                {
                        Object.Destroy(anchorRef.gameObject);
                }
                anchorRef = anchor;
                ShowLine(true);
                labelObject.SetActive(true);
                labelObject.GetComponentInChildren<Animation>().Play();
        }
}