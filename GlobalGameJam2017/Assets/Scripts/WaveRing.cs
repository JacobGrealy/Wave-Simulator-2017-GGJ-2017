using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRing
{
    private Vector2 originPosition;
    private float waveSpeed;
    private float maxHeight;
    private float minDistanceFromCenter;
    private float maxDistanceFromCenter;
    private float maxDistanceFromRingInside;
    private float maxDistanceFromRindOutside;
    private float impactForceStrength;

    private float radius;

    public WaveRing(Vector2 originPosition,float waveSpeed, float maxHeight, float minDistanceFromCenter, float maxDistanceFromCenter, float maxDistanceFromRingInside, float maxDistanceFromRindOutside, float impactForceStrength, float waveCountRatio)
    {
        this.originPosition = originPosition;
        this.waveSpeed = waveSpeed;
        this.maxHeight = maxHeight * waveCountRatio;
        this.minDistanceFromCenter = minDistanceFromCenter;
        this.maxDistanceFromCenter = maxDistanceFromCenter;
        this.maxDistanceFromRingInside = maxDistanceFromRingInside;
        this.maxDistanceFromRindOutside = maxDistanceFromRindOutside;
        this.impactForceStrength = impactForceStrength;

        radius = minDistanceFromCenter; 
    }

	public bool ShouldBeDestroyed()
    {
        return radius > maxDistanceFromCenter;
    }

    public float GetWaveHeight(Vector2 otherPosition)
    {
        float waveHeight = 0;

        float startofRing = radius - maxDistanceFromRingInside;
        float endofRing = radius + maxDistanceFromRindOutside;
        float otherRadius = (otherPosition - originPosition).magnitude;

        //is the other object inside of the ring?
        if (otherRadius >= startofRing && otherRadius <= endofRing)
        {
            float radiusRatio = 1f - (radius / maxDistanceFromCenter);
            float distanceFromRingRatio = 0f;
            if (otherRadius < radius)
            {
                distanceFromRingRatio = 1-((radius - otherRadius) / maxDistanceFromRingInside);
            }
            if (otherRadius > radius)
            {
                distanceFromRingRatio = 1-((otherRadius - radius) / maxDistanceFromRindOutside);
            }
            waveHeight = maxHeight * distanceFromRingRatio * radiusRatio * (impactForceStrength/500f);
        }

        return waveHeight;
    }


    // Update is called once per frame
    public void Update ()
    {
        radius += waveSpeed * Time.deltaTime;
    }
}
