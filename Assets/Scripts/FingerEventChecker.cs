﻿using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;



public class FingerEventChecker : MonoBehaviour, IInputHandler
{

    public enum EVENT
    {
        Idle, GroupPushAndPull,
    }

    //Events
    private Queue<uint> _IDs;
    private IInputSource _IIS;

    private EVENT _EventState;
    private Vector3 _EventValue;
    //Transforms

    public class Finger
    {
        public bool isFirst = true;
        public Vector3 OrinPosition = Vector3.zero;
        public Vector3 ViewPosition = Vector3.zero;
        public Vector3 ViewVelocity = Vector3.zero;
    }

    private Dictionary<uint, Finger> _Fingers;

    private void Awake()
    {
        _IDs = new Queue<uint>();
        _Fingers = new Dictionary<uint, Finger>();
        _EventState = EVENT.Idle;
    }

    // Use this for initialization
    void Start()
    {
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
        _EventState = EVENT.Idle;
        _EventValue = Vector3.zero;

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

            if (_IIS.TryGetGripPosition(id, out pos))
            {
                hitCount++; //  = Available Count
                Finger finger;

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

        //Debug.Log(_Fingers.Count + " " + _EventState);

        //Excuting Condition
        if (_EventState != EVENT.Idle) return;
        if (_Fingers.Count < 2) return;

        foreach (KeyValuePair<uint, Finger> pair in _Fingers)
        {
            uint key = pair.Key;
            Finger value = pair.Value;

            //Step1. Checking GroupPush & GroupPull
            {
                Vector3 DeltaPosition = (value.OrinPosition - value.ViewPosition);
                float Threshold = DeltaPosition.magnitude;

                if ((DeltaPosition.z < -0.05 || DeltaPosition.z > 0.05) && Threshold > 0.05)
                {
                    _EventState = EVENT.GroupPushAndPull;
                    return;
                }
            }

            //Step2. Checking Drag & Select
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculatingEvent()
    {
        switch (_EventState)
        {
            case EVENT.GroupPushAndPull:
                UpdateGroupPushAndPull();
                break;

            case EVENT.Idle:
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

        _EventValue = Vector3.one * pushDistance;

        //Debug.Log(_EventValue);
    }

    private void UpdateIdle()
    {

    }

    public EVENT GetEventStatus()
    {
        return _EventState;
    }

    public Vector3 GetEventValue()
    {
        return _EventValue;
    }
}