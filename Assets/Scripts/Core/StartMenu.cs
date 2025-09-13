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
    public TextMeshProUGUI textMeshPro;
    public Text sign, End1, End2, End3, lang;
    public string newText;
    public int update = 0;
    private bool isTyping = false;
    private Coroutine typeCoroutine;
    public AudioSource MainMusick;
    public AudioClip newClip;

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
    private IEnumerator WaitAndDisplayText()
    {
        while (update < 6)  // Continue until all lines are displayed
        {
            float elapsedTime = 0f;
            bool isClicked = false;

            // Wait for either 2 seconds or a click
            while (elapsedTime < 2f && !isClicked)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isClicked = true;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            update++;
            typeCoroutine = StartCoroutine(TypeText(GetNextLine()));

            while (isTyping)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StopCoroutine(typeCoroutine);
                    textMeshPro.text = newText + GetNextLine();
                    newText += GetNextLine();  // Update accumulated text
                    isTyping = false;  // Mark typing as complete
                }
                yield return null;
            }
        }
        sign.text = "Mayua";
        StartCoroutine(StartGamee());
    }
    private IEnumerator StartGamee()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 10f && !Input.GetMouseButtonDown(0))
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
            SceneManager.LoadScene("1");
    }
 private IEnumerator TypeText(string line)
    {
        isTyping = true;
        textMeshPro.text = newText;  // Reset to show accumulated text
        // Display the line one character at a time
        for (int i = 0; i <= line.Length; i++)
        {
            textMeshPro.text = newText + line.Substring(0, i);
            yield return new WaitForSeconds(0.05f);  // Adjust typing speed here
        }
        // When done, mark typing as complete and update accumulated text
        newText += line;
        isTyping = false;
    }

    private string GetNextLine()
    {
        switch (update)
        {
            case 1:
                return "";

            case 2:
                return  "\nPeople like to talk about equality.";

            case 3:
                return "\nHowever, people are not equal \nin rights from birth. ";

            case 4:
                return "\nThis truth is well known to slum dwellers...";
               
            case 5:
                return "\nEven so, WE will fight for a better future,";
              
            case 6:
                return "\nOUR future...";
              default: return "";
        }

    }

    public void ProcedePrequel()
    {
        CharChoose.SetActive(false);
        prequel.SetActive(true);
        MainMusick.clip = newClip;
        MainMusick.Play();
        StartCoroutine(WaitAndDisplayText());
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
