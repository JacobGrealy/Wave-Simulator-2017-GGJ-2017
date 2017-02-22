using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleController : MonoBehaviour {
    public float InitialCastleHealth;
    private float castleHealth;
	// Use this for initialization
	void Start ()
    {
        castleHealth = InitialCastleHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (castleHealth <= 0)
            DestroyCastle();
	}

    public void DestroyCastle()
    {
        //Do cool castle explosion stuff here
        //@TODO
        //Load Start Menu
        Debug.Log("Destroy Castle");//Debug, get rid of this
        SceneManager.LoadScene(0);
    }

    public void takeDamage(float damageAmount)
    {
        castleHealth -= damageAmount;
    }

}
