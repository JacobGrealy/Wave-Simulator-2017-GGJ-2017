using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallBehavior : MonoBehaviour
{
    public ParticleSystem trailParticleSystem;
    public ParticleSystem explosionParticleSystem;
    public WaveController waveController;
    public SplashWaveGeneratorBehavior splashWaveGeneratorBehavior;
    public Rigidbody myRigidbody;
    public float minFireForce = 1f;
    public float maxFireForce = 10f;
    public float gravityScalar = 2f;
    private bool physicsEnabled;

    public void Fire(float fireForceRatio)
    {
        setPhysicsEnabled(true);
        //Disconnect From Cannon
        this.transform.SetParent(null);
        //Enable Trail
        trailParticleSystem.Play();
        //Apply Explosive Force
        myRigidbody.AddRelativeForce(Vector3.forward * Mathf.Lerp(minFireForce, maxFireForce, fireForceRatio));
    }

    //Use this for initialization
    void Start ()
    {
        setPhysicsEnabled(false);
        
        //@todo Set A Death timer
    }
    	
	// Update is called once per frame
	void Update ()
    {
        //@todo if death Timer reached, destory this.	
        
        myRigidbody.AddForce(Physics.gravity * gravityScalar * Time.deltaTime);
        //See if we are under water (if we are, we must have collided with water, so let's trigger the onWaterCollision() function)
        if (physicsEnabled && waveController.IsOverWater(this.gameObject.transform.position))
        {
            //if we are below water level
            float waterHeight = waveController.GetHeightAtPosition(this.transform.position);
            float centerShift = -2f;
            if (this.transform.position.y - centerShift < waterHeight)
            {
                onWaterCollision();
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (physicsEnabled)
        {
            IDamagable damagable = (IDamagable)(col.gameObject.GetComponent(typeof(IDamagable)));
            if (damagable != null)
            {
                damagable.DoDamage(1f);
                startExplosion();
                this.kill();
            }
        }
    }

    private void setPhysicsEnabled(bool physicsEnabled)
    {
        this.physicsEnabled = physicsEnabled;
        myRigidbody.isKinematic = !physicsEnabled;
        myRigidbody.detectCollisions = physicsEnabled;
    }

    public void onWaterCollision()
    {
        splashWaveGeneratorBehavior.gameObject.transform.SetParent(null);
        splashWaveGeneratorBehavior.gameObject.SetActive(true);
        splashWaveGeneratorBehavior.setForce(myRigidbody.velocity.sqrMagnitude*myRigidbody.mass);
        waveController.AddSplashWaveGenerator(splashWaveGeneratorBehavior);
        //@todo Mist particles?
        //------------
        this.kill();
    }

    public void startExplosion()
    {
        explosionParticleSystem.gameObject.SetActive(true);
        explosionParticleSystem.gameObject.transform.SetParent(null);
        explosionParticleSystem.Play();
    }

    public void kill()
    {
        GameObject.Destroy(this.gameObject);
    }

}
