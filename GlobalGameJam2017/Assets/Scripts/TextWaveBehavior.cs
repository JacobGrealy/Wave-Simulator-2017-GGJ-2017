using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWaveBehavior
{
    private float yPosition;
    private int[,] heightArray;
    private float speed;

    public TextWaveBehavior(int[,] heightArray, float startYPosition, float speed)
    {
        this.heightArray = heightArray;
        this.yPosition = startYPosition;
        this.speed = speed;
    }
	
	// Update is called once per frame
	public void Update ()
    {
        this.yPosition -= speed * Time.deltaTime;
	}

    public bool ContainsPosition(int x, int y)
    {
        return (y > (int)(yPosition) && y < ((int)yPosition) + heightArray.GetLength(1));
    }

    public float GetHeightAtPosition(int x, int y)
    {
        if (!ContainsPosition(x, y)) return 0;

        //we need to flip x
        int flippedX = heightArray.GetLength(0)-1-x;
        float height = 0;
        height = (float)(heightArray[flippedX, (int)(y - yPosition)]);

        /*//Making leading wave blocks
        if (height == 0)
        {
            //if the block above us is 1, set this to .5
            if(GetHeightAtPosition(x, y+1) ==1)
            {
                height = .1f;
            } 
        }*/

        return height;
    }
}
