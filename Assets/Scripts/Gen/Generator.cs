using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    int detail = 50; // target mul

    // Use this for initialization
    void Start()
    {
        MeshFilterGen();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MeshFilterGen()
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>() as MeshFilter;
        Mesh mesh = meshFilter.mesh;


        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();


        //refac later
        for (int idx = 0; idx <= detail; idx ++)
        {
            // x
            float w = -0.5f + (float)idx / (float)detail;
            w = 0.5f * Mathf.Sin(w * Mathf.PI);// * (-1.0f + Mathf.Cos(w * 2.0f * Mathf.PI));

            // y
            float gap = 0.025f;
            float h = Mathf.Clamp(Mathf.Abs(w) - (0.5f - gap), 0.0f, gap) / gap;
            h = Mathf.Tan(h * Mathf.PI * 0.49f);

            //d = 0.5f * Mathf.Sin(d * Mathf.PI);
            //Top Vertex
            Vector3 vertTop;
            vertTop.x = w;
            vertTop.y = 0.5f - gap * h; 
            vertTop.z = 0.5f;

            //Bottom Vertex
            Vector3 vertBot;
            vertBot.x = w;
            vertBot.y = -0.5f + gap * h;
            vertBot.z = 0.5f;

            //Top UV
            Vector2 uvTop;
            uvTop.x = 0.5f + w;
            uvTop.y = 0.0f;

            //Bot UV
            Vector2 uvBot;
            uvBot.x = 0.5f + w;
            uvBot.y = 1.0f;

            vertices.Add(vertTop);
            vertices.Add(vertBot);

            uvs.Add(uvTop);
            uvs.Add(uvBot);

            triangles.Add(idx * 2 - 1);
            triangles.Add(idx * 2 + 1);
            triangles.Add(idx * 2 + 0);

            triangles.Add(idx * 2 + 0);
            triangles.Add(idx * 2 + 1);
            triangles.Add(idx * 2 + 2);
        }

        triangles.RemoveRange((detail + 1) * 6 - 3, 3);
        triangles.RemoveRange(0, 3);
        
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(null);
        mesh.SetUVs(0, uvs);

        mesh.UploadMeshData(true);
    }

}
