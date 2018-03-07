using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Generator3D : MonoBehaviour
{

    int detail = 30; // target mul

    // Use this for initialization
    void Start()
    {

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>() as MeshFilter;
        Mesh mesh = meshFilter.mesh;
        int idx = 0;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<Vector2> uvs = new List<Vector2>();

        List<int> triangles = new List<int>();

        //Vertices & Coords (Front / Rear)
        for (idx = 0; idx <= detail; idx++)
        {
            float prog = (float)idx / (float)detail;

            //Vertex
            float w = -0.5f + (1.0f - Mathf.Cos(prog * Mathf.PI)) * 0.5f;

            Vector3 vertTopFront = new Vector3(w, 0.5f, 0.02f);
            Vector3 vertBotFront = new Vector3(w, -0.5f, 0.02f);
            Vector3 vertTopRear = new Vector3(w, 0.5f, -0.02f);
            Vector3 vertBotRear = new Vector3(w, -0.5f, -0.02f);

            Vector3 normFront = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 normRear = new Vector3(0.0f, 0.0f, -1.0f);

            Vector2 uvTop = new Vector2(0.5f + w, 0.0f);
            Vector2 uvBot = new Vector3(0.5f + w, 1.0f);

            vertices.Add(vertTopFront); //for Front
            vertices.Add(vertBotFront); //for Front
            normals.Add(normFront);
            normals.Add(normFront);

            vertices.Add(vertTopRear);  //for Rear
            vertices.Add(vertBotRear);  //for Rear
            normals.Add(normRear);
            normals.Add(normRear);

            uvs.Add(uvTop);
            uvs.Add(uvBot);
            uvs.Add(uvTop);
            uvs.Add(uvBot);

            colors.Add(Color.white);
            colors.Add(Color.white);
            colors.Add(Color.white);
            colors.Add(Color.white);
        }


        //Faces (indices)
        for (int i = 0; i < detail; i++)
        {
            //Front
            triangles.Add(i * 4 + 0);
            triangles.Add(i * 4 + 1);
            triangles.Add(i * 4 + 4);
            triangles.Add(i * 4 + 4);
            triangles.Add(i * 4 + 1);
            triangles.Add(i * 4 + 5);

            //Rear
            triangles.Add(i * 4 + 0 + 2);
            triangles.Add(i * 4 + 4 + 2);
            triangles.Add(i * 4 + 1 + 2);
            triangles.Add(i * 4 + 5 + 2);
            triangles.Add(i * 4 + 1 + 2);
            triangles.Add(i * 4 + 4 + 2);
        }

        int TBIdx = idx * 4;

        //Vertices & Coords (Top / Bottom)
        for (idx = 0; idx <= detail; idx++)
        {
            float prog = (float)idx / (float)detail;

            //Vertex
            float w = -0.5f + (1.0f - Mathf.Cos(prog * Mathf.PI)) * 0.5f;

            Vector3 vertTopRear = new Vector3(w, 0.5f, -0.02f);
            Vector3 vertTopFront = new Vector3(w, 0.5f, 0.02f);
            Vector3 vertBotRear = new Vector3(w, -0.5f, -0.02f);
            Vector3 vertBotFront = new Vector3(w, -0.5f, 0.02f);

            Vector3 normTop = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 normBot = new Vector3(0.0f,-1.0f, 0.0f);

            Vector2 uvRear  = new Vector2(0.5f + w, 0.0f);
            Vector2 uvFront = new Vector3(0.5f + w, 1.0f);

            vertices.Add(vertTopRear);  //for Top
            vertices.Add(vertTopFront); //for Top
            normals.Add(normTop);
            normals.Add(normTop);
            uvs.Add(uvRear);
            uvs.Add(uvFront);

            vertices.Add(vertBotRear);  //for Bot
            vertices.Add(vertBotFront); //for Bot
            normals.Add(normBot);
            normals.Add(normBot);
            uvs.Add(uvRear);
            uvs.Add(uvFront);

            colors.Add(Color.black);
            colors.Add(Color.black);
            colors.Add(Color.black);
            colors.Add(Color.black);

        }

        //Faces (indices)
        for (int i = 0; i < detail; i++)
        {
            //Front
            triangles.Add(TBIdx + i * 4 + 0);
            triangles.Add(TBIdx + i * 4 + 1);
            triangles.Add(TBIdx + i * 4 + 4);
            triangles.Add(TBIdx + i * 4 + 4);
            triangles.Add(TBIdx + i * 4 + 1);
            triangles.Add(TBIdx + i * 4 + 5);

            //Rear
            triangles.Add(TBIdx + i * 4 + 0 + 2);
            triangles.Add(TBIdx + i * 4 + 4 + 2);
            triangles.Add(TBIdx + i * 4 + 1 + 2);
            triangles.Add(TBIdx + i * 4 + 5 + 2);
            triangles.Add(TBIdx + i * 4 + 1 + 2);
            triangles.Add(TBIdx + i * 4 + 4 + 2);
        }

        int LRIdx = TBIdx + idx * 4;

        {
            vertices.Add(new Vector3(-0.5f, 0.5f, -0.02f));
            vertices.Add(new Vector3(-0.5f,-0.5f, -0.02f));
            vertices.Add(new Vector3(-0.5f, 0.5f,  0.02f));
            vertices.Add(new Vector3(-0.5f,-0.5f,  0.02f));

            uvs.Add(new Vector2( 0.0f, 0.0f));
            uvs.Add(new Vector2( 1.0f, 0.0f));
            uvs.Add(new Vector2( 0.0f, 1.0f));
            uvs.Add(new Vector2( 1.0f, 1.0f));

            normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));
            normals.Add(new Vector3(-1.0f, 0.0f, 0.0f));

            colors.Add(Color.black);
            colors.Add(Color.black);
            colors.Add(Color.black);
            colors.Add(Color.black);

            triangles.Add(LRIdx + 0);
            triangles.Add(LRIdx + 1);
            triangles.Add(LRIdx + 2);
            triangles.Add(LRIdx + 1);
            triangles.Add(LRIdx + 3);
            triangles.Add(LRIdx + 2);

            vertices.Add(new Vector3( 0.5f,  0.5f,  0.02f));
            vertices.Add(new Vector3( 0.5f, -0.5f,  0.02f));
            vertices.Add(new Vector3( 0.5f,  0.5f, -0.02f));
            vertices.Add(new Vector3( 0.5f, -0.5f, -0.02f));

            uvs.Add(new Vector2(0.0f, 0.0f));
            uvs.Add(new Vector2(1.0f, 0.0f));
            uvs.Add(new Vector2(0.0f, 1.0f));
            uvs.Add(new Vector2(1.0f, 1.0f));

            normals.Add(new Vector3( 1.0f, 0.0f, 0.0f));
            normals.Add(new Vector3( 1.0f, 0.0f, 0.0f));
            normals.Add(new Vector3( 1.0f, 0.0f, 0.0f));
            normals.Add(new Vector3( 1.0f, 0.0f, 0.0f));

            colors.Add(Color.black);
            colors.Add(Color.black);
            colors.Add(Color.black);
            colors.Add(Color.black);

            triangles.Add(LRIdx + 4);
            triangles.Add(LRIdx + 5);
            triangles.Add(LRIdx + 6);
            triangles.Add(LRIdx + 5);
            triangles.Add(LRIdx + 7);
            triangles.Add(LRIdx + 6);
        }


        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetColors(colors);

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
