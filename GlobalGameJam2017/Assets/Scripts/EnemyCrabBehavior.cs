using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrabBehavior : MonoBehaviour//, IDamagable
{
    public GameObject crabGhost;
    public float crabSpeed = 10.0f;
    public float crabDamage = 1f;
    public Rigidbody rigidbody;
    private float health;

	// Use this for initialization
	void Start ()
    {
        health = 1f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (health <= 0)
            Kill();
        Vector3 crabMovementForce = Vector3.forward * crabSpeed * Time.deltaTime;
        rigidbody.AddRelativeForce(crabMovementForce);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains("Buoy"))
        {
            this.Kill();
        }
        if (col.gameObject.name.Contains("EnemyDamageZone"))
        {
            Debug.Log("We Collided with Damage Zone");//Debug, get rid of this
            ((CastleController)col.gameObject.GetComponent("CastleController")).takeDamage(crabDamage);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("EnemyDamageZone"))
        {
            Debug.Log("We Collided with Damage Zone");//Debug, get rid of this
            ((CastleController)other.gameObject.GetComponent("CastleController")).takeDamage(crabDamage);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void DoDamage(float damage)
    {
        health -= damage;
    }

    public void Kill()
    {
        //set the ghost to active
        crabGhost.SetActive(true);
        //detach the ghost from this
        crabGhost.transform.SetParent(null);
        //destroy this
        Object.Destroy(this.gameObject);
    }
}
