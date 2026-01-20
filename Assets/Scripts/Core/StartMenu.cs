using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TESTING;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Application = UnityEngine.Application;
using Text = UnityEngine.UI.Text;


public class StartMenu : MonoBehaviour
{
    public GameObject menu, prequel, Loaddd, Help, Achiv, CharChoose, config, backgroundColorless, backgroundColor;
    public Text  End1, End2, End3, lang, ChaptNo, ChaptName;
    public Text[] Ui;
    public AudioSource MainMusick;
    public AudioClip Chapt1, Chapt2;
    public float typingSpeed = 0.05f;
    public Font Notes;

    private string[] languages = { "EN", "UA" };
    private int languageIndex = 0;

    public SpriteRenderer ChooseBackground;
    public GameObject[] Levels;
    public static string mainCharacter;
    public static string SupportCharacter;
    public int level, currentlevel;

    public void Start()
    {
       
        languageIndex = System.Array.IndexOf(languages, TestDialogueFiles.Languague);
        if (languageIndex == -1) languageIndex = 0;

        level = PlayerPrefs.GetInt("level", 1);
        switch (level)
        {
            case 2:
                backgroundColorless.SetActive(false);
                backgroundColor.SetActive(true);

                Color blue = new Color(74f / 255f, 166f / 255f, 215f / 255f);

                foreach (Text txt in Ui)
                {
                    txt.color = blue;
                }

                Debug.Log("Level2");
                break;
            case 3:
                Debug.Log("a is a string");
                break;
            case 4:
                Debug.Log("a is a string");
                break;
            case 5:
                Debug.Log("a is a string");
                break;

            default:
                backgroundColorless.SetActive(true);
                backgroundColor.SetActive(false);

                Color gray = new Color(144f / 255f, 144f / 255f, 144f / 255f);

                foreach (Text txt in Ui)
                {
                    txt.color = gray;
                }

                Debug.Log("Level1");
                break;
        }
        // Add button listener
    }
    public void HelpMenu()
    {
        Help.SetActive(true);
    }

    public void ConfigMenu()
    {
        config.SetActive(true);
        lang.text = TestDialogueFiles.Languague;
    }

    public void closeConfigMenu()
    {
        config.SetActive(false);
    }

    public void LangChange()
    {
        languageIndex = (languageIndex + 1) % languages.Length;
        TestDialogueFiles.Languague = languages[languageIndex]; // Update static variable
        lang.text = TestDialogueFiles.Languague;

        switch (languageIndex)
        {
            case 0:
                ENMenu instance0 = FindObjectOfType<ENMenu>(); // Find in the scene
                if (instance0 != null)
                {
                    instance0.Translate();
                }
                else
                {
                    Debug.LogError("ENMenu not found in the scene!");
                }
                break;

            case 1:
                UAMenu instance1 = FindObjectOfType<UAMenu>(); // Find in the scene
                if (instance1 != null)
                {
                    instance1.Translate();
                }
                else
                {
                    Debug.LogError("UAMenu not found in the scene!");
                }
                break;

            default:
                Debug.LogError("Language error (out of range)");
                break;
        }
    }


      

    public void Achivments()
    {
        Achiv.SetActive(true);
        PlayerPrefs.GetString(CMD_Database_Estansions_Example.End1);
        PlayerPrefs.GetString(CMD_Database_Estansions_Example.End2);
        PlayerPrefs.GetString(CMD_Database_Estansions_Example.End3);

        Debug.Log(CMD_Database_Estansions_Example.End1);
        if (CMD_Database_Estansions_Example.End1 != "" && CMD_Database_Estansions_Example.End1 != null)
        {
            End1.gameObject.SetActive(true);
        }
        Debug.Log(CMD_Database_Estansions_Example.End2);
        if (CMD_Database_Estansions_Example.End2 != "" && CMD_Database_Estansions_Example.End2 != null)
        {
            End2.gameObject.SetActive(true);
        }
        Debug.Log(CMD_Database_Estansions_Example.End3);
        if (CMD_Database_Estansions_Example.End3 != "" && CMD_Database_Estansions_Example.End3 != null)
        {
            End3.gameObject.SetActive(true);
        }
    }
    public void CloseAchivments()
    {
        Achiv.SetActive(false);
    }
    public void CloseHelpMenu()
    {
        Help.SetActive(false);
    }
    ////////////////////////////////////// EXIT BUTTON

    public void GameQuit()
    {
        Application.Quit();
    }
    ////////////////////////////////////// QUIT BUTTON


    ////////////////////////////////////////// NEW GAME 


    private IEnumerator ShowText2(string chaptNoText, string chaptNameText)
    {
     float floatAmplitude = 1f;   // how high letters bounce
     float floatSpeed = 2f;        // bounce speed
     float scaleAmplitude = 0.1f;  // scale bounce amount
     float scaleSpeed = 2f;        // scale bounce speed
     float letterDelay = 0.1f;     // delay between letters
     float duration = 0.4f;          // total animation time per letter



        // -------------------------
        // Setup Chapter Number
        // -------------------------
        ChaptNo.text = "";
        ChaptName.text = "";
        ChaptNo.font = Notes;
        ChaptName.font = Notes;

        Vector3 noInitialPos = ChaptNo.rectTransform.anchoredPosition;
        noInitialPos.x -= 0.2f;
        Vector3 nameInitialPos = ChaptName.rectTransform.anchoredPosition;
        nameInitialPos.x -= 1.1f;
        Vector3 noInitialScale = ChaptNo.rectTransform.localScale;
        Vector3 nameInitialScale = ChaptName.rectTransform.localScale;

        // Small offsets (optional) for style
        noInitialPos.x -= 2f;
        noInitialPos.y += 1f;

        // -------------------------
        // Animate Chapter Number letters
        // -------------------------
        for (int i = 0; i < chaptNoText.Length; i++)
        {
            ChaptNo.text += chaptNoText[i];

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float time = Time.time;
                float phase = i * 0.3f;

                // Bounce
                float yOffset = Mathf.Sin(time * floatSpeed + phase) * floatAmplitude;
                float scale = 1f + Mathf.Sin(time * scaleSpeed + phase) * scaleAmplitude;

                ChaptNo.rectTransform.anchoredPosition = noInitialPos + new Vector3(0, yOffset, 0);
                ChaptNo.rectTransform.localScale = noInitialScale * scale;

                yield return null;
            }

            yield return new WaitForSeconds(letterDelay);
        }

        // -------------------------
        // Animate Chapter Name letters (same effect)
        // -------------------------
        for (int i = 0; i < chaptNameText.Length; i++)
        {
            ChaptName.text += chaptNameText[i];

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float time = Time.time;
                float phase = i * 0.3f;

                // Bounce
                float yOffset = Mathf.Sin(time * floatSpeed + phase) * floatAmplitude;
                float scale = 1f + Mathf.Sin(time * scaleSpeed + phase) * scaleAmplitude;

                ChaptName.rectTransform.anchoredPosition = nameInitialPos + new Vector3(0, yOffset, 0);
                ChaptName.rectTransform.localScale = nameInitialScale * scale;

                yield return null;
            }

            yield return new WaitForSeconds(letterDelay);
        }

        // -------------------------
        // Gentle bounce for both texts
        // -------------------------
        float bounceTime = 0f;
        float totalBounceDuration = 3f; // bounce duration
        while (bounceTime < totalBounceDuration)
        {
            bounceTime += Time.deltaTime;
            float time = Time.time;

            // Bounce Chapter Number letters
            for (int i = 0; i < chaptNoText.Length; i++)
            {
                float phase = i * 0.3f;
                float yOffset = Mathf.Sin(time * floatSpeed + phase) * floatAmplitude;
                float scale = 1f + Mathf.Sin(time * scaleSpeed + phase) * scaleAmplitude;
                ChaptNo.rectTransform.anchoredPosition = noInitialPos + new Vector3(0, yOffset, 0);
                ChaptNo.rectTransform.localScale = noInitialScale * scale;
            }

            // Bounce Chapter Name letters
            for (int i = 0; i < chaptNameText.Length; i++)
            {
                float phase = i * 0.3f;
                float yOffset = Mathf.Sin(time * floatSpeed + phase) * floatAmplitude;
                float scale = 1f + Mathf.Sin(time * scaleSpeed + phase) * scaleAmplitude;
                ChaptName.rectTransform.anchoredPosition = nameInitialPos + new Vector3(0, yOffset, 0);
                ChaptName.rectTransform.localScale = nameInitialScale * scale;
            }

            yield return null;
        }

        // Reset to original positions/scales
        ChaptNo.rectTransform.anchoredPosition = noInitialPos;
        ChaptNo.rectTransform.localScale = noInitialScale;
        ChaptName.rectTransform.anchoredPosition = nameInitialPos;
        ChaptName.rectTransform.localScale = nameInitialScale;

        StartCoroutine(StartGamee());
    }


    public void Cheatlvl1()
    {

        Debug.Log("CheatLvl1");
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.Save();

        level = PlayerPrefs.GetInt("level", 1);
        SceneManager.LoadScene("Start");

    }

    public void Cheatlvl2()
    {
        Debug.Log("CheatLvl2");
        PlayerPrefs.SetInt("level", 2);
        PlayerPrefs.Save();

        level = PlayerPrefs.GetInt("level", 2);
        SceneManager.LoadScene("Start");
    }

    private IEnumerator ShowText(string text1, string text2)
    {

        // Очищуємо початково
        ChaptNo.text = "";
        ChaptName.text = "";
        yield return new WaitForSeconds(2f);
        // Друкуємо перший рядок
        foreach (char c in text1)
        {
            ChaptNo.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.5f); // пауза між рядками

        // Друкуємо другий рядок
        foreach (char c in text2)
        {
            ChaptName.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        StartCoroutine(StartGamee());
    }


    private IEnumerator StartGamee()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
            SceneManager.LoadScene("1");
    }

   

    public void ProcedePrequel()
    {
        CharChoose.SetActive(false);
        prequel.SetActive(true);

       
    }

    public void chosinGGNeil()
    {
        PlayerPrefs.SetInt("currentlevel", 1);
        PlayerPrefs.Save();
        TestDialogueFiles.mainCharacter = "Neil";
        TestDialogueFiles.SupportCharacter = "Rey";
        MainMusick.clip = Chapt1;
        MainMusick.loop = false;
        MainMusick.Play();
        StartCoroutine(ShowText("Chapter I", "Beyond The Gray"));

    }

    public void chosinGGSoundiel()
    {
        PlayerPrefs.SetInt("currentlevel", 2);
        PlayerPrefs.Save();
        TestDialogueFiles.mainCharacter = "Soundiel";
        TestDialogueFiles.SupportCharacter = "Rey";
        MainMusick.clip = Chapt2;
        MainMusick.loop = false;
        MainMusick.Play();
        StartCoroutine(ShowText2("Chapter ll", "Soundless"));
    }



    public void StartGame()
    {
        menu.SetActive(false);
        CharChoose.SetActive(true);
        MainMusick.Stop();
        Sprite newSprite;

        switch (level)
        {
            case 2:

                 newSprite = Resources.Load<Sprite>("Backgrounds/Startgame");

                if (newSprite != null)
                {
                    ChooseBackground.sprite = newSprite;
                }

                foreach (GameObject Character in Levels)
                {
                    Character.SetActive(false);
                }
                Levels[1].SetActive(true);

                Debug.Log("Level2");
                break;
            case 3:
                Debug.Log("a is a string");
                break;
            case 4:
                Debug.Log("a is a string");
                break;
            case 5:
                Debug.Log("a is a string");
                break;

            default:

                newSprite = Resources.Load<Sprite>("Backgrounds/StartGameMonochrome");

                if (newSprite != null)
                {
                    ChooseBackground.sprite = newSprite;
                }

                foreach (GameObject Character in Levels)
                {
                    Character.SetActive(false);
                }
                Levels[0].SetActive(true);
                break;
        }
    }
 ////////////////////////////////////////// NEW GAME 
 ///

//////////////////////////////////////////Load
    public void Loadin()
    {
        MenuButtons.Instance.Load = true;
        Loaddd.SetActive(true);
        MenuButtons.Instance.assignScreens();
    }


}
