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

    [SerializeField]
    Toggle[] drawModeToggles;

    int renderModeIndex;

    ImageData imageData;
    Texture2D textureForRawImage;
    RenderTexture renderTexture;


    private void Awake()
    {
        for(int i = 0; i<drawModeToggles.Length; i++) 
        {
            int index = i;
            Toggle toggle = drawModeToggles[index];
            toggle.onValueChanged.AddListener(isOn => {
                renderModeIndex = index;
            });
        }
    }

    void Start()
    {
        string path = "file://" + Application.persistentDataPath;
        Debug.Log(path);

        Texture2D originalTexture = Resources.Load("image-20mb") as Texture2D;

        Texture2D texture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
        texture.SetPixels(originalTexture.GetPixels());

        var format = texture.format;
        var byteArray = texture.GetRawTextureData();

        ImageData data = new ImageData()
        {
            byteArray = byteArray,
            format = texture.format,
            width = texture.width,
            height = texture.height
        };

        imageData = data;
    }


    private void DrawImage(ImageData data)
    {
        if (textureForRawImage == null)
        {
            textureForRawImage = new Texture2D(data.width, data.height, data.format, false);
        }

        textureForRawImage.LoadRawTextureData(data.byteArray);
        textureForRawImage.Apply();

        rawImage.texture = textureForRawImage;

    }

    private void DrawImageWithBlit(ImageData data)
    {
        if (textureForRawImage == null)
        {
            textureForRawImage = new Texture2D(data.width, data.height, data.format, false);
        }

        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(data.width, data.height, 32);
            rawImage.texture = renderTexture;
        }

        textureForRawImage.LoadRawTextureData(data.byteArray);
        textureForRawImage.Apply();

        //rawImage.texture = textureForRawImage;

        Graphics.Blit(textureForRawImage, renderTexture);
    }

    private void DrawImageByGrid(ImageData data)
    {
        if (textureForRawImage == null)
        {
            textureForRawImage = new Texture2D(data.width, data.height, data.format, false);
        }

        Color[] colors = new Color[data.width * data.height];

        int index = 0;
        int byteIndex = 0;

        for(int y = 0; y<colors.Length; y++)
        {
            colors[index] = new Color32(data.byteArray[byteIndex], data.byteArray[byteIndex+1], data.byteArray[byteIndex+2], data.byteArray[byteIndex+3]);
            byteIndex += 4;
            index++;
        }

        textureForRawImage.SetPixels(colors);
        textureForRawImage.Apply();
        rawImage.texture = textureForRawImage;
    }

    private void Update()
    {
        switch (renderModeIndex)
        {
            case 0:
                DrawImage(imageData);
                break;
            case 1:
                DrawImageWithBlit(imageData);
                break;
            case 2:
                DrawImageByGrid(imageData);
                break;
            default:
                break;
        }
        DrawImage(imageData);

        frameRateText.text = frameRateCounter.GetFrameRate().ToString();
    }
}
