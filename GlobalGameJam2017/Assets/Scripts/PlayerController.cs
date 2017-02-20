using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource quackAudioSource;
    public AudioClip[] quacks;
    public Rigidbody rigidbody;
    public float playerForwardSpeed = 500f;
    public float playerBackwardSpeed = 350f;
    public float TurnSpeed = 500f;

    // Use this for initialization
    void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
        updateControls();

    }
    private void updateControls()
    {
        //Go Forward
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddRelativeForce(Vector3.forward * playerForwardSpeed * Time.deltaTime);
        }

        //Go Backwards
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.AddRelativeForce(Vector3.back * playerBackwardSpeed * Time.deltaTime);
        }

        //Turn Left
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddRelativeTorque(0, - TurnSpeed * Time.deltaTime, 0);
        }

        //Turn Right
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddRelativeTorque(0, TurnSpeed * Time.deltaTime, 0);
        }

        //Drop Bomb
        if (Input.GetKey(KeyCode.Space))
        {
            //Check to make sure they haven't dropped a bomb too recently
        }

        //Dedicated Quack button
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int QuackIndex = Random.Range(0, quacks.Length - 1);
            quackAudioSource.pitch = Random.Range(.8f, 1.1f);
            quackAudioSource.PlayOneShot(quacks[QuackIndex], Random.Range(.2f, 1f));
        }
    }


}
