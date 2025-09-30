using System.Collections;
using System.Collections.Generic;
using TESTING;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject menu, prequel, Loaddd, Help, Achiv, CharChoose, config;
    public Text  End1, End2, End3, lang, ChaptNo, ChaptName;
    public AudioSource MainMusick;
    public AudioClip newClip;
    public float typingSpeed = 0.05f;

    private string[] languages = { "EN", "UA" };
    private int languageIndex = 0;

    public static string mainCharacter;
    public static string SupportCharacter;

    public void Start()
    {
       
        languageIndex = System.Array.IndexOf(languages, TestDialogueFiles.Languague);
        if (languageIndex == -1) languageIndex = 0;

       
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
        if (CMD_Database_Estansions_Example.End1 == "" || CMD_Database_Estansions_Example.End1 == null)
        {
            End1.gameObject.SetActive(true);
        }
        Debug.Log(CMD_Database_Estansions_Example.End2);
        if (CMD_Database_Estansions_Example.End2 == "" || CMD_Database_Estansions_Example.End2 == null)
        {
            End2.gameObject.SetActive(true);
        }
        Debug.Log(CMD_Database_Estansions_Example.End3);
        if (CMD_Database_Estansions_Example.End3 == "" || CMD_Database_Estansions_Example.End3 == null)
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

     private IEnumerator ShowText()
    {
        string text1 = "Chapter I";
        string text2 = "Beyond The Gray";

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
        MainMusick.clip = newClip;
        MainMusick.loop = false;
        MainMusick.Play();
        StartCoroutine(ShowText());
    }

    public void chosinGGRey()
    {

        TestDialogueFiles.mainCharacter = "Rey";
        TestDialogueFiles.SupportCharacter = "Mayua";
    }

    public void chosinGGMayua()
    {

        TestDialogueFiles.mainCharacter = "Mayua";
        TestDialogueFiles.SupportCharacter = "Rey";
    }
    public void StartGame()
    {
        menu.SetActive(false);
        CharChoose.SetActive(true);
        MainMusick.Stop();

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
