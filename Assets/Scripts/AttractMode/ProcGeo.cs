using System.Collections.Generic;
using UnityEngine;

namespace ProcGeo
{
    public class SubmeshData
    {
        public List<TriangleData> triangles = new List<TriangleData>();
        public List<int> indicies = new List<int>();
        public Material material;
        public int submeshIndex;

        public SubmeshData(int idx, Material mat)
        {
            material = mat;
            submeshIndex = idx;
        }
    }
    
    public class TriangleData
    {
        public Vector3[] verticies = new Vector3[3];
        public Vector2[] uvs = new Vector2[3];
        public Material material;
    }
}