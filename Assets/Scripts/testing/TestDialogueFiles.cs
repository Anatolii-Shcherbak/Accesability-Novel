using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using System.IO;
public class TestDialogueFiles : MonoBehaviour
{

    private string currentAct;
    TextAsset textAsset;
    List<string> lines;
    private string savePath, savePath1;
    private string buttName;
    public static string Languague = "EN";
    public static string mainCharacter = "Rey";
    public static string SupportCharacter = "Mayua";

    void Start()
      {
        savePath1 = Path.Combine(Application.persistentDataPath, "StaticData.json");

        if (!File.Exists(savePath1))
        {
            Debug.Log("StaticData.json does not exist. Creating a new file...");

            // Create a new instance of default static data
            StatickSaveData defaultData = new StatickSaveData
            {
                ButtName = "" // Assign default values
            };
            string defaultJson = JsonUtility.ToJson(defaultData, true);

            // Write the JSON to the file
            File.WriteAllText(savePath1, defaultJson);

            Debug.Log("StaticData.json created successfully.");
        }


        string json1 = File.ReadAllText(savePath1);
          StatickSaveData dat1 = JsonUtility.FromJson<StatickSaveData>(json1);

          buttName = dat1.ButtName;
        //  Debug.Log(buttName);

          if (!string.IsNullOrEmpty(buttName))
          {
            //  Debug.Log("Working?");
              savePath = Path.Combine(Application.persistentDataPath, "SavingData" + buttName + ".json");

              string json = File.ReadAllText(savePath);
              SavingData data = JsonUtility.FromJson<SavingData>(json);

              SaveScript.Instance.Loaded = data.loadded;
                currentAct = data.CurentFile1;
               SaveScript.Instance.CurentFile = currentAct;
              

          }
             // Debug.Log(SaveScript.Instance.Loaded);
           //   Debug.Log(SaveScript.Instance.CurrLine);


          if (SaveScript.Instance.Loaded)
          {
              Debug.Log($"Loaded save file: {currentAct}");
          }
          else
          {
              currentAct = "Chapter1"; // Default to Chapter1 if no save
              SaveScript.Instance.CurentFile = currentAct;
              Debug.Log($"Starting new game with file: {currentAct}");
          }

        savePath = Path.Combine(Application.persistentDataPath, "SavingData" + buttName + ".json");
        if (File.Exists(savePath))
        {
            string json2 = File.ReadAllText(savePath);
            SavingData data2 = JsonUtility.FromJson<SavingData>(json2);
            data2.loadded = false;

            string updatedJson = JsonUtility.ToJson(data2, true);
            File.WriteAllText(savePath, updatedJson);
        }



        StartConversation();

      }
      
    void StartConversation()
    {
        textAsset = null;
        lines = new List<string>();
        Resources.UnloadUnusedAssets();
        Debug.Log($"CurrentAct before loading: {currentAct}");
        
        textAsset = Resources.Load<TextAsset>(Languague +"/" + mainCharacter + "/" + currentAct);

        if (textAsset == null)
        {
            Debug.LogError($"File not found: {currentAct}");
            return;
        }

        Debug.Log($"Successfully loaded file: {currentAct}");
       
        lines.AddRange(textAsset.text.Split('\n'));

        DialougeSystem.instance.Say(lines);
       
        // List<string> lines = FileManager.ReadTextAsset(currentAct);
    }

    public void OnButtonClicked(string name)
    {
        string pas = name[name.Length - 1].ToString();
        ChoosenPass(pas);
    }

    public void ChoosenPass(string pas)
    {
        try
        {
            currentAct += "." + pas.ToString();
            SaveScript.Instance.CurentFile = currentAct;
            StartConversation();
        }
        catch
        {
        }


    }
}
