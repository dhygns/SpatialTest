using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FEC = FingerEventChecker;
using GroupEvent = FingerEventChecker.GROUPEVENT;

public class FingerChecker : MonoBehaviour
{
    public GameObject _prefabDot;

    private Dictionary<uint, GameObject> _Dots;
    // Use this for initialization
    void Start () {
        _Dots = new Dictionary<uint, GameObject>();
    }
	
	// Update is called once per frame
	void Update () {

        foreach(KeyValuePair<uint, FEC.Finger> pair in FEC.GetFingers())
        {
            uint index = pair.Key;
            FEC.Finger finger = pair.Value;

            GameObject dot;
            if(_Dots.TryGetValue(index, out dot))
            {

            }
            else
            {
                _Dots[index] = Instantiate(_prefabDot);
            }
            _Dots[index].transform.position = finger.WorldPosition;
        }
    }
}
