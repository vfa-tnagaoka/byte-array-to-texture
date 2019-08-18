using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageData
{
    public byte[] byteArray;
    public TextureFormat format;
    public int width;
    public int height;
}

public class Main : MonoBehaviour
{
    [SerializeField]
    GameSolution.FrameRateCounter frameRateCounter = default;
    [SerializeField]
    Text frameRateText = default;
    [SerializeField]
    RawImage rawImage = default;

    ImageData imageData;

    // Start is called before the first frame update
    void Start()
    {
        string path = "file://" + Application.persistentDataPath;
        Debug.Log(path);

        Texture2D texture = Resources.Load("image") as Texture2D;

        //Texture2D texture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
        //texture.SetPixels(originalTexture.GetPixels());

        var format = texture.format;
        var byteArray = texture.GetRawTextureData();

        ImageData data = new ImageData()
        {
            byteArray = byteArray,
            format = texture.format,
            width = texture.width,
            height = texture.height
        };

        //DrawImage(data);
        imageData = data;
    }


    private void DrawImage(ImageData data)
    {
        Texture2D texture = new Texture2D(data.width, data.height, data.format, false);

        //Color[] colors = new Color[data.width * data.height];

        //int index = 0;
        //int byteIndex = 0;

        //for(int y = 0; y<data.height; y++)
        //{
        //    for(int x = 0; x<data.width; x++)
        //    {
        //        if(byteIndex < data.byteArray.GetLength(0) - 100)
        //        colors[index] = new Color(data.byteArray[byteIndex++], data.byteArray[byteIndex++], data.byteArray[byteIndex++], data.byteArray[byteIndex]);
        //        index++;
        //    }
        //}

        //texture.SetPixels(colors);

        texture.LoadRawTextureData(data.byteArray);
        texture.Apply();

        rawImage.texture = texture;
    }

    private void Update()
    {
        DrawImage(imageData);
        frameRateText.text = frameRateCounter.GetFrameRate().ToString();
    }
}
