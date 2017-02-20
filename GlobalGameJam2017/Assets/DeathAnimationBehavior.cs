using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimationBehavior : MonoBehaviour {
    public AudioSource crabDeathAudioSource;
    public float riseSpeed = 2f;
    public float timer = 5f;


	// Use this for initialization
	void Start ()
    {
        crabDeathAudioSource.PlayDelayed(.1f);	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timer <= 0)
            GameObject.Destroy(this.gameObject);
        //rotate to face the camera
        transform.LookAt(Camera.main.transform.position, Vector3.up);

        //time before dissapearing
        timer -= Time.deltaTime;
        //move upward
        transform.position += transform.up * riseSpeed * Time.deltaTime;
	}
}
