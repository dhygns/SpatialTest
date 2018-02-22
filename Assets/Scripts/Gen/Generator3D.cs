using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Generator3D : MonoBehaviour
{

    int detail = 20; // target mul

    // Use this for initialization
    void Start()
    {

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>() as MeshFilter;
        Mesh mesh = meshFilter.mesh;


        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();


        //Vertices & Coords
        for (int idx = 0; idx <= detail; idx++)
        {
            //Vertex
            float w = -0.5f + (float)idx / (float)detail;

            Vector3 vertTopFront = new Vector3(w, 0.5f, 0.02f);
            Vector3 vertBotFront = new Vector3(w, -0.5f, 0.02f);
            Vector3 vertTopRear = new Vector3(w, 0.5f, -0.02f);
            Vector3 vertBotRear = new Vector3(w, -0.5f, -0.02f);

            //UV
            Vector2 uvTop = new Vector2(0.5f + w, 0.0f);
            Vector2 uvBot = new Vector3(0.5f + w, 1.0f);

            vertices.Add(vertTopFront);
            vertices.Add(vertBotFront);
            vertices.Add(vertTopRear);
            vertices.Add(vertBotRear);

            uvs.Add(uvTop);
            uvs.Add(uvBot);
            uvs.Add(uvTop);
            uvs.Add(uvBot);

        }

        //Faces (indices)
        for (int idx = 0; idx < detail; idx++)
        {
            //Front
            triangles.Add(idx * 4 + 0);
            triangles.Add(idx * 4 + 1);
            triangles.Add(idx * 4 + 4);
            triangles.Add(idx * 4 + 4);
            triangles.Add(idx * 4 + 1);
            triangles.Add(idx * 4 + 5);

            //Rear
            triangles.Add(idx * 4 + 0 + 2);
            triangles.Add(idx * 4 + 4 + 2);
            triangles.Add(idx * 4 + 1 + 2);
            triangles.Add(idx * 4 + 5 + 2);
            triangles.Add(idx * 4 + 1 + 2);
            triangles.Add(idx * 4 + 4 + 2);

            //Top
            triangles.Add(idx * 4 + 0 + 2);
            triangles.Add(idx * 4 + 0 + 0);
            triangles.Add(idx * 4 + 4 + 2);
            triangles.Add(idx * 4 + 4 + 2);
            triangles.Add(idx * 4 + 0 + 0);
            triangles.Add(idx * 4 + 4 + 0);

            //Bottom
            triangles.Add(idx * 4 + 1 + 2);
            triangles.Add(idx * 4 + 5 + 2);
            triangles.Add(idx * 4 + 1 + 0);
            triangles.Add(idx * 4 + 5 + 0);
            triangles.Add(idx * 4 + 1 + 0);
            triangles.Add(idx * 4 + 5 + 2);
        }

        //Left
        triangles.Add(2);
        triangles.Add(3);
        triangles.Add(0);
        triangles.Add(3);
        triangles.Add(1);
        triangles.Add(0);

        //Right
        triangles.Add(detail * 4 + 2);
        triangles.Add(detail * 4 + 0);
        triangles.Add(detail * 4 + 3);
        triangles.Add(detail * 4 + 3);
        triangles.Add(detail * 4 + 0);
        triangles.Add(detail * 4 + 1);

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(null);
        mesh.SetUVs(0, uvs);

        mesh.UploadMeshData(true);


        string path = EditorUtility.SaveFilePanel("Save Seprate Mesh Asset", "Assets/", "GeratedMesh", "asset");
        path = FileUtil.GetProjectRelativePath(path);

        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
