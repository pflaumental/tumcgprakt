using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.PhotonMapping {
    public class PhotonMap {

        // Related assignement: 8.3.a

        private Node root;

        private class Node {
            public Node left;
            public Node right;
            public Photon photon;
            public BSphere bSphere; // Encloses the space represented by this photon in the kd-tree

            public Node(PhotonMap.Node left, PhotonMap.Node right, Photon photon, BSphere bSphere) {
                this.left = left;
                this.right = right;
                this.photon = photon;
                this.bSphere = bSphere;
            }
        }

        public PhotonMap(Photon[] photons) {
            root = MakeTree(new List<Photon>(photons));
        }

        private PhotonMap.Node MakeTree(List<Photon> photons) {
            if (photons.Count == 0)
                return null;
            if(photons.Count == 1)
                return new PhotonMap.Node(null, null, photons[0], new BSphere(photons[0].position, 0f));
            Photon mid;
            Vec3 dimensions;
            BSphere nodeBSphere;
            FindMedianDimensionsAndBoundingSphere(photons, out mid, out dimensions, out nodeBSphere);
            photons.Remove(mid);
            if (dimensions.x >= dimensions.y)
                if (dimensions.x >= dimensions.z)
                    mid.flag = 0;
                else
                    mid.flag = 2;
            else if (dimensions.y >= dimensions.z)
                mid.flag = 1;
            else
                mid.flag = 2;
            List<Photon> leftPhotons;
            List<Photon> rightPhotons;
            Split(photons, mid, out leftPhotons, out rightPhotons);
            PhotonMap.Node leftNode = MakeTree(leftPhotons);
            PhotonMap.Node rightNode = MakeTree(rightPhotons);
            return new PhotonMap.Node(leftNode, rightNode, mid, nodeBSphere);
        }

        private void Split(
                List<Photon> photons, 
                Photon mid, 
                out List<Photon> left, 
                out List<Photon> right) {
            left = new List<Photon>();
            right = new List<Photon>();
            float midPos;
            switch (mid.flag) { 
                case 0: // x
                    midPos = mid.position.x;
                    foreach (Photon photon in photons) {
                        if (photon.position.x < midPos)
                            left.Add(photon);
                        else
                            right.Add(photon);
                    }
                    break;
                case 1: // y
                    midPos = mid.position.y;
                    foreach (Photon photon in photons) {
                        if (photon.position.y < midPos)
                            left.Add(photon);
                        else
                            right.Add(photon);
                    }
                    break;
                case 2: // z
                    midPos = mid.position.z;
                    foreach (Photon photon in photons) {
                        if (photon.position.z < midPos)
                            left.Add(photon);
                        else
                            right.Add(photon);
                    }
                    break;
            }
        }

        private void FindMedianDimensionsAndBoundingSphere(
                List<Photon> photons, 
                out Photon median, 
                out Vec3 dimensions,
                out BSphere boundingSphere) {
            Vec3 center = photons[0].position;
            Vec3 leftBottomFront = new Vec3(center.x, center.y, center.z);
            Vec3 rightTopBack = new Vec3(center.x, center.y, center.z);
            for(int i=1; i<photons.Count; i++) {
                float j = i + 1f;
                Vec3 pos = photons[i].position;
                center = center * ((float)i / j) + pos * (1f / j);
                if (pos.x < leftBottomFront.x)
                    leftBottomFront.x = pos.x;
                else if (pos.x > rightTopBack.x)
                    rightTopBack.x = pos.x;
                if (pos.y < leftBottomFront.y)
                    leftBottomFront.y = pos.y;
                else if (pos.y > rightTopBack.y)
                    rightTopBack.y = pos.y;
                if (pos.z < leftBottomFront.z)
                    leftBottomFront.z = pos.z;
                else if (pos.z > rightTopBack.z)
                    rightTopBack.z = pos.z;
            }
            dimensions = rightTopBack - leftBottomFront;
            boundingSphere = new BSphere(center, Vec3.GetLength(rightTopBack - center));
            median = photons[0];
            float median_centerSq = Vec3.GetLengthSq(center - median.position);
            for (int i = 1; i < photons.Count; i++) {
                Photon photon = photons[i];
                float photon_centerSq = Vec3.GetLengthSq(center - photon.position);
                if (photon_centerSq < median_centerSq) {
                    median_centerSq = photon_centerSq;
                    median = photon;
                }
            }
        }

        public Photon FindNearestPhoton(Vec3 position, out float distToNNSq) {
            distToNNSq = float.PositiveInfinity;
            return FindNearestPhoton(root, position, ref distToNNSq);
        }

        private Photon FindNearestPhoton(PhotonMap.Node node, Vec3 position, ref float distToNNSq) {
            if (node == null)
                return null;

            Photon result = null;
            float distToNodeSq = Vec3.GetLengthSq(position - node.photon.position);
            if (distToNodeSq < distToNNSq) {
                distToNNSq = distToNodeSq;
                result = node.photon;
            }
            float toPlane = 0f;
            switch (node.photon.flag) {
                case 0: // x
                    toPlane = node.photon.position.x - position.x;
                    break;
                case 1: // y
                    toPlane = node.photon.position.y - position.y;
                    break;
                case 2: // z
                    toPlane = node.photon.position.z - position.z;
                    break;
            }

            float toPlaneSq = toPlane * toPlane;
            if (toPlane > 0) { // position in left half space
                result = FindNearestPhoton(node.left, position, ref distToNNSq);
                if(toPlaneSq < distToNNSq)
                    result = FindNearestPhoton(node.right, position, ref distToNNSq);
            } else { // position in right half space
                result = FindNearestPhoton(node.right, position, ref distToNNSq);
                if(toPlaneSq < distToNNSq)
                    result = FindNearestPhoton(node.left, position, ref distToNNSq);
            }

            return result;
        }

        public List<PhotonDistanceSqPair> FindPhotonsInSphere(Vec3 center) {
            List<PhotonDistanceSqPair> result = new List<PhotonDistanceSqPair>();
            FindPhotonsInSphere(root, center, result);
            return result;
        }

        private void FindPhotonsInSphere(
                PhotonMap.Node node, 
                Vec3 center, 
                List<PhotonDistanceSqPair> result) {
            if (node == null)
                return;

            float distToNodeSq = Vec3.GetLengthSq(center - node.photon.position);
            if (distToNodeSq < Settings.Render.PhotonMapping.SphereRadiusSq) {
                result.Add(new PhotonDistanceSqPair(node.photon, distToNodeSq));
            }
            float toPlane = 0f;
            switch (node.photon.flag) {
                case 0: // x
                    toPlane = node.photon.position.x - center.x;
                    break;
                case 1: // y
                    toPlane = node.photon.position.y - center.y;
                    break;
                case 2: // z
                    toPlane = node.photon.position.z - center.z;
                    break;
            }

            float toPlaneSq = toPlane * toPlane;

            if (toPlane > 0 || toPlaneSq < Settings.Render.PhotonMapping.SphereRadiusSq)
                FindPhotonsInSphere(node.left, center, result);
            if (toPlane <= 0 || toPlaneSq < Settings.Render.PhotonMapping.SphereRadiusSq)
                FindPhotonsInSphere(node.right, center, result);
        }

        public List<PhotonDistanceSqPair> FindPhotonsAlongRay(Ray ray, float length) {
            List<PhotonDistanceSqPair> result = new List<PhotonDistanceSqPair>();
            if(length >= 2 * Settings.Render.PhotonMapping.CapsuleRadius)
                FindPhotonsInCapsule(
                        root,
                        ray.position + ray.direction * Settings.Render.PhotonMapping.CapsuleRadius, 
                        ray.direction,
                        length - 2 * Settings.Render.PhotonMapping.CapsuleRadius,
                        ray.position + ray.direction * (length - Settings.Render.PhotonMapping.CapsuleRadius),
                        result);
            return result;
        }

        private void FindPhotonsInCapsule(
                PhotonMap.Node node,
                Vec3 lineStart,
                Vec3 direction,
                float length,
                Vec3 lineEnd,
                List<PhotonDistanceSqPair> result) {
            if (node == null)
                return;
            Vec3 StN = node.photon.position - lineStart;
            float StNonRay = Vec3.Dot(direction, StN);
            Vec3 nearestPoint;
            if (StNonRay < 0f)
                nearestPoint = lineStart;
            else if (StNonRay > length)
                nearestPoint = lineEnd;
            else
                nearestPoint = lineStart + direction * StNonRay;
            float distToNodeSq = Vec3.GetLengthSq(nearestPoint - node.photon.position);
            if (distToNodeSq < Settings.Render.PhotonMapping.CapsuleRadiusSq) {
                result.Add(new PhotonDistanceSqPair(node.photon, distToNodeSq));
            }

            float toPlane = 0f;
            float lineStartToPlane = 0f;
            float lineEndToPlane = 0f;
            switch (node.photon.flag) {
                case 0: // x
                    lineStartToPlane = lineStart.x - node.photon.position.x;
                    lineEndToPlane = lineEnd.x - node.photon.position.x;
                    break;
                case 1: // y
                    lineStartToPlane = lineStart.y - node.photon.position.y;
                    lineEndToPlane = lineEnd.y - node.photon.position.y;
                    break;
                case 2: // z
                    lineStartToPlane = lineStart.z - node.photon.position.z;
                    lineEndToPlane = lineEnd.z - node.photon.position.z;
                    break;
            }
            if ((lineStartToPlane > 0 && lineEndToPlane < 0) // different signs
                    || (lineStartToPlane < 0 && lineEndToPlane > 0))
                toPlane = 0f; // => line intersects plane
            else { // same signs
                if(lineStartToPlane > 0)
                    toPlane = Math.Min(lineStartToPlane, lineEndToPlane);
                else
                    toPlane = Math.Max(lineStartToPlane, lineEndToPlane);
            }

            bool goLeft = false;
            bool goRight = false;
            if (toPlane <= 0 || toPlane < Settings.Render.PhotonMapping.CapsuleRadius) {
                if (IsCapsuleIntersectingNodeBoundingSphere(
                        lineStart,
                        direction,
                        length,
                        lineEnd,
                        node.bSphere))
                    goLeft = true;
            }
            if (toPlane > 0 || toPlane > -Settings.Render.PhotonMapping.CapsuleRadius) {
                if (IsCapsuleIntersectingNodeBoundingSphere(
                        lineStart,
                        direction,
                        length,
                        lineEnd,
                        node.bSphere))
                    goRight = true;
            }

            if (goLeft)
                FindPhotonsInCapsule(
                        node.left, 
                        lineStart, 
                        direction, 
                        length, 
                        lineEnd,
                        result);
            if (goRight)
                FindPhotonsInCapsule(
                        node.right,
                        lineStart,
                        direction,
                        length,
                        lineEnd,
                        result);
        }

        private bool IsCapsuleIntersectingNodeBoundingSphere(                
                Vec3 lineStart,
                Vec3 direction,
                float length,
                Vec3 lineEnd,
                BSphere boundingSphere) {
            Vec3 StC = boundingSphere.center - lineStart;
            float StNonRay = Vec3.Dot(direction, StC);
            Vec3 nearestPoint;
            if (StNonRay < 0f)
                nearestPoint = lineStart;
            else if (StNonRay > length)
                nearestPoint = lineEnd;
            else
                nearestPoint = lineStart + direction * StNonRay;
            float distCenterLineSq = Vec3.GetLengthSq(nearestPoint - boundingSphere.center);
            float maxDistance = (boundingSphere.radius + Settings.Render.PhotonMapping.CapsuleRadius);
            return distCenterLineSq < (maxDistance * maxDistance);
        }  
    }
}
