using DIALOGUE;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using System.Linq;


public class MenuButtons : MonoBehaviour
{
   public static MenuButtons Instance { get; private set; }
    public bool Save = false, Load = false, Inhistory = false, InAuto = false; 
    public GameObject MenuOverlay, Confirmation, pages, history, Char1, Char2, Autoo, EscMen;
    public ScrollRect scrollRect;
    public TextMeshProUGUI logText;
    private int maxLines =72;
    UnityEngine.UI.Button[] buttons;
    public UnityEngine.UI.Button clicedButton;
    public string ButtName, screenshotPath, dataPath;

    private void Awake()
    {
        Instance = this;
    }
    /// TEMP DECISION CHENGE
    public void Skip()
    {
        InAuto = true;
        StartCoroutine(AutoSkip());
    }
    private IEnumerator AutoSkip()
    {
        float timeElapsed = 0f;

        while (InAuto && PlayerInputManager.Instance.dynamicBool)
        {
            DialougeSystem.instance.OnUserPrompt_Next(); // Perform the action

            // Wait for the next frame and check InAuto and timeElapsed
            yield return null;

            timeElapsed += Time.deltaTime; // Increment time by the frame time

            // If 2 seconds have passed and InAuto is still true, continue.
            if (timeElapsed >= 2f)
            {
                timeElapsed = 0f; // Reset timer for the next 2-second wait cycle
            }

            // If InAuto is false, exit the loop and hide Autoo immediately
            if (!InAuto || !PlayerInputManager.Instance.dynamicBool)
            {
                yield break; // Exit the coroutine
            }
        }
    }
    /// TEMP DECISION CHENGE

    public void MenuEsc()
    {
        PlayerInputManager.Instance.dynamicBool = false;
        EscMen.SetActive(true);
    }

    public void EscYes()
    {
        SceneManager.LoadScene("Start");
    }

    public void EscNo()
    {
        PlayerInputManager.Instance.dynamicBool = true;
        EscMen.SetActive(false);
    }

    public void Auto()
    {
        Autoo.SetActive(true);
        InAuto = true;
        StartCoroutine(AutoProcess());

    }
    private IEnumerator AutoProcess()
    {
        while (InAuto && PlayerInputManager.Instance.dynamicBool)
        {
            DialougeSystem.instance.OnUserPrompt_Next(); // Perform the action

            // Wait for 2 seconds or break immediately if InAuto becomes false
            float elapsedTime = 0f;
            while (elapsedTime < 2.5f && InAuto && PlayerInputManager.Instance.dynamicBool)  // Loop until 2 seconds or InAuto is false
            {
                elapsedTime += Time.deltaTime; // Count time that has passed
                yield return null; // Yield every frame to check for InAuto change
            }

            if (!InAuto) // If InAuto is false, stop the process immediately
            {
                Autoo.SetActive(false); // Hide Autoo immediately
                yield break; // Exit the coroutine
            }
        }

        // This will run when the while loop exits
        Autoo.SetActive(false); // Ensure Autoo is hidden when exiting the loop
    
}

    public void Saving()
    {
        Save = true;
        Load = false;
        SaveLoadMenu();
    }
    public void SaveLoadMenu()
    {
        PlayerInputManager.Instance.dynamicBool = false;
        MenuOverlay.SetActive(true);
        Confirmation.SetActive(false);
        pages.SetActive(true);

        assignScreens();
    }
   public void assignScreens()
    {
        GameObject parentObject = GameObject.FindGameObjectWithTag("Pages");
        UnityEngine.UI.Button[] buttons = parentObject.GetComponentsInChildren<UnityEngine.UI.Button>(true);

        foreach (UnityEngine.UI.Button button in buttons)
        {
            
            screenshotPath = Path.Combine(Application.persistentDataPath, "screenshot" + button.name + ".png");
            if (System.IO.File.Exists(screenshotPath))
            {
                byte[] imageData = System.IO.File.ReadAllBytes(screenshotPath);
                Texture2D screenshotTexture = new Texture2D(2, 2);
                screenshotTexture.LoadImage(imageData);

                Rect rect = new Rect(0, 0, screenshotTexture.width, screenshotTexture.height);

                Sprite screenshotSprite = Sprite.Create(screenshotTexture, rect, new Vector2(0.5f, 0.5f));

                button.image.sprite = screenshotSprite;

                int number;
                if (int.TryParse(button.name, out number))
                {
                    dataPath = Path.Combine(Application.persistentDataPath, "SavingData" + button.name + ".json");
                    
                    string json = File.ReadAllText(dataPath);
                    SavingData dateData = JsonUtility.FromJson<SavingData>(json);

                    Transform dateTransform = button.GetComponentsInChildren<Transform>(true)
                                .FirstOrDefault(t => t.name == "Date");

                    Text dateText = dateTransform.GetComponent<Text>();
                    dateTransform.gameObject.SetActive(true);
                    dateText.text = dateData.date;
                  

                }

            }
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
    }

    void OnButtonClicked(UnityEngine.UI.Button clickedButton)
    {
        ButtName = clickedButton.name;
        clicedButton = clickedButton;
         Debug.Log($"Button {clickedButton.name} was clicked!");
    }

    public void Loading()
    {
        Save = false;
        Load = true;
        SaveLoadMenu();
    }


    public void HistoruShow()
    {

        if (Inhistory == false)
        {
            PlayerInputManager.Instance.dynamicBool = false;
            Char1.SetActive(false);
            Char2.SetActive(false);
            history.SetActive(true);
            Inhistory = true;
            ScrollToLastLine();
            return;
        }

        if (Inhistory == true)
        {
            PlayerInputManager.Instance.dynamicBool = true;
            history.SetActive(false);
            Char1.SetActive(true);
            Char2.SetActive(true);
            Inhistory = false;
            return;
        }
         void ScrollToLastLine()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(logText.rectTransform);

            // Calculate the total height of the text content
            float contentHeight = logText.preferredHeight;
            float viewportHeight = scrollRect.viewport.rect.height;

             string[] lines = logText.text.Split('\n');

            if (lines.Length > maxLines)
            {
                int cut = lines.Length - maxLines;
               logText.text = string.Join("\n", lines, cut, lines.Length - cut);
            }

                // If content is taller than the viewport, adjust scroll position
                if (contentHeight > viewportHeight)
            {
                // Scroll to the bottom of the last line
                float scrollPosition = (contentHeight - viewportHeight) / contentHeight;
                if(1 - scrollPosition <= 0.4)
                {
                    scrollRect.verticalNormalizedPosition = 0;
                }
                else
                scrollRect.verticalNormalizedPosition = 1 - scrollPosition;
                //Debug.Log(scrollPosition);
            }
              
            else
            {
                // Content fits within viewport; keep scroll position at the top
                scrollRect.verticalNormalizedPosition = 1;
            }
        }
    }
}

