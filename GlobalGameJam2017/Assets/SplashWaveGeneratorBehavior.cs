using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashWaveGeneratorBehavior : MonoBehaviour
{
    //-----------Constraints
    //----Individual Wave Controls
    public float waveSpeed;
    public float maxHeight;
    public float minDistanceFromCenter;
    public float maxDistanceFromCenter;
    public float maxDistanceFromRingInside;
    public float maxDistanceFromRindOutside;
    private float impactForceStrength;
    private float maxDistanceSquared;

    //Multiple wave controls
    public float numberOfWavesToGenerate;
    public float timeBetweenWaves;
    //time between waves variance (Random)

    //-----------The Model
    private float numberOfWavesGenerated;
    private float timeSinceLastWaveWasGenerated;
    List<WaveRing> waveRingList = new List<WaveRing>();

    public void setForce(float impactForceStrength)
    {
        this.impactForceStrength = impactForceStrength;
    }



    // Use this for initialization
    void Start ()
    {
        numberOfWavesGenerated = 0;
        maxDistanceSquared = Mathf.Pow(maxDistanceFromCenter + maxDistanceFromRindOutside, 2);
        timeSinceLastWaveWasGenerated = timeBetweenWaves;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Create new wave
        CreateNewWave();

        //Update Waves
        UpdateWaves();

        //Remove Waves that are past maxDistanceFromCenter
        RemoveOldWaves();

        timeSinceLastWaveWasGenerated += Time.deltaTime;
    }

    public float GetWaveHeight(Vector3 otherPosition)
    {
        float waveHeight = 0f;
        Vector2 horizontalThisPosition = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 horizontalOtherPosition = new Vector2(otherPosition.x, otherPosition.z);

        //Large area check
        float distanceSquared = (horizontalOtherPosition - horizontalThisPosition).sqrMagnitude;
        if (distanceSquared < maxDistanceSquared)
        {
            //waveHeight = maxHeight; //For Debugging
            //indidvidual Wave Check
            foreach (WaveRing waveRing in waveRingList)
            {
                waveHeight = waveRing.GetWaveHeight(horizontalOtherPosition);
                //Assuming no overlapping waves, once we find the wave that effects height we can stop looking at other waveRings.
                if (waveHeight > 0)
                {
                    return waveHeight;
                }
            }
        }
        return waveHeight;
    }

    public bool ShouldBeDestroyed()
    {
        return numberOfWavesGenerated >= numberOfWavesToGenerate && waveRingList.Count == 0;
    }


    private void UpdateWaves()
    {
        foreach (WaveRing waveRing in waveRingList)
        {
            waveRing.Update();
        }
    }

    private void CreateNewWave()
    {        
        if (numberOfWavesGenerated < numberOfWavesToGenerate && timeSinceLastWaveWasGenerated >= timeBetweenWaves)
        {
            float waveCountRatio = Mathf.Pow((numberOfWavesToGenerate - numberOfWavesGenerated) / numberOfWavesToGenerate,3);
            Vector2 horizontalThisPosition = new Vector2(this.transform.position.x, this.transform.position.z);
            WaveRing waveRingToGenerate = new WaveRing(horizontalThisPosition,waveSpeed, maxHeight, minDistanceFromCenter, maxDistanceFromCenter, maxDistanceFromRingInside, maxDistanceFromRindOutside, impactForceStrength, waveCountRatio);
            waveRingList.Add(waveRingToGenerate);
            numberOfWavesGenerated++;
            timeSinceLastWaveWasGenerated = 0;
        }
    }

    private void RemoveOldWaves()
    {
        List<WaveRing> waveRingListToKeep = new List<WaveRing>();
        foreach (WaveRing waveRing in waveRingList)
        {
            if (!waveRing.ShouldBeDestroyed())
            {
                waveRingListToKeep.Add(waveRing);
            }
        }
        waveRingList = waveRingListToKeep;
    }
}
