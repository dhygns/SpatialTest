using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour {
    public FingerEventChecker FEC;
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

		switch(FEC.GetEventStatus())
        {
            case FingerEventChecker.EVENT.Idle: break;
            case FingerEventChecker.EVENT.GroupPushAndPull:
                {
                    targetPosition -= transform.forward * FEC.GetEventValue().x * Time.deltaTime * interactionSpeed;

                    float dist = (Camera.main.transform.position - targetPosition).magnitude;
                    if (dist < 0.8f) targetPosition -= transform.forward;
                }
                break;
        }
	}
}
