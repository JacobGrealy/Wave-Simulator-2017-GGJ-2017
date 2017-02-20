using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {
    public WaveTextHeightMapGenerator waveTextHeightMapGenerator;

    //Number of Columns
    int columns = 56;
    //Number of rows
    int rows = 36;

    //the space between the cubes
    public float margin = 0f;//.1f;

    public float waveHeightGeneral = 1f;
    public float waveHeightMin = .01f;

    public float perlinScalerXBase = .25f;
    public float perlinScalerYBase = .1f;
    public float perlinScalerXMedium = .25f;
    public float perlinScalerYMedium = .05f;

    public float heightScalarbase = 2f;
    public float heightScalarMedium = 30f;
    public float heightScalarLarge = 12f;

    public float minPerlinBase = .4f;
    public float minPerlinMedium = .6f;
    public float minPerlinLarge = .8f;

    public float waveSpeed = .006f;
    public float distanceFromShoreScalar = .5f;

    //A wave cube, we will make a bunch of these
    public GameObject waveCube;
    public int framesToWaitForUpdate = 2;
    public float waveForceScaler = 1f;

    //2d array to hold the wave cubes
    private GameObject[,] waveCubeArray;
    //Where the wave is horizontally in the perlin noise
    private float waveX;
    //Tracks how many frames since we last updated wave height
    private int updateCount = 0;
    private List<SplashWaveGeneratorBehavior> splashWaveGeneratorBehaviorList;




    private Vector2 GetIndexesFromPosition(Vector3 otherPosition)
    {
        Vector3 otherLocalPosition = otherPosition - this.transform.position;
        float positionRatioX = otherLocalPosition.x / (columns * (waveCube.GetComponent<Renderer>().bounds.size.x));
        float positionRatioY = otherLocalPosition.z / (rows * (waveCube.GetComponent<Renderer>().bounds.size.z));
        int indexX = (int)(positionRatioX * columns);
        int indexY = (int)(positionRatioY * rows);
        return new Vector2(indexX, indexY);
    }

    public bool IsOverWater(Vector3 otherPosition)
    {
        return IsOverWater(GetIndexesFromPosition(otherPosition));
    }
    public bool IsOverWater(Vector2 indexes)
    {
        int indexX = (int)(indexes.x);
        int indexY = (int)(indexes.y);
        if (indexX < 0 || indexX >= columns || indexY < 0 || indexY > rows)
            return false;
        return true;
    }

    public float GetHeightAtPosition(Vector3 otherPosition)
    {
        return GetHeightAtPosition(GetIndexesFromPosition(otherPosition));
    }
    public float GetHeightAtPosition(Vector2 indexes)
    {
        int indexX = (int)(indexes.x);
        int indexY = (int)(indexes.y);        
        return (waveCubeArray[indexX, indexY].transform.position.y + waveCubeArray[indexX, indexY].GetComponent<Renderer>().bounds.size.y);
    }

    public float GetWaveSizeAtPosition(Vector3 otherPosition)
    {
        return GetWaveSizeAtPosition(GetIndexesFromPosition(otherPosition));
    }
    public float GetWaveSizeAtPosition(Vector2 indexes)
    {
        int indexX = (int)(indexes.x);
        int indexY = (int)(indexes.y);
        return (waveCubeArray[indexX, indexY].GetComponent<Renderer>().bounds.size.y);
    }


    public Vector3 GetForceFromWavesAtPosition(Vector3 otherPosition)
    {
        return GetForceFromWavesAtPosition(GetIndexesFromPosition(otherPosition));
    }
    //Apply force way from high point of wave
    public Vector3 GetForceFromWavesAtPosition(Vector2 centerIndex)
    {
        Vector3 waveForce = new Vector3();

        Vector2 leftIndex = centerIndex + new Vector2(-1f, 0);
        Vector2 rightIndex = centerIndex + new Vector2(+1f, 0);
        Vector2 upIndex = centerIndex + new Vector2(0, +1f);
        Vector2 downIndex = centerIndex + new Vector2(0, -1f);

        float centerSize = GetWaveSizeAtPosition(centerIndex);

        float leftSize = 1f; //1 if not over water
        if(IsOverWater(leftIndex)) leftSize = GetWaveSizeAtPosition(leftIndex);

        float rightSize = 1f;
        if (IsOverWater(rightIndex)) rightSize = GetWaveSizeAtPosition(rightIndex);

        float upSize = 1f;
        if (IsOverWater(upIndex)) upSize = GetWaveSizeAtPosition(upIndex);

        float downSize = 1f;
        if (IsOverWater(downIndex)) downSize = GetWaveSizeAtPosition(downIndex);
        
        //Determine the horizontal force
        if (leftSize > centerSize || rightSize > centerSize)
        {
            waveForce += new Vector3(leftSize - rightSize,  0, 0);
        }

        //Add on the verticle force
        if (leftSize > centerSize || rightSize > centerSize)
        {
            waveForce += new Vector3(0, 0, upSize - downSize);
        }

        return waveForce * waveForceScaler;
    }

    private void RemoveOldSplashWaveGenerators()
    {
        List<SplashWaveGeneratorBehavior> SplashWaveGeneratorBehaviorListToKeep = new List<SplashWaveGeneratorBehavior>();
        foreach (SplashWaveGeneratorBehavior splashWaveGenerator in splashWaveGeneratorBehaviorList)
        {
            if (!splashWaveGenerator.ShouldBeDestroyed())
            {
                SplashWaveGeneratorBehaviorListToKeep.Add(splashWaveGenerator);
            }
            else
            {
                GameObject.Destroy(splashWaveGenerator.gameObject);
            }
        }
        splashWaveGeneratorBehaviorList = SplashWaveGeneratorBehaviorListToKeep;
    }

    public void AddSplashWaveGenerator(SplashWaveGeneratorBehavior splashWaveGeneratorBehavior)
    {
        splashWaveGeneratorBehaviorList.Add(splashWaveGeneratorBehavior);
    }

    // Use this for initialization
    void Start ()
    {
        //Instantiate the waveCubeArray
        waveCubeArray = new GameObject[columns, rows];

        //Instantiate wave cubes
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 cubePosition = this.transform.position + new Vector3(x * (waveCube.transform.localScale.x+margin), 0 ,y * (waveCube.transform.localScale.z+margin));
                waveCubeArray[x, y] = Instantiate(waveCube,cubePosition,this.gameObject.transform.rotation,this.gameObject.transform);
            }            
        }

        //Instantiate list for storing SplashWaveGenerators
        splashWaveGeneratorBehaviorList = new List<SplashWaveGeneratorBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        waveX += waveSpeed * Time.deltaTime;
        updateCount += 1;
        if (updateCount > framesToWaitForUpdate)
        {
            updateCount = 0;
            //Change Cubes height
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    GameObject currentCube = waveCubeArray[x, y];
                    float waveHeight = 0;

                    float perlinNoiseBaseValue = Mathf.PerlinNoise(waveX + x * perlinScalerXBase, y * perlinScalerYBase);
                    float perlinNoiseMediumValue = Mathf.PerlinNoise(waveX + x * perlinScalerXMedium, y * perlinScalerYMedium);

                    //Make Wave Height = Base Wave Height
                    waveHeight = (Mathf.Max(minPerlinBase, perlinNoiseBaseValue) - minPerlinBase) * heightScalarbase;

                    //Add medium perlin wave onto base wave
                    waveHeight += (Mathf.Max(minPerlinMedium, perlinNoiseMediumValue) - minPerlinMedium) * heightScalarMedium;

                    //Draw Wave Text
                    waveHeight += waveTextHeightMapGenerator.getHeightAtPosition(y, x);

                    //Make the Wave height larger closer to shore
                    waveHeight *= waveHeightGeneral * ((1f - distanceFromShoreScalar) + distanceFromShoreScalar * (((1.0f * columns - x) / columns)));

                    //Add Explosion Waves
                    //Check for dead splashwavegenerators, remove them from list
                    RemoveOldSplashWaveGenerators();
                    //check if this cube is near an explosion
                    //Calculate height based on time since explosion start and distance from explosion
                    foreach (SplashWaveGeneratorBehavior splashWaveGenerator in splashWaveGeneratorBehaviorList)
                    {
                        float splashHeight = splashWaveGenerator.GetWaveHeight(currentCube.transform.position);
                        waveHeight += splashHeight;
                    }

                    //Make waveHeightsure the wave is atleast a certain height (so that the texture doesn't freak out
                    waveHeight = Mathf.Max(waveHeightMin, waveHeight);
                    currentCube.transform.localScale = new Vector3(currentCube.transform.localScale.x, waveHeight, currentCube.transform.localScale.z);
                    ((WaveCubeBehavior)currentCube.GetComponent(typeof(WaveCubeBehavior))).UpdateBubbleParticles();
                }
            }
        }
    }
}
