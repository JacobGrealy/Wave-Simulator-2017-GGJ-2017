using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBehavior : MonoBehaviour
{
    public bool isPushedByWaves = true;
    public Rigidbody rigidbody;
    public WaveController waveController;
    public float bouyancyScalar = 2f;
    public float dampeningFactor =.9f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Gravity Yo
        rigidbody.AddForce((Physics.gravity));
        //if xz position is inside of xz water bounding box
        if (waveController.IsOverWater(this.transform.position))
        {
            //if we are below water level
            float waterHeight = waveController.GetHeightAtPosition(this.transform.position);
            if (this.transform.position.y-2 < waterHeight)
            {
                //Move towards water level
                float distanceRatio = (waterHeight - this.transform.position.y) / (waterHeight+10);
                Vector3 bouyancyForce = new Vector3(0, -Physics.gravity.y * (1f+bouyancyScalar * distanceRatio), 0);
                rigidbody.AddForce((bouyancyForce));//*Time.deltaTime);
                //Apply dampening
                if(rigidbody.velocity.y < 0) rigidbody.velocity = rigidbody.velocity * (1 - dampeningFactor * Time.deltaTime);

                //Push by waves
                rigidbody.AddForce(waveController.GetForceFromWavesAtPosition(this.transform.position) * Time.deltaTime);
            }
        }
    }
}
