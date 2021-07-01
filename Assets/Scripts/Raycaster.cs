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
        public bool ProcessInput() //false = no new touch input
        {
                if (Input.touchCount == 0) return false;
                
                var touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Began) return false;
                
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon)) //Raycast touch location and pass ARAnchor
                {
                        var hitPose = hits[0].pose;
                        var hitTrackableId = hits[0].trackableId;
                        var hitPlane = planeManager.GetPlane(hitTrackableId);
                        ARAnchor anchor = anchorManager.AttachAnchor(hitPlane, hitPose);
                        if (anchor != null)
                        {
                                hitCount++;
                                callback(anchor, hitCount % 2);
                                return true;
                        }
                }
                return false;
        }
        
        
}