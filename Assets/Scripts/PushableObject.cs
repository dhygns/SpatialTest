using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FEC = FingerEventChecker;
using GroupEvent = FingerEventChecker.GROUPEVENT;

public class PushableObject : MonoBehaviour {
    // Use this for initialization

    public float chasingSpeed;
    public float interactionSpeed;
    public Vector3 targetPosition;

    void Start () {
        chasingSpeed = Random.Range(0.8f, 4.2f);
        interactionSpeed = Random.Range(17.0f, 20.0f);
        targetPosition = transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        transform.position += (targetPosition - transform.position) * chasingSpeed* Time.deltaTime;
        transform.LookAt(Camera.main.transform);

		switch(FEC.GetEventState())
        {
            case GroupEvent.Idle: break;
            case GroupEvent.PushAndPull:
                {
                    targetPosition -= transform.forward * FEC.GetEventValue().x * Time.deltaTime * interactionSpeed;

                    float dist = (Camera.main.transform.position - targetPosition).magnitude;
                    if (dist < 0.8f) targetPosition -= transform.forward;
                }
                break;
            case GroupEvent.PartialSelect:
                {

                }
                break;
        }
	}


    public bool getViewPortPosition()
    {
        return false;
    }
}
