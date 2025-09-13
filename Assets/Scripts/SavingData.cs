using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavingData 
{
    // Name of Main Character
    public string mainCharacter;
    public string SupportCharacter;

     // Backgorund
     public string textureBase64;  // Texture converted to Base64
     public Color color;           // Color of the Raw Image
     public Vector2 sizeDelta;
     // Backgorund 

    // Char1
    public string sourceImageName; // Name of the sprite (or path if needed)
    public Color color1;            // Color of the image
    public Vector2 size;           // Width and height
    public Vector3 position;       // Position (x, y, z)

    // Char2 
    public string sourceImageName2; // Name of the sprite (or path if needed)
    public Color color2;            // Color of the image
    public Vector2 size2;           // Width and height
    public Vector3 position2;       // Position (x, y, z)
                                    // Char2
   // Novel  State Save 
    public int CurrLine1;
    public string CurentFile1;
    public bool loadded;

    public string date; // currentDate = DateTime.Now.ToString("MMM dd, HH:mm:ss");

}
