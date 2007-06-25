using System;
using System.Collections.Generic;
using System.Text;
using RayTracerFramework.Geometry;

namespace RayTracerFramework.PhotonMapping {
    class PhotonMap {
        private Node root;

        private class Node {
            public Node left;
            public Node right;
            public Photon photon;

            public Node(PhotonMap.Node left, PhotonMap.Node right, Photon photon) {
                this.left = left;
                this.right = right;
                this.photon = photon;
            }
        }

        public PhotonMap(Photon[] photons) {
            root = MakeTree(new List<Photon>(photons));
        }

        private PhotonMap.Node MakeTree(List<Photon> photons) {
            if (photons.Count == 0)
                return null;
            if(photons.Count == 1)
                return new PhotonMap.Node(null, null, photons[0]);
            Photon mid;
            Vec3 dimensions;
            FindMedianAndDimensions(photons, out mid, out dimensions);
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
            return new PhotonMap.Node(leftNode, rightNode, mid);
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

        private void FindMedianAndDimensions(
                List<Photon> photons, 
                out Photon median, 
                out Vec3 dimensions) {
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

        public Photon FindNearestPhoton(Vec3 position) {
            List<Photon> path = new List<Photon>();
            Dive(root, position, path);
            float result_positionSq = float.PositiveInfinity;
            Photon result = null;
            foreach (Photon photon in path) {
                float photon_positionSq = Vec3.GetLengthSq(photon.position - position);
                if (photon_positionSq < result_positionSq) {
                    result_positionSq = photon_positionSq;
                    result = photon;
                }
            }
            return result;
        }

        private void Dive(PhotonMap.Node node, Vec3 position, List<Photon> path) {
            path.Add(node.photon);
            switch (node.photon.flag) {
                case 0:
                    if (position.x <= node.photon.position.x && node.left != null)
                        Dive(node.left, position, path);
                    else if (node.right != null)
                        Dive(node.right, position, path);
                    break;
                case 1:
                    if (position.y <= node.photon.position.y && node.left != null)
                        Dive(node.left, position, path);
                    else if (node.right != null)
                        Dive(node.right, position, path);
                    break;
                case 2:
                    if (position.z <= node.photon.position.z && node.left != null)
                        Dive(node.left, position, path);
                    else if (node.right != null)
                        Dive(node.right, position, path);
                    break;
            }
        }

        //private Photon FindNearestPhoton(PhotonMap.Node node, Vec3 position) { 
        //    float node_positionSq = Vec3.GetLengthSq(node.photon.position - position);
        //    float left_positionSq = float.PositiveInfinity;
        //    if(node.left != null)
        //        left_positionSq = Vec3.GetLengthSq(node.left.photon.position - position);
        //    float right_positionSq = float.PositiveInfinity;
        //    if (node.right != null)
        //        right_positionSq = Vec3.GetLengthSq(node.right.photon.position - position);
        //    if (node_positionSq < left_positionSq)
        //        if (node_positionSq < right_positionSq)
        //            return node.photon;
        //        else
        //            return FindNearestPhoton(node.right, position);
        //    else if (left_positionSq < right_positionSq)
        //        return FindNearestPhoton(node.left, position);
        //    else
        //        return FindNearestPhoton(node.right, position);
        //}
    }
}