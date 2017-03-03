using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPath : MonoBehaviour
{
    public CannonController cannonController;
    public LineRenderer lineRenderer;
    public GameObject startingGameObject;

    private float textureOffset = 0;
    private float textureScrollSpeed =1.5f;
    private int numSamplePoints = 10;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CannonBallBehavior loadedCannonBallScript = ((CannonBallBehavior)cannonController.loadedCannonBall.GetComponent(typeof(CannonBallBehavior)));
        List<Vector3> newPositionsList = new List<Vector3>();
        newPositionsList.Add(startingGameObject.transform.position);

        //Generate Curve
        if (loadedCannonBallScript != null)
        {
            Vector3 direction = (startingGameObject.transform.position).normalized; //need to update this with direction of Cannon

            float angle = cannonController.getXRotation();
            float initialForce = loadedCannonBallScript.calculateFireForce(cannonController.getChargePercent()).magnitude;
            float mass = loadedCannonBallScript.myRigidbody.mass;
            float drag = loadedCannonBallScript.myRigidbody.drag;
            Vector3 gravity = Physics.gravity * loadedCannonBallScript.gravityScalar;

            float initialVelocity = initialForce / mass; //we can fudge this math because the acceleration will be applied instantly over 1 frame
            float distanceFlown = Mathf.Pow(initialVelocity, 2) * Mathf.Sin(2 * angle) / gravity.magnitude;
            float flightTime = distanceFlown / (initialVelocity * Mathf.Cos(angle));

            for (int t = 1; t <= numSamplePoints; t++)
            {
                float percent = (t * 1.0f) / numSamplePoints;
                Debug.Log(percent);
                float currentTime = percent * flightTime;
                Debug.Log(currentTime);
                float height = gravity.magnitude * Mathf.Pow(currentTime, 2) + Mathf.Sin(angle) * initialVelocity;
                float distance = initialVelocity * Mathf.Cos(angle) * currentTime;
                Debug.Log(new Vector3(direction.x * distance, direction.y * distance, height));
                newPositionsList.Add(new Vector3(direction.x * distance, direction.y * distance, height));
            }

        }
        lineRenderer.SetPositions(newPositionsList.ToArray());
        textureOffset -= textureScrollSpeed * Time.deltaTime;
        lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(textureOffset, 0));
    }
}
