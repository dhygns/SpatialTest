using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

using HoloToolkit.Unity.InputModule;

using FEC = FingerEventChecker;
using GroupEvent = FingerEventChecker.GROUPEVENT;

public class FingerChecker : MonoBehaviour
{
    public Camera _camera;
    public GameObject _prefabDot;

    public class Blob
    {
        public GameObject obj = null;
        public Vector3 evnpos = Vector3.zero;
    }

    private Dictionary<uint, Blob> _Dots;
    private List<uint> _DotIndices;

    // Use this for initialization
    void Start()
    {
        _Dots = new Dictionary<uint, Blob>();
        _DotIndices = new List<uint>();

        InteractionManager.InteractionSourceLost += (args) =>
        {
            if (args.state.source.kind != InteractionSourceKind.Hand) return;
            uint id = args.state.source.id;

            _DotIndices.Remove(id);

            Destroy(_Dots[id].obj);
            _Dots.Remove(id);
        };

        InteractionManager.InteractionSourceUpdated += (args) =>
        {
            if (args.state.source.kind != InteractionSourceKind.Hand) return;
            uint id = args.state.source.id;

            Vector3 pos;
            args.state.sourcePose.TryGetPosition(out pos);
            Blob target;
            if(_Dots.TryGetValue(id, out target))
            {
                //pos to ray
                Vector3 viewpos = Camera.main.WorldToViewportPoint(pos);
                Ray ray = Camera.main.ViewportPointToRay(viewpos);

                Vector3 retpos = ray.GetPoint(5.0f);
                if(0.4f > (retpos - target.evnpos).magnitude)
                {
                    target.evnpos = retpos;
                }
            }
            
        };

        InteractionManager.InteractionSourceDetected += (args) =>
        {
            if (args.state.source.kind != InteractionSourceKind.Hand) return;
            uint id = args.state.source.id;
            _DotIndices.Add(id);

            _Dots[id] = new Blob();
            _Dots[id].obj = Instantiate(_prefabDot);
            _Dots[id].obj.transform.localScale = Vector3.one * 0.2f;

            Vector3 pos;
            args.state.sourcePose.TryGetPosition(out pos);
            Blob target;
            if (_Dots.TryGetValue(id, out target))
            {
                //pos to ray
                Vector3 viewpos = Camera.main.WorldToViewportPoint(pos);
                Ray ray = Camera.main.ViewportPointToRay(viewpos);
                target.obj.transform.position = target.evnpos = ray.GetPoint(5.0f);
            }
        };

    }

    // Update is called once per frame
    void Update()
    {
        //calc distance
        float dist = Mathf.Infinity;
        for(int i = 0; i < _Dots.Count - 1; i++)
        {
            for(int j = 1; j < _Dots.Count; j++)
            {
                uint idA = _DotIndices[i];
                uint idB = _DotIndices[j];

                float tmpdist = (_Dots[idA].obj.transform.position - _Dots[idB].obj.transform.position).magnitude;
                dist = Mathf.Min(tmpdist, dist);
            }
        }

        //updating
        foreach (KeyValuePair<uint, Blob> pair in _Dots)
        {
            uint id = pair.Key;
            Blob target = pair.Value;

            Material mtrl = target.obj.GetComponent<MeshRenderer>().material;
            mtrl.color = Color.Lerp(Color.blue, Color.red, Mathf.Max(0.0f, dist- 2.0f));

            target.obj.transform.position += (target.evnpos - target.obj.transform.position) * 4.0f * Time.deltaTime;
        }
    }
}
