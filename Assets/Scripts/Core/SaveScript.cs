using DIALOGUE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Windows;
using File = System.IO.File;
//using File = System.IO.File;


public class SaveScript : MonoBehaviour
{
    public GameObject Pages;
    public GameObject confirmation;
    public Text what;
    public GameObject MenuOverlay;
    private string screenshotPath;
    public string savePath;
    public bool Loaded;
    string savePath1;
    public string Backrg;
    public string char1Name, char2Name;
    private RawImage rawImage;
    private Image Char1, Char2;
    public int CurrLine = 0;
    public string CurentFile;
    public string buttName;
    SavingData data = new SavingData();
    StatickSaveData dat = new StatickSaveData();
    public string butn;
    public Button qbut;
    public bool Qsavee = false;
    public GameObject qsv;
    public static SaveScript Instance { get; private set; }
    public void Awake()
    {
        Backrg = "Backgrund";
        char1Name = "Character1"; char2Name = "Character2";
        rawImage = GameObject.Find(Backrg)?.GetComponent<RawImage>();
        Char1 = GameObject.Find(char1Name)?.GetComponent<Image>();
        Char2 = GameObject.Find(char2Name)?.GetComponent<Image>();
        Instance = this;
    }

    public void PlotSave()
    {
        QuickSaveExecution();
        SavePlotData();
    }
    public void SavePlotData()
    {
        data.CurrLine1 = CurrLine + 2;
        data.CurentFile1 = CurentFile;
        data.mainCharacter = TestDialogueFiles.mainCharacter;
        data.SupportCharacter = TestDialogueFiles.SupportCharacter;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

    }
    public void QuickSaveExecution()
    {
        butn = "1";
        MenuButtons.Instance.ButtName = butn;
        screenshotPath = Path.Combine(Application.persistentDataPath, "screenshot" + butn + ".png");
        if (File.Exists(screenshotPath))
        {
            File.Delete(screenshotPath);
        }
        Qsavee = true;
        StartCoroutine(UpdateButtonImage());

        savePath = Path.Combine(Application.persistentDataPath, "SavingData" + butn + ".json");
        SaveBackground(rawImage);
        SaveImage(Char1);
        SaveImage2(Char2);
        SaveCurrentDataaa();
        ShowSavedMessage();
    }
    public void QuickSave()
    {
        QuickSaveExecution();
        SaveData();
    }


    public void QuickLoad()
    {
        butn = "1";
        MenuButtons.Instance.ButtName = butn;
        savePath = Path.Combine(Application.persistentDataPath, "SavingData" + butn + ".json");
        if (File.Exists(savePath))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("1");
            Loaded = true;
        }
      
    }


    public void ActualSave()
    {
        if (MenuButtons.Instance.Save == true)
        {
            butn = MenuButtons.Instance.ButtName;
            screenshotPath = Path.Combine(Application.persistentDataPath, "screenshot" + butn + ".png");
            if (File.Exists(screenshotPath))
            {
                File.Delete(screenshotPath);
            }

            confirmation.SetActive(false);
            MenuOverlay.SetActive(false);
            StartCoroutine(UpdateButtonImage());

            savePath = Path.Combine(Application.persistentDataPath, "SavingData" + butn + ".json");

            PlayerInputManager.Instance.dynamicBool = true;
            SaveCurrentDataaa();
            SaveBackground(rawImage);
            SaveImage(Char1);
            SaveImage2(Char2);
            SaveData();
           

        }

        if (MenuButtons.Instance.Load == true)
        {
           
            butn = MenuButtons.Instance.ButtName;
            savePath = Path.Combine(Application.persistentDataPath, "SavingData" + butn + ".json");
          
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene("1");
                Loaded = true;
        }
    }

    public void SaveCurrentDataaa()
    {
        int number;
        if (int.TryParse(butn, out number))
        {
            data.date = DateTime.Now.ToString("MMM dd, HH:mm:ss");
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
        }
    }

    public void lod()
    {
        Debug.Log(butn);
       
        if (File.Exists(savePath))
        {
            // Read and load existing data
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<SavingData>(json);
        }
        else
        {
            Debug.LogWarning("No save file found. Creating new save data.");
            data = new SavingData(); // Initialize new data object if no file exists
        }

        Loaded = true;
        data.loadded = Loaded;

        // Write updated data back to the file
        string updatedJson = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, updatedJson);

        Debug.Log("Data updated and saved during 'lod'. File path: " + savePath);
    }

    public void LoadData()
    {
        string json = File.ReadAllText(savePath);
        SavingData data = JsonUtility.FromJson<SavingData>(json);
        CurrLine = data.CurrLine1;
        CurentFile = data.CurentFile1;
        TestDialogueFiles.mainCharacter = data.mainCharacter;
        TestDialogueFiles.SupportCharacter = data.SupportCharacter;

        lok();
        lod();
    }
    public void lok()
        {
        savePath1 = Path.Combine(Application.persistentDataPath, "StaticData.json");
        //   Debug.Log(butn);

        dat.ButtName = butn;

        string json3 = JsonUtility.ToJson(dat, true);
        File.WriteAllText(savePath1, json3);
    }

    public void SaveCurrentData()
    {

      //  data.dates[butn]
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
    public void SaveData()
    {
        data.CurrLine1 = CurrLine;
        data.CurentFile1 = CurentFile;
        data.mainCharacter = TestDialogueFiles.mainCharacter;
        data.SupportCharacter = TestDialogueFiles.SupportCharacter;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Unregister the callback to prevent duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Loaded = true;
        rawImage = GameObject.Find(Backrg)?.GetComponent<RawImage>();
        Char1 = GameObject.Find(char1Name)?.GetComponent<Image>();
        Char2 = GameObject.Find(char2Name)?.GetComponent<Image>();


        savePath = Path.Combine(Application.persistentDataPath, "SavingData" + butn + ".json");

        // Reapply saved data after the scene is loaded
        LoadBackground(rawImage);
        LoadImage(Char1);
        LoadImage2(Char2);
        LoadData();

    }

    public void LoadBackground(RawImage rawImage)
    {

         if (!File.Exists(savePath))
          {
              Debug.LogWarning("Save file not found!");
              return;
          } 
   
        string json = File.ReadAllText(savePath);
        SavingData data = JsonUtility.FromJson<SavingData>(json);

        // Load Texture
        if (!string.IsNullOrEmpty(data.textureBase64))
        {
            byte[] textureBytes = System.Convert.FromBase64String(data.textureBase64); // Convert Base64 to bytes
            Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D
            texture.LoadImage(textureBytes); // Load the image data into the texture
            rawImage.texture = texture;
        }

        // Load Color
        rawImage.color = data.color;

        // Load Size
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = data.sizeDelta;

        Debug.Log("RawImage loaded!");
    }

    public void LoadImage(Image Char)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("JSON file not found at: " + savePath);
            return;
        }

        // Read JSON from file
        string json = File.ReadAllText(savePath);

        // Deserialize JSON into a SavingData object
        SavingData data = JsonUtility.FromJson<SavingData>(json);

        // Apply the loaded data to the target Image
        if (Char != null)
        {
            Char.color = data.color1;
            Char.rectTransform.sizeDelta = data.size;
            Char.rectTransform.anchoredPosition3D = data.position;

            // Load sprite by name (requires sprites in Resources folder)
            if (!string.IsNullOrEmpty(data.sourceImageName))
            {
               
                Sprite loadedSprite = Resources.Load<Sprite>("Characters/"  +   data.sourceImageName);
                Debug.Log("Characters/"  + data.sourceImageName);
                if (loadedSprite != null)
                {
                    Char.sprite = loadedSprite;
                }
                else
                {
                    Debug.LogError("Sprite not found in Resources: " + data.sourceImageName);
                }
            }
        }

        Debug.Log("Data loaded and applied to the target Image.");
    }
    public void LoadImage2(Image Char)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError("JSON file not found at: " + savePath);
            return;
        }

        // Read JSON from file
        string json = File.ReadAllText(savePath);

        // Deserialize JSON into a SavingData object
        SavingData data = JsonUtility.FromJson<SavingData>(json);

        // Apply the loaded data to the target Image
        if (Char != null)
        {
            Char.color = data.color2;
            Char.rectTransform.sizeDelta = data.size2;
            Char.rectTransform.anchoredPosition3D = data.position2;

            // Load sprite by name (requires sprites in Resources folder)
            if (!string.IsNullOrEmpty(data.sourceImageName2))
            {
                Sprite loadedSprite = Resources.Load<Sprite>("Characters/" + data.sourceImageName2);
                Debug.Log("Characters/"  + "/" + data.sourceImageName2);
                if (loadedSprite != null)
                {
                    Char.sprite = loadedSprite;
                }
                else
                {
                    Debug.LogError("Sprite not found in Resources: " + data.sourceImageName2);
                }
            }
        }

        Debug.Log("Data loaded and applied to the target Image.");
    }

    public void SaveImage2(Image Char)
    {
        data.sourceImageName2 = Char.sprite ? Char.sprite.name : "NoSprite";
        data.color2 = Char.color;
        data.size2 = Char.rectTransform.sizeDelta;
        data.position2 = Char.rectTransform.anchoredPosition3D;

        // Convert to JSON and save to file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

    }
    public void SaveImage(Image Char1)
    {
        data.sourceImageName = Char1.sprite ? Char1.sprite.name : "NoSprite";
        data.color1 = Char1.color;
        data.size = Char1.rectTransform.sizeDelta;
        data.position = Char1.rectTransform.anchoredPosition3D;
       
        // Convert to JSON and save to file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

    }
    public void SaveBackground(RawImage rawImage)
    {
        // Save Texture (convert to Base64 string for serialization)
        if (rawImage.texture is Texture2D texture2D)
        {
            byte[] textureBytes = texture2D.EncodeToPNG(); // Convert texture to PNG bytes
            data.textureBase64 = System.Convert.ToBase64String(textureBytes); // Convert bytes to Base64
        }
        else
        {
            Debug.LogWarning("RawImage texture is not a Texture2D, skipping texture save.");
        }

        // Save Color
        data.color = rawImage.color;

        // Save Size
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        data.sizeDelta = rectTransform.sizeDelta;

        // Serialize to JSON
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"RawImage saved to {savePath}");

    }

    private IEnumerator UpdateButtonImage()
    {
        screenshotPath = Path.Combine(Application.persistentDataPath, "screenshot" + MenuButtons.Instance.ButtName + ".png");
 
        ScreenCapture.CaptureScreenshot(screenshotPath);
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
        // Wait until the screenshot file is saved
        while (!File.Exists(screenshotPath))
        {
            yield return null;
        }

        // Load the screenshot as a texture
        byte[] imageData = File.ReadAllBytes(screenshotPath);
        Texture2D screenshotTexture = new Texture2D(2, 2);
        screenshotTexture.LoadImage(imageData);

        // Create a Sprite from the texture
        Rect rect = new Rect(0, 0, screenshotTexture.width, screenshotTexture.height);
        Sprite screenshotSprite = Sprite.Create(screenshotTexture, rect, new Vector2(0.5f, 0.5f));

        if(Qsavee == true)
         Qsave(screenshotSprite);
        else
        usualSave(screenshotSprite);
      
        // Assign the sprite to the button's image
        //MenuButtons.Instance.clicedButton.image.sprite = screenshotSprite; 

    }
    private void usualSave(Sprite sprite) {
        MenuButtons.Instance.clicedButton.image.sprite = sprite;
    }
    private void Qsave(Sprite screenshotSprite)
    {
        Qsavee = false;
        qbut.image.sprite = screenshotSprite;
    }
    public void ChangePage(GameObject buttonClicked)
    {
        string buttonName = buttonClicked.name;
        foreach (Transform child in Pages.transform)
        {
            if (child.gameObject.name == buttonName)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        MenuButtons.Instance.assignScreens();
    }


    public void ShowSavedMessage()
    { 
            StartCoroutine(DisplayMessageCoroutine(1f));
    }

    private IEnumerator DisplayMessageCoroutine(float duration)
    {
        PlayerInputManager.Instance.dynamicBool = false;
        qsv.SetActive(true);  // Ensure it's visible
        Time.timeScale = 0f; // Stop all gameplay activities
        

        yield return new WaitForSecondsRealtime(duration);  // Wait for 2 seconds (unaffected by Time.timeScale)

        qsv.gameObject.SetActive(false);  // Hide the message
        Time.timeScale = 1f;  // Resume gameplay
        PlayerInputManager.Instance.dynamicBool = true;

    }



public void savin()
    {
        MenuButtons.Instance.Save = true;
        MenuButtons.Instance.Load = false;
    }

    public void Loadin()
    {
        MenuButtons.Instance.Save = false;
        MenuButtons.Instance.Load = true;
    }

    public void Returning()
    {
        MenuOverlay.SetActive(false);
        MenuButtons.Instance.Save = false;
        MenuButtons.Instance.Load = false;
        PlayerInputManager.Instance.dynamicBool = true;

    }
    public void Click()
    {
        StartCoroutine(HandleButtonClick());
    }

    private IEnumerator HandleButtonClick()
    {
        yield return new WaitForEndOfFrame();

        butn = MenuButtons.Instance.ButtName;

        if (MenuButtons.Instance.Save == true)
            {
                Pages.SetActive(false);
                 confirmation.SetActive(true);
                what.text = "SAVE?";
            }

        if (MenuButtons.Instance.Load == true)
            {   

            Debug.Log(butn);
            savePath = Path.Combine(Application.persistentDataPath, "SavingData" + butn + ".json");
                if (File.Exists(savePath))
                {
                Pages.SetActive(false);
                confirmation.SetActive(true);
                what.text = "LOAD?";

                }
            }
        
    }

    public void Noclick()
    {
        confirmation.SetActive(false);
        Pages.SetActive(true);
    }

}
