using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyBehavior : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float returnSpeed = 1f;
    Vector3 startingPos;

	// Use this for initialization
	void Start ()
    {
        startingPos = this.transform.position;		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 forceDirection = (new Vector3(startingPos.x - this.transform.position.x,0, startingPos.z - this.transform.position.z)).normalized;
        rigidbody.AddForce(forceDirection * returnSpeed);        	
	}
}
