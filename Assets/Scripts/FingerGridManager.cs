using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FEC = FingerEventChecker;
using GroupEvent = FingerEventChecker.GROUPEVENT;

public class FingerGridManager : MonoBehaviour
{

    //Shader 
    private MaterialPropertyBlock _propertyBlock;
    private Renderer _rdrr;

    //Meshes
    private MeshFilter _filter;
    private Mesh _mesh;

    //Dummies
    public Camera _cam;
    
    // Use this for initialization
    void Start()
    {

        _rdrr = GetComponent<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();

        _filter = GetComponent<MeshFilter>();
        _mesh = _filter.mesh;
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.0f;
        transform.up = Vector3.up;
        //_cam.transform.up = Camera.main.transform.up;
        transform.forward = Camera.main.transform.forward;
        //Processing Condition
        if (FEC.GetEventState() != GroupEvent.PartialSelect)
        {
            Vector3[] vertices = _mesh.vertices;

            //LB
            vertices[0].x = 0.0f;
            vertices[0].y = 0.0f;
            vertices[0].z = 0.5f;
            //Debug.Log(vertices[0]);


            //RT
            vertices[1].x = 0.0f;
            vertices[1].y = 0.0f;
            vertices[1].z = 0.5f;
            //Debug.Log(vertices[1]);

            //RB
            vertices[2].x = 0.0f;
            vertices[2].y = 0.0f;
            vertices[2].z = 0.5f;
            //Debug.Log(vertices[2]);

            //LT
            vertices[3].x = 0.0f;
            vertices[3].y = 0.0f;
            vertices[3].z = 0.5f;
            //Debug.Log(vertices[3]);

            _mesh.vertices = vertices;
            _mesh.RecalculateBounds();
        }
        else
        {
            //Control Each Vertex
            Vector3[] vertices = _mesh.vertices;
            //for(int i = 0; i < _mesh.vertices.Length; i++)

            Vector3 LTView = FEC.GetEventLTValue();
            Vector3 RBView = FEC.GetEventRBValue();
            
            Ray LTRay = _cam.ViewportPointToRay(LTView);
            Ray RBRay = _cam.ViewportPointToRay(RBView);

            Vector3 LTPoint = LTRay.GetPoint(1.5f);
            Vector3 RBPoint = RBRay.GetPoint(1.5f);

            //LB
            vertices[0].x = LTPoint.x;
            vertices[0].y = RBPoint.y;
            vertices[0].z = 0.0f;
            //Debug.Log(vertices[0]);


            //RT
            vertices[1].x = RBPoint.x;
            vertices[1].y = LTPoint.y;
            vertices[1].z = 0.0f;
            //Debug.Log(vertices[1]);

            //RB
            vertices[2].x = RBPoint.x;
            vertices[2].y = RBPoint.y;
            vertices[2].z = 0.0f;
            //Debug.Log(vertices[2]);

            //LT
            vertices[3].x = LTPoint.x;
            vertices[3].y = LTPoint.y;
            vertices[3].z = 0.0f;
            //Debug.Log(vertices[3]);

            _mesh.vertices = vertices;
            _mesh.RecalculateBounds();
        }

        //_propertyBlock.SetVector("_Grid", FEC.GetEventValue());
        //_rdrr.SetPropertyBlock(_propertyBlock);
    }


}
