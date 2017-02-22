using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTextHeightMapGenerator : MonoBehaviour
{
    public Texture2D waveFontTexture2D;
    int[,] waveFontTextureArray;
    int characterWidth = 4;
    int characterHeight = 6;
    int numberCharactersPerRow = 32;
    int lineSpacing = 0;
    float HeightScalar = 7f;

    List<TextWaveBehavior> textWaveBehaviorList;
    
    // Use this for initialization
    void Start ()
    {
        textWaveBehaviorList = new List<TextWaveBehavior>();
        //For testing let's make a wave here
        //GenerateTextWaveGameObject("123456", 36, 56f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TextWaveBehavior textWaveBehavior in textWaveBehaviorList)
        {
            textWaveBehavior.Update();
        }
    }

    private int CalculateHeightValueAt(int x, int y)
    {
        int height = 0;
        Color[] waveFontTextureColorArray = waveFontTexture2D.GetPixels(x, waveFontTexture2D.height-1-y, 1, 1, 0);
        if(!waveFontTextureColorArray[0].Equals(Color.white))
        {
            height = 1;
        }
        return height;
    }

    private int[,] CalculateCharacterAsHeightArray(char charToConvert)
    {
        int[,] characterAsArray = new int[characterWidth, characterHeight];
        int charAsInt = (int)(charToConvert);
        int TopLeftPixelX = charAsInt % numberCharactersPerRow;
        int TopLeftPixelY = charAsInt / numberCharactersPerRow * characterHeight;

        for (int x = 0; x < characterWidth; x++)
        {
            for (int y = 0; y < characterHeight; y++)
            {
                characterAsArray[x, y] = CalculateHeightValueAt(TopLeftPixelX + x, TopLeftPixelY + y);
            }
        }
        return characterAsArray;
    }

    public float getHeightAtPosition(int x, int y)
    {
        float height = 0f;

        foreach (TextWaveBehavior textWaveBehavior in textWaveBehaviorList)
        {
            if(textWaveBehavior.ContainsPosition(x,y))
            {
                height += textWaveBehavior.GetHeightAtPosition(x, y);
            }
        }
        return height * HeightScalar;
    }

    public TextWaveBehavior GenerateTextWaveGameObject(string textToGenerate, int maxLineLength, float startYPosition, float speed)
    {

        string[] textToGenerateLinesArray = textToGenerate.Split('\n');
        int numLines = textToGenerateLinesArray.Length;
        int arrayHeight = (numLines * characterHeight) + (numLines-1) * lineSpacing;
        int[,] heightArray = new int[maxLineLength, arrayHeight];
        
        for (int currentlineNum = 0; currentlineNum < numLines; currentlineNum++)
        {
            string currentLine = textToGenerateLinesArray[currentlineNum];
            for (int currentCharNum = 0; (currentCharNum * characterWidth < maxLineLength && currentCharNum < currentLine.Length); currentCharNum++)
            {
                //get charheightArray 4x6
                int[,] characterAsArray = CalculateCharacterAsHeightArray(currentLine[currentCharNum]);
                int TopLeftPixelX = currentCharNum * characterWidth;
                int TopLeftPixelY = currentlineNum * (characterHeight + lineSpacing);
                //Add Character heightmap to String heightmap
                for (int x = 0; x < characterWidth; x++)
                {
                    for (int y = 0; y < characterHeight; y++)
                    {
                        heightArray[TopLeftPixelX + x, TopLeftPixelY + y] = characterAsArray[x,y];
                    }
                }
            }
        }
        TextWaveBehavior textWaveBehavior = new global::TextWaveBehavior(heightArray, startYPosition, speed);
        textWaveBehaviorList.Add(textWaveBehavior);
        return textWaveBehavior;
    }
}
