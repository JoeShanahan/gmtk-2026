using System.Collections.Generic;
using ProcGeo;
using UnityEngine;

public class AoeCircleGenerator : MonoBehaviour
{
    [SerializeField]
    private float _powerfulRange = 2;
    
    [SerializeField]
    private float _weakRange = 2;

    [SerializeField] private Material _weakMaterial;
    [SerializeField] private Material _powerfulMaterial;

    [SerializeField]
    protected MeshRenderer _meshRenderer;

    [SerializeField]
    protected MeshFilter _meshFilter;
    
    private Vector2 v2_0_0 = new Vector2(0, 0);
    private Vector2 v2_1_0 = new Vector2(1, 0);
    private Vector2 v2_0_1 = new Vector2(0, 1);
    private Vector2 v2_1_1 = new Vector2(1, 1);
    private Vector2 v2_05_1 = new Vector2(0.5f, 1);

    private void Start()
    {
        var tris = GetTris();
        Mesh mesh = GenerateMeshFromTriangleData(tris, out Material[] mats);

        _meshFilter.mesh = mesh;
        _meshRenderer.sharedMaterials = mats;
    }
    
    private List<TriangleData> GetTris()
    {
        var tris = new List<TriangleData>();
        Vector3 littleUp = new Vector3(0, 0.01f, 0);

        for (int i = 0; i < 32; i++)
        {
            float circA = (Mathf.PI * 2 / 32) * i;
            float circB = (Mathf.PI * 2 / 32) * (i + 1);

            Vector3 pointA = littleUp + new Vector3(Mathf.Cos(circA) * _powerfulRange, 0, Mathf.Sin(circA) * _powerfulRange);
            Vector3 pointB = littleUp + new Vector3(Mathf.Cos(circB) * _powerfulRange, 0, Mathf.Sin(circB) * _powerfulRange);

            Vector3 pointA2 = littleUp + new Vector3(Mathf.Cos(circA) * (_powerfulRange + _weakRange), 0,
                Mathf.Sin(circA) * (_powerfulRange + _weakRange));
            Vector3 pointB2 = littleUp + new Vector3(Mathf.Cos(circB) * (_powerfulRange + _weakRange), 0,
                Mathf.Sin(circB) * (_powerfulRange + _weakRange));
            
            tris.Add(new TriangleData()
            {
                material = _powerfulMaterial,
                verticies = new[] { pointA, littleUp, pointB},
                uvs =  new[] { v2_0_0, v2_05_1, v2_1_0 }
            });
            
            tris.Add(new TriangleData()
            {
                material = _weakMaterial,
                verticies = new[] { pointA2, pointA, pointB},
                uvs =  new[] { v2_0_0, v2_0_1, v2_1_1 }
            });
            
            tris.Add(new TriangleData()
            {
                material = _weakMaterial,
                verticies = new[] { pointB2, pointA2, pointB},
                uvs =  new[] { v2_1_0, v2_0_0, v2_1_1 }
            });
        }

        return tris;
    }
    
    private Mesh GenerateMeshFromTriangleData(IEnumerable<TriangleData> fullTriangleList, out Material[] materials)
    {
        Mesh myMesh = new Mesh();

        List<SubmeshData> submeshList = CreateSubmeshes(fullTriangleList);
        List<Vector3> fullVertexList = GenerateVerticies(submeshList, out List<Vector2> uvList);

        myMesh.SetVertices(fullVertexList);
        myMesh.SetUVs(0, uvList);

        myMesh.subMeshCount = submeshList.Count;

        foreach (SubmeshData submesh in submeshList)
            myMesh.SetTriangles(submesh.indicies, submesh.submeshIndex);

        myMesh.RecalculateNormals();
        materials = GetMaterialsForSubmeshes(submeshList);

        myMesh.name = "GeneratedMesh";
        return myMesh;
    }

    private Material[] GetMaterialsForSubmeshes(List<SubmeshData> submeshList)
    {
        var materialList = new Material[submeshList.Count];

        foreach (SubmeshData submesh in submeshList)
            materialList[submesh.submeshIndex] = submesh.material;

        return materialList;
    }

    private List<Vector3> GenerateVerticies(IEnumerable<SubmeshData> submeshList, out List<Vector2> uvs)
    {
        var fullVertexList = new List<Vector3>();
        uvs = new List<Vector2>();

        foreach (SubmeshData submesh in submeshList)
        {
            foreach (TriangleData dat in submesh.triangles)
            {
                int start = fullVertexList.Count;
                fullVertexList.AddRange(dat.verticies);
                uvs.AddRange(dat.uvs);

                for (int j = 0; j < 3; j++)
                    submesh.indicies.Add(start + j);
            }
        }

        return fullVertexList;
    }
    
    private List<SubmeshData> CreateSubmeshes(IEnumerable<TriangleData> fullTriangleList)
    {
        var submeshDict = new Dictionary<Material, SubmeshData>();

        foreach (TriangleData dat in fullTriangleList)
        {
            if (submeshDict.ContainsKey(dat.material) == false)
                submeshDict[dat.material] = new SubmeshData(submeshDict.Count, dat.material);

            submeshDict[dat.material].triangles.Add(dat);
        }

        return new List<SubmeshData>(submeshDict.Values);
    }
}

