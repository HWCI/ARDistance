using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Raycaster
{
        ARRaycastManager raycastManager;
        ARAnchorManager anchorManager;
        ARPlaneManager planeManager;
        private Action<ARAnchor, int> callback;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();
        private int hitCount;

        public Raycaster(ARRaycastManager rcM, ARAnchorManager aM, ARPlaneManager pM, Action<ARAnchor, int> onHit)
        {
                raycastManager = rcM;
                anchorManager = aM;
                planeManager = pM;
                callback = onHit;
        }
        
        //Check for inputs
        public void ProcessInput() 
        {
                if (Input.touchCount == 0)
                        return;
                
                var touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Began)
                        return;

                // Raycast against planes and feature points
                const TrackableType trackableTypes =
                        TrackableType.FeaturePoint |
                        TrackableType.PlaneWithinPolygon;

                // Perform the raycast
                if (raycastManager.Raycast(touch.position, hits, trackableTypes))
                {
                        // Raycast hits are sorted by distance, so the first one will be the closest hit.
                        var hit = hits[0];

                        // Create a new anchor
                        var anchor = CreateAnchor(hit);
                        if (anchor)
                        {
                                Debug.Log("Created anchor " + hitCount);
                                hitCount++;
                                callback(anchor, hitCount % 2);
                        }
                        else
                        {
                                Debug.Log("Error creating anchor");
                        }
                }
        }
        
        //Create an ARanchor on raycast hit position
        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
                ARAnchor anchor = null;
                if (hit.trackable is ARPlane plane)
                {
                        if (planeManager)
                        {
                                anchor = anchorManager.AttachAnchor(plane, hit.pose);
                                return anchor;
                        }
                }
                var gameObject = new GameObject();
                gameObject.transform.position = hit.pose.position;
                gameObject.transform.rotation = hit.pose.rotation;
                anchor = gameObject.AddComponent<ARAnchor>();
                return anchor;
        }
        
        
}