using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;



public class FingerEventChecker : MonoBehaviour, IInputHandler
{

    public enum GROUPEVENT
    {
        Idle, PushAndPull, PartialSelect
    }

    //Events
    private Queue<uint> _IDs;
    private IInputSource _IIS;

    private GROUPEVENT _EventState;
    private Vector4 _EventValue;
    private Vector3 _EventLTValue;
    private Vector3 _EventRBValue;
    //Transforms

    public class Finger
    {
        public bool isFirst = true;
        public Vector3 OrinPosition = Vector3.zero;
        public Vector3 ViewPosition = Vector3.zero;
        public Vector3 ViewVelocity = Vector3.zero;
    }

    private Dictionary<uint, Finger> _Fingers;


    static private FingerEventChecker instance;
    private void Awake()
    {
        if(!instance) instance = this;
    }

    // Use this for initialization
    void Start()
    {
        _IDs = new Queue<uint>();
        _Fingers = new Dictionary<uint, Finger>();
        _EventState = GROUPEVENT.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        SeperatingInput();
        SeperatingEvent();
        CalculatingEvent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputDown(InputEventData eventData)
    {
        _IIS = eventData.InputSource;
        _IDs.Enqueue(eventData.SourceId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputUp(InputEventData eventData)
    {
        _EventState = GROUPEVENT.Idle;
        _EventValue = Vector4.zero;

        _IIS = null;
        _IDs.Clear();
        _Fingers.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SeperatingInput()
    {
        //Excuting Condition
        if (_IIS == null) return;

        int hitCount = 0;

        foreach (uint id in _IDs)
        {
            Vector3 pos;

            //Get World Position
            if (_IIS.TryGetGripPosition(id, out pos)) 
            {
                //Convert worldposition to viewportposition (viewport position include depth too)
                pos = Camera.main.WorldToViewportPoint(pos);

                hitCount++; //  = Available Count
                Finger finger;

                //Trying to Get Value from Dic
                if (!_Fingers.TryGetValue(id, out finger)) 
                {
                    finger = new Finger();
                    finger.isFirst = false;
                    finger.OrinPosition = pos;
                    finger.ViewPosition = pos;
                    
                    _Fingers[id] = finger;
                }

                finger.ViewVelocity = pos - finger.ViewPosition;
                finger.ViewPosition = pos;
            }
            else
            {
                _Fingers.Remove(id);
            }
        }

        if (hitCount == 0)
        {
            OnInputUp(null);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SeperatingEvent()
    {
        Debug.Log("Event Stats :" + _EventState);
        //Excuting Condition
        if (_EventState != GROUPEVENT.Idle) return;
        if (_Fingers.Count < 2) return;

        foreach (KeyValuePair<uint, Finger> pair in _Fingers)
        {
            uint key = pair.Key;
            Finger value = pair.Value;

            //Step1. Checking GroupPush & GroupPull
            {
                Vector3 DeltaPosition = (value.OrinPosition - value.ViewPosition);

                if ((DeltaPosition.z < -0.01f || DeltaPosition.z > 0.01f))
                {
                    _EventState = GROUPEVENT.PushAndPull;
                    return;
                }
            }

            //Step2. Checking Drag & Select
            {
                Vector3 DeltaPosition = (value.OrinPosition - value.ViewPosition);
                float Threshold = DeltaPosition.x + DeltaPosition.y;

                if (Mathf.Abs(DeltaPosition.x) > 0.09f || Mathf.Abs(DeltaPosition.y) > 0.09f)
                {
                    _EventState = GROUPEVENT.PartialSelect;
                    return;
                }
            }

            //Step3. ?
            {

            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculatingEvent()
    {
        switch (_EventState)
        {
            case GROUPEVENT.PushAndPull:
                UpdateGroupPushAndPull();
                break;
            case GROUPEVENT.PartialSelect:
                UpdateGroupPartialSelect();
                break;
            case GROUPEVENT.Idle:
                UpdateIdle();
                break;
        }
    }

    private void UpdateGroupPushAndPull()
    {
        float pushDistance = 0.0f;

        foreach(KeyValuePair<uint, Finger> pair in _Fingers)
        {
            uint id = pair.Key;
            Finger value = pair.Value;

            pushDistance = 
                Mathf.Abs(pushDistance) < Mathf.Abs(value.ViewPosition.z - value.OrinPosition.z) ?
                value.ViewPosition.z - value.OrinPosition.z : pushDistance;
        }

        _EventValue = Vector4.one * pushDistance;

        //Debug.Log(_EventValue);
    }
    private void UpdateGroupPartialSelect()
    {
        uint cnt = 0;

        foreach (KeyValuePair<uint, Finger> pair in _Fingers)
        {
            if (cnt == 2) continue;
            uint id = pair.Key;
            Finger value = pair.Value;

            if (cnt == 0)
            {
                _EventLTValue = value.ViewPosition;
            }
            else
            {
                _EventRBValue = value.ViewPosition;
            }

            cnt++;
        }
    }

    private void UpdateIdle()
    {

    }

    public GROUPEVENT getEventState()
    {
        return _EventState;
    }

    public Vector4 getEventValue()
    {
        return _EventValue;
    }

    public Vector3 getEventLTValue()
    {
        return _EventLTValue;
    }

    public Vector3 getEventRBValue()
    {
        return _EventRBValue;
    }



    static public GROUPEVENT GetEventState()
    {
        return instance.getEventState();
    }

    static public Vector4 GetEventValue()
    {
        return instance.getEventValue();
    }

    static public Vector3 GetEventLTValue()
    {
        return instance.getEventLTValue();
    }

    static public Vector3 GetEventRBValue()
    {
        return instance.getEventRBValue();
    }
}