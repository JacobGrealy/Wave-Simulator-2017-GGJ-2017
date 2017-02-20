using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject cannonParent;
    public GameObject cannonBarrel;
    public MeterController metercontroller;
    public AudioSource cannonAudioSource;
    public ParticleSystem dustEmitter;
    public GameObject cannonBallDummy;
    private GameObject loadedCannonBall;

    Vector3 cannonBaseInitialRot;
    Vector3 cannonBarrelInitialRot;

    enum CannonState {WAITING,CHARGING,FIRING,PRECOOLDOWN,COOLDOWN};
    private CannonState cannonState;

    //Rotation Details
    float barrelRotationMax = -62f;
    float barrelRotationMin = 0f;

    //Charging variables
    float chargePercent =0f;
    public float chargeRate = .6f;
    public float coolDownRate = 1f;
    float preCoolDownPercent = 1f;
    float preCoolDownRate = .8f;

    // Use this for initialization
    void Start ()
    {
        Vector3 cannonBaseInitialRot = cannonParent.transform.localEulerAngles;
        Vector3 cannonBarrelInitialRot = cannonBarrel.transform.localEulerAngles;
        cannonState = CannonState.WAITING;
        ReloadCannonBall();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Do something based on the current state
        //waiting
        if (cannonState == CannonState.WAITING) UpdateWaitingState();
        //Charging State
        else if (cannonState == CannonState.CHARGING) UpdateChargingState();
        //Firing
        else if (cannonState == CannonState.FIRING) UpdateFiringState();
        else if (cannonState == CannonState.PRECOOLDOWN) UpdatePreCooldownState();
        //Returning to startPosition / on cooldown
        else if (cannonState == CannonState.COOLDOWN) UpdateCooldownState();

        //Rotate towards mouse
        if (cannonState != CannonState.FIRING)
            RotateTowardsMouse();

    }

    private void RotateTowardsMouse()
    {
         // speed is the rate at which the object will rotate
         float speed = 5f;
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane cannonPlane = new Plane(Vector3.up, cannonParent.transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (cannonPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - cannonParent.transform.position);

            // Smoothly rotate towards the target point.
            cannonParent.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }

    public void UpdateWaitingState()
    {
        //if the player clicks
        if(Input.GetMouseButton(0) == true)
        {
            //Debug.Log("Switching to charge State");
            cannonState = CannonState.CHARGING;
        }
    }

    public void UpdateChargingState()
    {
        if (chargePercent <= 1f)
        {
            //Debug.Log("Charging...");
            chargePercent += chargeRate * Time.deltaTime;
            //Update the meter
            metercontroller.setLengthFromRatio(chargePercent);
            //Update Cannon Height
            setBarrelRotationFromRatio(chargePercent);
        }

        if (Input.GetMouseButton(0) == false)
        {
            cannonState = CannonState.FIRING;
        }
        
    }

    public void UpdateFiringState()
    {
        cannonAudioSource.Play();
        dustEmitter.Play();
        //Fire Cannon Ball (pass it the charge percent)
        ((CannonBallBehavior)loadedCannonBall.GetComponent(typeof(CannonBallBehavior))).Fire(chargePercent);
        
        preCoolDownPercent = 1f;
        cannonState = CannonState.PRECOOLDOWN;
    }

    public void UpdatePreCooldownState()
    {
        preCoolDownPercent -= preCoolDownRate * Time.deltaTime;
        if (preCoolDownPercent <= 0)
            cannonState = CannonState.COOLDOWN;
    }

    public void UpdateCooldownState()
    {
        chargePercent -= coolDownRate * Time.deltaTime;
        //Update the meter
        metercontroller.setLengthFromRatio(chargePercent);
        //Update Cannon Height
        setBarrelRotationFromRatio(chargePercent);

        if (chargePercent <= 0)
        {
            //Reload Cannonball
            ReloadCannonBall();

            //Switch to Waiting state
            cannonState = CannonState.WAITING;
        }
    }

    public void setBarrelRotationFromRatio(float ratio)
    {
        float xRot = Mathf.Lerp(barrelRotationMin, barrelRotationMax, (ratio));
        cannonBarrel.transform.localEulerAngles = new Vector3(xRot, cannonBarrel.transform.localEulerAngles.y, cannonBarrel.transform.localEulerAngles.z);
    }

    private void ReloadCannonBall()
    {
        loadedCannonBall = Object.Instantiate(cannonBallDummy, cannonBarrel.transform);
        loadedCannonBall.SetActive(true);
    }

}
