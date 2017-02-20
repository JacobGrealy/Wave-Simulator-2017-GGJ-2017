using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCubeBehavior : MonoBehaviour {
    public Color bubbleStartColor;
    public Color bubbleEndColor;
    public float bubbleDisperseSpeed = .5f;
    public float bubleMinAlpha = .1f;
    public float bubbleMaxAlpha = .5f;
    public string materialColorName = "_Color";
    public GameObject highLightCube;
    public ParticleSystem bubbles;
    public GameObject bubblesGameObject;
    public float bubbleScaleStartPoint = 1f;
    private float colorRatio;
    
    
    
    
    // Use this for initialization
    void Start ()
    {
	    	
	}
	
    public void Highlight()
    {
        highLightCube.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {       
    }

    public void UpdateBubbleParticles()
    {
        if (this.transform.localScale.y > bubbleScaleStartPoint)
        {
            //if (!bubbles.isPlaying)
            //bubbles.Play();
            bubblesGameObject.SetActive(true);
            colorRatio = 0f;
            bubblesGameObject.GetComponent<Renderer>().material.SetColor(materialColorName, bubbleStartColor);
        }
        else
        {
            //if (bubbles.isPlaying)
            //bubbles.Stop();
            if (bubblesGameObject.activeSelf == true)
            {
                colorRatio += bubbleDisperseSpeed * Time.deltaTime;
                colorRatio = Mathf.Min(colorRatio, 1.0f); //cap it at 1
                float colorR = Mathf.Lerp(bubbleStartColor.r, bubbleEndColor.r, colorRatio);
                float colorG = Mathf.Lerp(bubbleStartColor.g, bubbleEndColor.g, colorRatio);
                float colorB = Mathf.Lerp(bubbleStartColor.b, bubbleEndColor.b, colorRatio);
                bubblesGameObject.GetComponent<Renderer>().material.SetColor(materialColorName, new Color(colorR, colorG, colorB));
                if (colorRatio >= 1.0f)
                {
                    bubblesGameObject.SetActive(false);
                }
            }
        }
    }
}
