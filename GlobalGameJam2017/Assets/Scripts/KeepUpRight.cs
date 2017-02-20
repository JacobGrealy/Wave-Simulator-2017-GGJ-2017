using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpRight : MonoBehaviour {
    public float buoyantTorque = 500f;
    public Rigidbody rigidbody;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Rotate towards upright
        Quaternion rotationDelta = Quaternion.FromToRotation(transform.up, Vector3.up);
        rigidbody.AddTorque(rotationDelta.x * buoyantTorque * Time.deltaTime, rotationDelta.y * buoyantTorque * Time.deltaTime, rotationDelta.z * buoyantTorque * Time.deltaTime);
    }
}
