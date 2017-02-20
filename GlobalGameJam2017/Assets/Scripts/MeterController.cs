using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterController : MonoBehaviour {
    public float sizeRatio = .01f;
    public Material material;

    Vector3 startSize;

    float minLength = 0.0006f;
    float maxLength = 0.034f;    
    
    public void setLengthFromRatio(float ratio)
    {
        //Debug.Log("setLengthFromRatio: " + ratio);
        float xSizeValue = (ratio * (maxLength - minLength) + minLength);
        this.gameObject.transform.localScale = new Vector3(xSizeValue,startSize.y,startSize.z);
    }
    
    // Use this for initialization
    void Start ()
    {
        startSize = gameObject.transform.localScale;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //this.gameObject.transform.localScale += new Vector3(.1f, 0, 0);
        //if (this.gameObject.transform.localScale.x > maxLength)
        //    this.gameObject.transform.localScale = startSize;
        updateColor();
    }

    private void updateColor()
    {
        //setLengthFromRatio(sizeRatio);

        float lengthRatio = (this.gameObject.transform.localScale.x - minLength) / maxLength;
        float colorR = Mathf.Lerp(Color.green.r, Color.red.r, lengthRatio);
        float colorG = Mathf.Lerp(Color.green.g, Color.red.g, lengthRatio);
        float colorB = Mathf.Lerp(Color.green.b, Color.red.b, lengthRatio);
        material.SetColor("_Color", new Color(colorR, colorG, colorB));
    }
}
